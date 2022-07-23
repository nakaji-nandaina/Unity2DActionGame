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
    [SerializeField]
    private float rangeToChase,rangeToWalk,chaseWaitTime,chaseTime,rangeToAttack;
    private float chaseCounter, chaseWaitCounter;

    [SerializeField]
    private float waitAfterHitting;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private float attackTime, attackCounter;
    [SerializeField]
    private float isAttackTime, isAttackCounter;
    [SerializeField]
    private int xp=10;
    [SerializeField]
    private bool longAt=false;


    [SerializeField]
    private BoxCollider2D area;

    [SerializeField]
    private int currentHealth;

    private bool isKnockingBack;

    [SerializeField]
    private float knockBackTime,knockBackForce;

    private float knockBackCounter;

    private Vector2 knockDir;

    private bool isDead;

    NavMeshAgent2D agent;
    private bool isChaseing;
    
    private BreakObj breakObj;
    [SerializeField]
    private GameObject damageUI;

    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>();
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        waitCounter = waitTime;
        chaseWaitCounter = chaseWaitTime;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        breakObj = GetComponent<BreakObj>();
        isAttackCounter = isAttackTime;
        attackCounter = attackTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            
            if (isKnockingBack)
            {
                if (knockBackCounter > 0)
                {
                    knockBackCounter -= Time.deltaTime;
                    rb.velocity = knockDir * knockBackForce;
                    
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    isKnockingBack = false;

                }
                return;
            }

            if (Vector3.Distance(transform.position, playerPos.position) < rangeToChase)
            {
                isChaseing = true;
            }
            if (Vector3.Distance(transform.position, playerPos.position) >= rangeToWalk)
            {
                isChaseing = false;
            }

            if (!isChaseing&& attackCounter == attackTime)
            {
                enemyAnim.SetFloat("Y", -1.0f);
                enemyAnim.SetFloat("X", 0.0f);
                notChase();
            }
            if(isChaseing|| attackCounter != attackTime)
            {
                MakeDir();
                Chase();
            }
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
                Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);

            /*if (moveDir.x < 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
            */
        }
    }

    private void notChase()
    {
        if (waitCounter >= 0)
        {
            waitCounter -= Time.deltaTime;
            rb.velocity = Vector2.zero;
            if (waitCounter <= 0)
            {
                moveCounter = walkTime;
                
                moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                moveDir.Normalize();
            }
        }
        else
        {
            moveCounter -= Time.deltaTime;
            rb.velocity = moveDir * moveSpeed;
            if (moveCounter <= 0)
            {
                waitCounter = waitTime;
            }
        }
    }
    private void Chase()
    {
        if (longAt && (Vector3.Distance(transform.position, playerPos.position) < rangeToAttack || attackCounter!=attackTime))
        {
            LongAttack();
        }
        else if (chaseWaitCounter >= 0)
        {
            chaseWaitCounter -= Time.deltaTime;
            rb.velocity = Vector2.zero;
            if (chaseWaitCounter <= 0)
            {
                chaseCounter = chaseTime;
                moveDir = playerPos.position-this.transform.position;
                moveDir.Normalize();
            }
        }
        else
        {
            chaseCounter -= Time.deltaTime;
            //rb.velocity = moveDir * chaseSpeed;
            agent.Trace(this.transform.position, playerPos.position, chaseSpeed);
            if (chaseCounter <= 0)
            {
                chaseWaitCounter = chaseWaitTime;
            }
        }
    }

    private void LongAttack()
    {
        rb.velocity = Vector2.zero;
        enemyAnim.SetBool("LongAttack", true);
        if (attackCounter<=0)
        {
            if (isAttackCounter == isAttackTime)
            {

            }
            if (isAttackCounter <= 0)
            {
                isAttackCounter = isAttackTime;
                attackCounter = attackTime;
                enemyAnim.SetBool("LongAttack", false);
            }
            else
            {
                isAttackCounter -= Time.deltaTime;
            }
        }
        else
        {
            attackCounter -= Time.deltaTime;
        }

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
            if (isChaseing)
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
            if (isChaseing)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);
            }
        }
    }
    public void KnockBack(Vector3 position)
    {
        isKnockingBack = true;
        knockBackCounter = knockBackTime;
        knockDir = transform.position - position;
        knockDir.Normalize();
    }
    public void TakeDamage(int damage,Vector3 position)
    {
        if (!isDead)
        {

            currentHealth -= damage;
            GameObject DamageObj = Instantiate(damageUI,playerPos.position, Quaternion.Euler(0,0,0));
            DamageObj.GetComponent<DamageUI>().DamageSet(damage, playerPos.position,this.gameObject);
            if (currentHealth <= 0)
            {
                //rb.velocity = Vector2.zero;
                //Destroy(this.GetComponent<Animator>());
                //explodable.explode();
                //Destroy(this.GetComponent<Rigidbody2D>());
                //Destroy(this.GetComponent<Animator>());
                //Destroy(this.GetComponent<BoxCollider2D>());
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().KillEnemy(xp);
                isDead = true;
                rb.velocity = Vector2.zero;
                breakObj.BreakThis();
                //Destroy(gameObject);
            }
            KnockBack(position);
        }
    }
}
