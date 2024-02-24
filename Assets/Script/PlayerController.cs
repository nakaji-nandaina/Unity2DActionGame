using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    private GameObject weaponHolder;
    private GameObject weaponObj;
    private Sprite defaultWeaponSprite;
    private Animator weaponAnim;
    private PlayerShotManager ShotManager;
    Vector3 shotRotate;

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
    //ノックバック時間
    [SerializeField]
    private float knockbackTime, knockbackForce;
    private float knockbackCounter;
    //無敵時間
    [SerializeField]
    private float invicibilityTime;
    private float invincibilityCounter;
    //回避移動
    private float avoidTime=0.2f;
    private float avoidCounter;
    private float avoidForce = 20f;
    private float avoidDistime = 0.5f;
    private float avoidDisCounter;
    private float avoidEffectTime = 0.05f;
    private float avoidEffectCounter;
    private Vector2 avoidDir;
    //攻撃時間
    private float attackTime = 0.5f;
    private float attackCounter;
    private float attackAnimTime = 0.5f;
    private float attackAnimCounter;
    public bool kaiwaNow;
    //private bool isDead;
    public WeaponData weapon;

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
    public NS ns;

    private float GameOverTime=4f;
    private float GameOverCounter;
    public AudioClip deadclip;


    private KeyCode[] numkey = new KeyCode[]
    {
        KeyCode.Alpha1, KeyCode.Alpha2,
        KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7
    };


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
                playerAnim.SetTrigger("Dead");
                GameManager.instance.GameOverUI.SetActive(true);
                GameManager.instance.GameOverUI.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                GameManager.instance.StopBGM();
                GameManager.instance.PlayAudio(deadclip);
                GameOverCounter = 0;

                ps = nextState;
                break;
        }

    }
    public enum NS
    {
        free,
        kb,
        avoid,
    }
    public void changeNS(NS nextState)
    {
        switch (nextState) {
            case NS.free:
                if (ns == NS.avoid) avoidDisCounter = avoidDistime;
                ns = nextState;
                break;
            case NS.kb:
                ns = nextState;
                break;
            case NS.avoid:
                avoidEffectCounter = 0;
                avoidCounter = avoidTime;
                invincibilityCounter = avoidTime/2;
                ns = nextState;
                break;

        }

    }

    void Start()
    {
        levelClip = GameManager.instance.clips[1];
        weaponHolder = this.gameObject.transform.GetChild(0).gameObject;
        weaponAnim = weaponHolder.GetComponent<Animator>();
        weaponObj = weaponHolder.transform.GetChild(0).gameObject;
        weaponObj.AddComponent<Sword>();
        ImgWeapon = weaponObj.GetComponent<SpriteRenderer>();
        defaultWeaponSprite = ImgWeapon.sprite;
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
        //weaponHolder.transform.localEulerAngles =new Vector3(0,0,90);
        switch (ps){
            case PS.dead:
                rb.velocity = Vector2.zero;
                if (GameOverCounter >= GameOverTime) return;
                GameOverCounter += Time.deltaTime;
                GameManager.instance.GameOverUI.GetComponent<Image>().color = new Color(0, 0, 0, GameOverCounter / (GameOverTime/1.2f));
                if (GameOverCounter < GameOverTime) return;
                GameManager.instance.GameOverUI.transform.Find("DeadText").gameObject.SetActive(true);
                GameManager.instance.GameOverUI.transform.Find("DeadButton").gameObject.SetActive(true);
                GameManager.instance.GameOverUI.transform.Find("DeadButton").gameObject.GetComponent<Button>().onClick.AddListener(ToStart);

                return;
            case PS.normal:
                Normal();
                
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
    private void ToStart()
    {
        LoadPlayer();
        GameManager.StartSpone = new Vector2(-38, -22);
        FadeManager.Instance.LoadScene("StartScene", 1f);
    }

    private void Normal()
    {
        if (invincibilityCounter > 0) invincibilityCounter -= Time.deltaTime;
        switch (ns)
        {
            case NS.free:
                Move();
                Attack();
                Avoid();
                UseShortCut();
                break;
            case NS.kb:
                knockbackCounter -= Time.deltaTime;
                rb.velocity = knockbackForce * knockDir;
                if (knockbackCounter <= 0) changeNS(NS.free);
                return;
            case NS.avoid:
                avoidCounter -= Time.deltaTime;
                rb.velocity = avoidForce * avoidDir;
                if (avoidCounter <= 0) changeNS(NS.free);
                avoidEffectCounter -= Time.deltaTime;
                if (avoidEffectCounter > 0) return;
                avoidEffectCounter = avoidEffectTime;
                GameObject avoideffect = new GameObject("avoid");
                avoideffect.AddComponent<AvoidEffect>();
                avoideffect.transform.position = this.gameObject.transform.position;
                return;
        }
        if (attackCounter > 0) attackCounter -= Time.deltaTime;
        if (attackCounter < 0) attackCounter = 0;
        if (attackAnimCounter > 0) attackAnimCounter -= Time.deltaTime;
        if (attackAnimCounter < 0) attackAnimCounter = 0;
        if (avoidDisCounter > 0) avoidDisCounter -= Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.O))SavePlayer();
        //if (Input.GetKeyDown(KeyCode.L))LoadPlayer();
        if (Input.GetKeyDown(KeyCode.O)) OpenWeaponPouch();
        if (Input.GetKeyDown(KeyCode.I)) OpenInventory();
    }

    private void Avoid()
    {
        if (avoidDisCounter > 0) return;
        if (attackAnimCounter > 0) return;
        if (rb.velocity.normalized == Vector2.zero) return;
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        avoidDir = rb.velocity.normalized;
        changeNS(NS.avoid);
        playerAnim.SetTrigger("IsAttack");
    }
    public void UseShortCut()
    {
        for (int i = 0; i < 7; i++)
        {
            if (!Input.GetKeyDown(numkey[i])) continue;
            Debug.LogError(i);
            GameManager.instance.inventoryUI.ShortcutUseItem(i, inventory, ShortCut);
            break;
        }
    }

    public void UseItem()
    {
        GameManager.instance.inventoryUI.useItem(inventory);
    }

    public void HealPlayer(int healP)
    {
        Debug.LogError("heal");
        GameManager.instance.PlayAudio(GameManager.instance.clips[0]);
        currentHealth += healP;
        currentHealth = currentHealth < maxHealth ? currentHealth : maxHealth ;
        GameManager.instance.UpdateHealthUI();
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
        Debug.Log(GameManager.instance.weaponUI);
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
            changeNS(NS.kb);
            //isknockingback = true;
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
                //ps=PS.dead;
                changePS(PS.dead);
                //Destroy(this.gameObject.GetComponent<SpriteRenderer>());
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
        if (attackCounter > 0) return;
        //weaponHolder.transform.localEulerAngles = shotRotate;
        if (Input.GetMouseButtonDown(0)||(Input.GetMouseButton(0)&&weapon.nagaoshi))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 attackDir = mousePos - this.transform.position;
            ImgWeapon.sprite = weapon.ImgWeapon;
            weaponAnim.applyRootMotion = false;
            weaponHolder.transform.localEulerAngles = new Vector3(0, 0, 0);
            attackTime = weapon.DisTime;
            if (attackAnimTime > attackTime) attackAnimCounter = attackTime;
            else attackAnimCounter = attackAnimTime;
            //Debug.Log(attackDir);
            attackCounter = attackTime;
            playerAnim.SetTrigger("IsAttack");

            if (attackDir.x < 0) weaponObj.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else weaponObj.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            if (Mathf.Abs(attackDir.x) <= Mathf.Abs(attackDir.y))
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
            //weaponAnim.SetTrigger("Attack");
            if (weapon.kinsetu)
            {
                weaponAnim.SetTrigger("Attack");
                Debug.LogError(ImgWeapon.bounds.size.ToString());
                weaponObj.transform.localPosition = new Vector3(0, ImgWeapon.bounds.size.y*0.55f,0);
                weaponObj.GetComponent<BoxCollider2D>().enabled = true;
                weaponObj.GetComponent<BoxCollider2D>().size = new Vector2(ImgWeapon.bounds.size.x,ImgWeapon.bounds.size.y);
                weaponObj.GetComponent<Sword>().WD = this.weapon;
                GameManager.instance.PlayAudio(weapon.ShotSound);
                Debug.LogError(weaponObj.transform.position);
                return;
            }
            if (weapon.yumi)
            {
                weaponAnim.applyRootMotion = true;
                weaponAnim.SetTrigger("Yumi");
                
                weaponHolder.transform.localEulerAngles = new Vector3(0,0,Mathf.Atan2(attackDir.normalized.y, attackDir.normalized.x) * Mathf.Rad2Deg -90);
            }
            weaponAnim.SetTrigger("Attack");
            weaponObj.GetComponent<BoxCollider2D>().enabled = false;
            weaponObj.transform.localPosition = new Vector2(0, 0.8f);
            ShotManager.ShotAttack(this.transform.position, attackDir, weapon.shot, at, kbforce,weapon);
        }
    }

}
