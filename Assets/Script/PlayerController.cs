using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField,Tooltip("移動速度")] 
    private int moveSpeed;
    
    //方向
    private bool IsLeft;
    private bool IsRight;
    private bool IsFront;
    private bool IsBack;

    //[SerializeField]
    public Animator playerAnim;

    public Rigidbody2D rb;

    private SpriteRenderer ImgWeapon;
    private Animator weaponAnim;
    private PlayerShotManager ShotManager;

    //ステータス
    [System.NonSerialized]
    public int currentHealth;
    public int maxHealth;
    public int currentXP;
    public int nextXP;
    public int currentLevel;
    public int at;
    public float kbforce=0.1f;

    private bool isknockingback;
    private Vector2 knockDir;

    [SerializeField]
    private float knockbackTime, knockbackForce;
    private float knockbackCounter;

    [SerializeField]
    private float invicibilityTime;
    private float invincibilityCounter;

    private float attackTime = 0.5f;
    private float attackCounter;
    private float attackAnimTime = 0.5f;
    private float attackAnimCounter;
    public bool kaiwaNow;
    //private bool isDead;
    public WeaponData weapon;

    [SerializeField]
    private AudioClip levelClip;
    [SerializeField]
    private GameObject levelText;
    private float leveluptime =1.0f;
    private float levelupcount;

    public InventoryObject inventory;
    public InventoryObject ShortCut;
    public MySkills skills;
    public WeaponPouch weaponPouch;
    //データ保存用
    public DataBase database;
    public List<int> itemId;
    public List<int> shortcutId;
    public List<int> itemAmount;
    public List<int> skillId;
    public List<int> skillLvs;
    public List<int> weaponId;
    public int mainWeapon=0;

    private List<int> defnum = new List<int>() { 0, 0, 0, 0, 0, 0, 0 }; 
    private float[] buffStatus = new float[4];
    public PS ps;

    public enum PS
    {
        normal,
        stop,
        conversation,
        inventory,
        weapon,
        dead,
    }
    public void changePS(PS nextState)
    {
        switch (nextState) {
            case PS.normal:
                playerAnim.SetBool("IsMove", true);
                ps = nextState;
                break;
            case PS.stop:
                rb.velocity = Vector2.zero;
                ps = nextState;
                break;
            case PS.conversation:
                rb.velocity = Vector2.zero;
                playerAnim.SetBool("IsMove", false);
                ps = nextState;
                break;
            case PS.inventory:
                rb.velocity = Vector2.zero;
                ps = nextState;
                break;
            case PS.weapon:
                rb.velocity = Vector2.zero;
                ps = nextState;
                break;
            case PS.dead:
                rb.velocity = Vector2.zero;
                ps = nextState;
                break;
        }

    }

    void Start()
    {
        GameObject weaponhold = this.gameObject.transform.GetChild(0).gameObject;
        weaponAnim = weaponhold.GetComponent<Animator>();
        GameObject weaponObj = weaponhold.transform.GetChild(0).gameObject;
        ImgWeapon = weaponObj.GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
        SetPlayerInstance();
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            currentHealth = GameManager.maxHealth;
        }
        GameManager.instance.UpdateHealthUI();
        GameManager.instance.UpdateXPUI();
        ShotManager=GetComponent<PlayerShotManager>();
        GameManager.instance.inventoryUI.UpdateShortCutInventoryUI(ShortCut);
    }

    private void SetPlayerInstance()
    {
        ps = PS.normal;
        //インベントリのセット
        inventory = InventoryObject.CreateInstance<InventoryObject>();
        shortcutId = GameManager.shortcutId;
        itemId = GameManager.itemId;
        itemAmount = GameManager.itemAmount;
        inventory.SetInitiate(itemId, itemAmount, database);
        ShortCut.SetInitiateShortcut(shortcutId, database);
        GameManager.instance.inventoryUI.UpdateShortCutInventoryUI(ShortCut);
        //武器のセット
        weaponPouch = WeaponPouch.CreateInstance<WeaponPouch>();
        weaponId = GameManager.weaponId;
        mainWeapon = GameManager.mainWeapon;
        weaponPouch.SetInitiate(weaponId, database);
        weapon = weaponPouch.Pouch[mainWeapon];
        //スキルのセット
        skillId = GameManager.skillId;
        skillLvs = GameManager.skillLv;
        skills.SetInitiate(skillId, skillLvs, database);
        //ステータスのセット
        levelupcount = 0;
        currentXP = GameManager.currentXP;
        nextXP = GameManager.nextXP;
        currentLevel = GameManager.currentlevel;
        //maxHealth = GameManager.maxHealth;
        SetStatus();
        currentHealth = GameManager.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        switch (ps){
            case PS.dead:
                rb.velocity = Vector2.zero;
                return;
            case PS.normal:
                if (invincibilityCounter > 0)invincibilityCounter -= Time.deltaTime;
                if (isknockingback)
                {
                    knockbackCounter -= Time.deltaTime;
                    rb.velocity = knockbackForce * knockDir;
                    if (knockbackCounter <= 0)isknockingback = false;
                    return;
                }
                Move();
                Attack();
                if (attackCounter > 0)attackCounter -= Time.deltaTime;
                if (attackCounter < 0)attackCounter = 0;
                if (attackAnimCounter > 0) attackAnimCounter -= Time.deltaTime;
                if (attackAnimCounter < 0) attackAnimCounter = 0;
                //if (Input.GetKeyDown(KeyCode.O))SavePlayer();
                //if (Input.GetKeyDown(KeyCode.L))LoadPlayer();
                if (Input.GetKeyDown(KeyCode.O)) OpenWeaponPouch();
                if (Input.GetKeyDown(KeyCode.I)) OpenInventory();
                break;

            case PS.inventory:
                if (Input.GetKeyDown(KeyCode.I))
                {
                    CloseInventory();
                }
                break;
            case PS.weapon:
                if (Input.GetKeyDown(KeyCode.O))
                {
                    CloseWeaponPouch();
                }
                break;
        }

        if (levelupcount > 0)
        {
            levelupcount -= Time.deltaTime;
            if (levelupcount <= 0)
            {
                levelText.SetActive(false);
            }
        }

    }

    public void UseItem()
    {
        GameManager.instance.inventoryUI.useItem(inventory);
    }

    public void OpenInventory()
    {
        GameManager.instance.inventoryUI.InventoryBox.SetActive(true);
        GameManager.instance.inventoryUI.UpdateInventoryUI(inventory);
        changePS(PS.inventory);
    }
    public void CloseInventory()
    {
        GameManager.instance.inventoryUI.InventoryBox.SetActive(false);
        GameManager.instance.inventoryUI.CloseInventory();
        changePS(PS.normal);
    }

    public void OpenWeaponPouch()
    {
        GameManager.instance.weaponUI.WeaponPouchPanel.SetActive(true);
        GameManager.instance.weaponUI.InitiateWeaponPouch(weaponPouch);
        GameManager.instance.weaponUI.UpdateWeaponPouchUI(weaponPouch);
        changePS(PS.weapon);
    }

    public void CloseWeaponPouch()
    {
        GameManager.instance.weaponUI.WeaponPouchPanel.SetActive(false);
        GameManager.instance.weaponUI.UpdateWeaponPouchUI(weaponPouch);
        weapon = weaponPouch.Pouch[mainWeapon];
        changePS(PS.normal);
    }

    public void SavePlayer()
    {
        itemId = database.GetItemIds(inventory);
        shortcutId = database.GetItemIds(ShortCut);
        itemAmount = database.GetItemAmounts(inventory);
        skillId = database.GetSkillIds(skills);
        skillLvs = database.GetSkillLvs(skills);
        weaponId = database.GetWeaponIds(weaponPouch);

        PlayerStatus.GetInstance().ReStatus(currentXP, GameManager.currentMoney, currentLevel,itemId,shortcutId,itemAmount,skillId,skillLvs,weaponId,mainWeapon);
        PlayerStatus.GetInstance().Save();
        Debug.Log(PlayerStatus.GetInstance().currentLv);
    }

    public void LoadPlayer()
    {
        //Load
        PlayerStatus.GetInstance().Load();
        Debug.Log(PlayerStatus.GetInstance().currentLv);

        currentLevel = PlayerStatus.GetInstance().currentLv;
        currentXP = PlayerStatus.GetInstance().currentXp;
        nextXP = database.playerLvDatabase[currentLevel - 1].NextXP;
        GameManager.currentMoney = PlayerStatus.GetInstance().Gold;

        inventory = InventoryObject.CreateInstance<InventoryObject>();
        shortcutId = PlayerStatus.GetInstance().shortcutIds;
        itemId = PlayerStatus.GetInstance().itemIds;
        itemAmount = PlayerStatus.GetInstance().itemAmounts;
        skillId = PlayerStatus.GetInstance().skillIds;
        skillLvs = PlayerStatus.GetInstance().skillLvs;
        weaponId = PlayerStatus.GetInstance().weaponIds;
        mainWeapon = PlayerStatus.GetInstance().mainWeapon;

        if (weaponId.Count == 0)
        {
            mainWeapon = 0;
            weaponId.Add(0);
        }
        inventory.SetInitiate(itemId, itemAmount, database);
        ShortCut.SetInitiateShortcut(shortcutId, database);
        weaponPouch.SetInitiate(weaponId, database);
        weapon = weaponPouch.Pouch[mainWeapon];
        skills.SetInitiate(skillId, skillLvs, database);

        GameManager.instance.PlayerStateHold();
        SetStatus();
        currentHealth = maxHealth;
        GameManager.instance.inventoryUI.UpdateShortCutInventoryUI(ShortCut);
        GameManager.instance.UpdateHealthUI();
        GameManager.instance.UpdateXPUI();
        GameManager.instance.UpdateMoneyUI(GameManager.currentMoney);
    }

    public void SetStatus()
    {
        buffStatus = skills.culculateSkillEffects();
        float plusAt = buffStatus[0];
        float timesAt = buffStatus[1] + 1;
        float plusHp = buffStatus[2];
        float timesHp = buffStatus[3]+1;
        at = (int)(database.playerLvDatabase[currentLevel - 1].Attack*timesAt+plusAt);
        maxHealth = (int)(database.playerLvDatabase[currentLevel - 1].Hp * timesHp + plusHp);
    }

    /// <summary>
    /// プレイヤー吹き飛ばし処理
    /// </summary>
    /// <param name="position"></param>
    public void KnockBack(Vector3 position)
    {
        if (invincibilityCounter <= 0)
        {
            knockbackCounter = knockbackTime;
            isknockingback = true;
            knockDir = transform.position - position;
            knockDir.Normalize();
        }
    }
    /// <summary>
    /// プレイヤーダメージ処理
    /// </summary>
    /// 
    public void KillEnemy(int XP)
    {
        bool isLevelUpOnce=true;
        currentXP += XP;
        while (currentXP >= nextXP)
        {
            currentLevel++;
            SetStatus();
            currentHealth = maxHealth;
            currentXP -= nextXP;
            nextXP =database.playerLvDatabase[currentLevel-1].NextXP;
            if (isLevelUpOnce)
            {
                isLevelUpOnce = false;
                GameManager.instance.PlayAudio(levelClip);
                levelupcount = leveluptime;
                levelText.SetActive(true);
            }
        }
        GameManager.instance.UpdateXPUI();
        GameManager.instance.UpdateHealthUI();
        
    }

    public void DamagePlayer(int Damage)
    {
        if (invincibilityCounter <= 0)
        {
            currentHealth = Mathf.Clamp(currentHealth - Damage, 0, maxHealth);
            invincibilityCounter = invicibilityTime;
            if(currentHealth == 0&&ps!=PS.dead)
            {
                //gameObject.SetActive(false);
                ps=PS.dead;
                Destroy(this.gameObject.GetComponent<SpriteRenderer>());
            }
        }
        GameManager.instance.UpdateHealthUI();
        
    }
    private void Move()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;

        if (rb.velocity != Vector2.zero)
        {
            playerAnim.SetBool("IsMove", true);
            if (attackAnimCounter > 0) return;
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    playerAnim.SetFloat("X", 1f);
                    playerAnim.SetFloat("Y", 0);
                }
                else
                {
                    playerAnim.SetFloat("X", -1f);
                    playerAnim.SetFloat("Y", 0);
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 1f);
            }
            else
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", -1f);
            }
        }
        else
        {
            playerAnim.SetBool("IsMove", false);
        }

    }
    private void Attack()
    {

        if (Input.GetMouseButtonDown(0)&&attackCounter<=0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 attackDir = mousePos - this.transform.position;
            ImgWeapon.sprite = weapon.ImgWeapon;
            attackTime = weapon.DisTime;
            if (attackAnimTime > attackTime) attackAnimCounter = attackTime;
            else attackAnimCounter = attackAnimTime;
            //Debug.Log(attackDir);
            attackCounter = attackTime;
            playerAnim.SetTrigger("IsAttack");
            
            if(Mathf.Abs(attackDir.x) <= Mathf.Abs(attackDir.y))
            {
                if (attackDir.y <= 0f)
                {
                    playerAnim.SetFloat("X", 0);
                    playerAnim.SetFloat("Y", -1f);
                    weaponAnim.SetFloat("X", 0);
                    weaponAnim.SetFloat("Y", -1f);
                }
                else
                {
                    playerAnim.SetFloat("X", 0);
                    playerAnim.SetFloat("Y", 1f);
                    weaponAnim.SetFloat("X", 0);
                    weaponAnim.SetFloat("Y", 1f);
                }
            }
            else
            {
                if (attackDir.x < 0f)
                {
                    playerAnim.SetFloat("X", -1f);
                    playerAnim.SetFloat("Y", 0);
                    weaponAnim.SetFloat("X", -1f);
                    weaponAnim.SetFloat("Y", 0);
                }
                else
                {
                    playerAnim.SetFloat("X", 1f);
                    playerAnim.SetFloat("Y", 0);
                    weaponAnim.SetFloat("X", 1f);
                    weaponAnim.SetFloat("Y", 0);
                }
            }
            weaponAnim.SetTrigger("Attack");
            ShotManager.ShotAttack(this.transform.position, attackDir, weapon.shot, at, kbforce,weapon);
        }
    }

}
