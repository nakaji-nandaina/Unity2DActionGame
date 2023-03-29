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

    [SerializeField]
    public Animator playerAnim;

    public Rigidbody2D rb;

    [SerializeField]
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
    public bool kaiwaNow;
    public bool isMove;
    private bool isDead;
    public GameObject[] myWeapon;

    [SerializeField]
    private AudioClip levelClip;
    [SerializeField]
    private GameObject levelText;
    private float leveluptime =1.0f;
    private float levelupcount;

    public InventoryObject inventory;
    public InventoryObject ShortCut;
    public MySkills skills;
    public DataBase database;
    private List<int> itemId;
    private List<int> itemAmount;

    private float[] buffStatus = new float[4];

    void Start()
    {
        currentXP = GameManager.currentXP;
        nextXP = GameManager.nextXP;
        currentLevel = GameManager.currentlevel;
        kaiwaNow = false;
        isMove = false;
        maxHealth = GameManager.maxHealth;
        SetStatus();
        currentHealth = GameManager.currentHealth;
        levelupcount = 0;
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            currentHealth = GameManager.maxHealth;
        }
        GameManager.instance.UpdateHealthUI();
        GameManager.instance.UpdateXPUI();
        ShotManager=GetComponent<PlayerShotManager>();
        GameManager.instance.inventoryUI.UpdateShortCutInventoryUI(ShortCut);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelupcount > 0)
        {
            levelupcount -= Time.deltaTime;
            if (levelupcount <= 0)
            {
                levelText.SetActive(false);
            }
        }
        if (!kaiwaNow&&!isDead&&!isMove)
        {
            if (invincibilityCounter > 0)
            {
                invincibilityCounter -= Time.deltaTime;
            }
            if (isknockingback)
            {
                knockbackCounter -= Time.deltaTime;
                rb.velocity = knockbackForce * knockDir;

                if (knockbackCounter <= 0)
                {
                    isknockingback = false;
                }
                else
                {
                    return;
                }
            }
            Move();
            Attack();
            if (attackCounter > 0)
            {
                attackCounter -= Time.deltaTime;
            }
            if (attackCounter < 0)
            {
                attackCounter = 0;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SavePlayer();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayer();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!GameManager.instance.inventoryUI.InventoryBox.activeInHierarchy)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
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
        GameManager.instance.Playerstop();
    }
    public void CloseInventory()
    {
        GameManager.instance.inventoryUI.InventoryBox.SetActive(false);
        GameManager.instance.inventoryUI.CloseInventory();
        GameManager.instance.Playerstart();
    }
    public void SavePlayer()
    {
        itemId = database.GetItemIds(inventory);
        itemAmount = database.GetItemAmounts(inventory);
        PlayerStatus.GetInstance().ReStatus(currentXP, GameManager.currentMoney, currentLevel,itemId,itemAmount);
        PlayerStatus.GetInstance().Save();
        Debug.Log(PlayerStatus.GetInstance().currentLv);
    }

    public void LoadPlayer()
    {
        PlayerStatus.GetInstance().Load();
        Debug.Log(PlayerStatus.GetInstance().currentLv);
        currentLevel = PlayerStatus.GetInstance().currentLv;
        currentXP = PlayerStatus.GetInstance().currentXp;
        nextXP = database.playerLvDatabase[currentLevel - 1].NextXP;
        SetStatus();
        currentHealth = maxHealth;
        GameManager.currentMoney = PlayerStatus.GetInstance().Gold;


        inventory = InventoryObject.CreateInstance<InventoryObject>();
        itemId = PlayerStatus.GetInstance().itemIds;
        itemAmount = PlayerStatus.GetInstance().itemAmounts;
        inventory.SetInitiate(itemId, itemAmount, database);
        
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
            if(currentHealth == 0)
            {
                //gameObject.SetActive(false);
                isDead = true;
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
            if (attackCounter <= 0)
            {
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        playerAnim.SetFloat("X", 1f);
                        playerAnim.SetFloat("Y", 0);

                        //weaponAnim.SetFloat("X", 1f);
                        //weaponAnim.SetFloat("Y", 0);
                    }
                    else
                    {
                        playerAnim.SetFloat("X", -1f);
                        playerAnim.SetFloat("Y", 0);

                        //weaponAnim.SetFloat("X", -1f);
                        //weaponAnim.SetFloat("Y", 0);
                    }
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    playerAnim.SetFloat("X", 0);
                    playerAnim.SetFloat("Y", 1f);

                    //weaponAnim.SetFloat("X", 0);
                    //weaponAnim.SetFloat("Y", 1f);
                }
                else
                {
                    playerAnim.SetFloat("X", 0);
                    playerAnim.SetFloat("Y", -1f);

                    //weaponAnim.SetFloat("X", 0);
                    //weaponAnim.SetFloat("Y", -1f);
                }
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
            ShotManager.ShotAttack(this.transform.position,mousePos,attackDir,myWeapon[0],at,kbforce);

        }
    }

}
