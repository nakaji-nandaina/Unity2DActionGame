using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    

    private Rigidbody2D rb;
    private Animator enemyAnim;

    private Transform playerPos;

    Vector2 Dir;

    [SerializeField]
    private float moveSpeed,waitTime,walkTime;
    private float moveCounter, waitCounter;
    private Vector2 moveDir;
    [SerializeField, Tooltip("追いかけ速度")]
    private float chaseSpeed;
    [SerializeField,Tooltip("直線追いかけ")]
    private bool straght=false;
    [SerializeField]
    private float rangeToChase,rangeToWalk,chaseWaitTime,chaseTime,rangeToAttack;
    private float chaseCounter, chaseWaitCounter;

    [SerializeField, Tooltip("攻撃力")]
    private int attackDamage;
    [SerializeField, Tooltip("攻撃間隔")]
    private float attackTime, attackCounter;
    [SerializeField, Tooltip("攻撃後インターバル")]
    private float isAttackTime;
    [SerializeField]
    private float isAttackCounter;
    [SerializeField, Tooltip("経験値")]
    private int xp=10;
    [SerializeField, Tooltip("遠距離攻撃？")]
    private bool longAt=false;
    [SerializeField, Tooltip("遠距離攻撃の連続回数")]
    private int multiAt = 1;
    private int multiAtcount;

    [SerializeField, Tooltip("連続攻撃間隔")]
    private float multiAttime = 1f;

    [SerializeField]
    private GameObject attackObj;

    [SerializeField]
    private BoxCollider2D area;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private float knockBackTime,knockBackForce;
    private float knockBackPForce;
    private float knockBackCounter;

    private Vector2 knockDir;
    NavMeshAgent2D agent;
    private EnemyShotManager ShotManager;
    private BreakObj breakObj;
    [SerializeField]
    private GameObject damageUI;

    public enum EnemyState
    {
        Longshot,
        NotChase,
        Wait,
        Move,
        KnockBack,
        Dead,
    }

    public EnemyState ES;

    public enum NCState { 
        walk,
        wait,
    }

    public NCState NCS;

    public void ChangeES(EnemyState next)
    {
        switch (next) 
        {
            case EnemyState.Wait:
                chaseWaitCounter = chaseWaitTime;
                ES = next;
                break;
            case EnemyState.Move:
                chaseCounter = chaseTime;
                ES = next;
                break;
            case EnemyState.Longshot:
                enemyAnim.SetBool("LongAttack", true);
                ES = next;
                break;
            case EnemyState.KnockBack:
                ES = next;
                break;
            case EnemyState.NotChase:
                enemyAnim.SetFloat("X", 0.0f);
                enemyAnim.SetFloat("Y", -1.0f);
                NCS = NCState.wait;
                waitCounter = waitTime;
                ES = next;
                break;
            case EnemyState.Dead:
                rb.velocity = Vector2.zero;
                ES = next;
                break;
        }

    }

    void Start()
    {
        NCS = NCState.wait;
        ES = EnemyState.NotChase;
        agent = GetComponent<NavMeshAgent2D>();
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        waitCounter = waitTime;
        chaseWaitCounter = chaseWaitTime;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        breakObj = GetComponent<BreakObj>();
        isAttackCounter = isAttackTime;
        attackCounter = attackTime;
        ShotManager = GetComponent<EnemyShotManager>();
        multiAtcount = multiAt;
        area = GameObject.FindGameObjectWithTag("EnemyArea").GetComponent<BoxCollider2D>();
    }
    
    void knockback()
    {
        knockBackCounter -= Time.deltaTime;
        if (knockBackCounter <= 0)
        {
            ChangeES(EnemyState.Move);
            return;
        }
        rb.velocity = knockDir * knockBackPForce;
    }

    private void move()
    {
        if (longAt && Vector3.Distance(transform.position, playerPos.position) < rangeToAttack)
        {
            ChangeES(EnemyState.Longshot);
            return;
        }
        if (chaseCounter <= 0)
        {
            ChangeES(EnemyState.Wait);
            return;
        }
        MakeDir();
        chaseCounter -= Time.deltaTime;
        if (straght) rb.velocity = moveDir * chaseSpeed;
        else agent.Trace(this.transform.position, playerPos.position, chaseSpeed);
    }

    void wait()
    {
        if (chaseWaitCounter <= 0)
        {
            ChangeES(EnemyState.Move);
            return;
        }
        MakeDir();
        chaseWaitCounter -= Time.deltaTime;
        rb.velocity = Vector2.zero;
    }

    void toNotChase()
    {
        if(Vector3.Distance(transform.position, playerPos.position) >= rangeToWalk)
        {
            ChangeES(EnemyState.NotChase);
            return;
        }
    }

    void NotChase()
    {
        if(Vector3.Distance(transform.position, playerPos.position) < rangeToChase)
        {
            ChangeES(EnemyState.Move);
            return;
        }
        switch (NCS) {
            case NCState.walk:
                moveCounter -= Time.deltaTime;
                rb.velocity = moveDir * moveSpeed;
                if (moveCounter > 0) return;
                waitCounter = waitTime;
                NCS = NCState.wait;
                break;

            case NCState.wait:
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;
                if (waitCounter > 0) return;
                moveCounter = walkTime;
                moveDir= new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                moveDir.Normalize();
                NCS = NCState.walk;
                break;
        }
    }

    void LongShot()
    {
        rb.velocity = Vector2.zero;
        attackCounter -= Time.deltaTime;
        if (attackCounter > 0) return;
        if (isAttackCounter == isAttackTime)
        {
            Vector2 attackDir= playerPos.position - this.transform.position;
            isAttackCounter -= Time.deltaTime;
            ShotManager.EmemyShot(playerPos.position, this.gameObject.transform.position, attackDir, attackObj);
            if (multiAtcount <= 1) return;
            multiAtcount--;
            attackCounter = multiAttime;
            isAttackCounter = isAttackTime;
            return;
        }
        else if (isAttackCounter <= 0)
        {
            isAttackCounter = isAttackTime;
            multiAtcount = multiAt;
            attackCounter = attackTime;
            enemyAnim.SetBool("LongAttack", false);
            ChangeES(EnemyState.Move);
            return;
        }
        isAttackCounter -= Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        switch (ES)
        {
            case EnemyState.NotChase:
                NotChase();
                break;
            case EnemyState.Dead:
                rb.velocity = Vector2.zero;
                return;
            case EnemyState.Wait:
                wait();
                toNotChase();
                break;
            case EnemyState.Move:
                move();
                toNotChase();
                break;
            case EnemyState.KnockBack:
                knockback();
                return;
            case EnemyState.Longshot:
                LongShot();
                break;
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
            Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);
    }    

    private void MakeDir()
    {
        Dir = playerPos.position - this.transform.position;
        if (Mathf.Abs(Dir.x) <= Mathf.Abs(Dir.y))
        {
            if (Dir.y<=0f)
            {
                enemyAnim.SetFloat("X", 0.0f);
                enemyAnim.SetFloat("Y", -1.0f);
            }
            else
            {
                enemyAnim.SetFloat("X", 0.0f);
                enemyAnim.SetFloat("Y", 1.0f);
            }
        }
        else
        {
            if (Dir.x < 0f)
            {
                enemyAnim.SetFloat("X", -1.0f);
                enemyAnim.SetFloat("Y", 0.0f);
            }
            else
            {
                enemyAnim.SetFloat("X", 1.0f);
                enemyAnim.SetFloat("Y", 0.0f);
            }
        }
        

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (ES!=EnemyState.Dead)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);
            }
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ES != EnemyState.Dead) 
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);
            }
        }
    }
    public void KnockBack(Vector3 position,float force)
    {
        knockBackCounter = knockBackTime;
        knockDir = transform.position - position;
        knockDir.Normalize();
        knockBackPForce = knockBackForce + force;
        ChangeES(EnemyState.KnockBack);
    }
    public void TakeDamage(int damage,Vector3 position,float kbforce)
    {
        if (ES == EnemyState.Dead) return;
        int culDamage = Random.Range((int)(damage * 0.8), (int)(damage * 1.3));
        currentHealth -= culDamage;
        GameObject DamageObj = Instantiate(damageUI, playerPos.position, Quaternion.Euler(0, 0, 0));
        DamageObj.GetComponent<DamageUI>().DamageSet(culDamage, playerPos.position, this.gameObject);
        if (currentHealth <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().KillEnemy(xp);
            ChangeES(EnemyState.Dead);
            rb.velocity = Vector2.zero;
            breakObj.BreakThis();
            return;
        }
        KnockBack(position, kbforce);
    }
}
