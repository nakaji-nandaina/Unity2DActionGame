using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossfirstBody : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject boss;
    private BossEnemyfirst bossfirst;
    private EnemyShotManager enemyshot;
    private float currentT;  //0f
    private float nextT;     //rand
    Rigidbody2D rb;
    private float speed;     //5f
    private bool isJoint;
    Vector2 dir;
    private int maxhp;
    private int hp;
    Animator anim;
    float damagedTime = 0.2f;
    float damagedCount = 0f;
    [SerializeField]
    Sprite[] sprites;
    SpriteRenderer sr;

    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        bossfirst = boss.GetComponent<BossEnemyfirst>();
        enemyshot = this.gameObject.GetComponent<EnemyShotManager>();
        if (enemyshot == null)
        {
            this.gameObject.AddComponent<EnemyShotManager>();
            enemyshot= this.gameObject.GetComponent<EnemyShotManager>();
        }

        currentT = 0f;
        nextT = Random.Range(3f, 5f);
        dir.x = Random.Range(-1f,1f);
        dir.y = Random.Range(-1f, 1f);
        speed = 5f;
        isJoint = true;
        sr = GetComponent<SpriteRenderer>();
        maxhp = 200;
        hp = maxhp;
    }
    private void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        damagedCount = 0 > damagedCount - Time.deltaTime ? 0 : damagedCount - Time.deltaTime;
        spriteDir();
        switch (bossfirst.currentState) {
            case BossEnemyfirst.BossState.Battle:
                battle();
                break;
        }
    }

    private void battle()
    {
        currentT += Time.deltaTime;
        switch (bossfirst.battleState) {
            case BossEnemyfirst.BattleState.Syukai:
                if (currentT < nextT) return;
                nextT = Random.Range(3f, 5f);
                currentT = 0f;
                Shot();
                break;
            case BossEnemyfirst.BattleState.Hansya:
                if (isJoint)
                {

                    Destroy(GetComponent<DistanceJoint2D>());
                    isJoint = false;
                    //return;
                    Debug.Log("jointOut");
                }
                rb.velocity = new Vector2(dir.x, dir.y).normalized * speed;
                //éÀèo
                if (currentT < nextT) return;
                nextT = Random.Range(2f, 4f);
                currentT = 0f;
                Shot();
                break;
        }
    }
    private void spriteDir()
    {
        Vector3 Ppos = GameManager.instance.Player.transform.position;
        if (Ppos.y > this.transform.position.y)
        {
            if (Ppos.x < this.transform.position.x) sr.sprite = sprites[0];
            else sr.sprite = sprites[1];
            return;
        }
        if (Ppos.x < this.transform.position.x) sr.sprite = sprites[2];
        else sr.sprite = sprites[3];
    }
    private void Bound()
    {
        dir.x = Random.Range(-1f, 1f);
        dir.y = Random.Range(-1f, 1f);
    }

    private void Shot()
    {
        //Debug.Log(bossfirst.BodyShotWeapon);
        Vector3 Ppos=GameManager.instance.Player.transform.position;
        Vector3 Tpos = this.gameObject.transform.position;
        Vector2 attackDir = Ppos - Tpos;
        enemyshot.EmemyShot(Ppos, Tpos, attackDir, bossfirst.BodyShotWeapon,bossfirst.At);
    }

    private void hitOthers()
    {
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Hansya:
                Bound();
                break;
        }
    }

    public void takeDamage(int at,AudioClip clip)
    {
        if (bossfirst.currentState != BossEnemyfirst.BossState.Battle) return;
        if (damagedCount > 0) return;
        damagedCount = damagedTime;
        GameManager.instance.PlayAudio(clip);
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Syukai:
                bossfirst.TakeDamage(at);
                anim.SetTrigger("Damaged");
                break;
            case BossEnemyfirst.BattleState.Hansya:
                hp -= at;
                anim.SetTrigger("Damaged");
                if (hp <= 0)
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                bossfirst.playerhit(this.transform.position);
                hitOthers();
                break;

            case "Wall":
                hitOthers();
                break;

            default:
                hitOthers();
                break;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                bossfirst.playerhit(this.transform.position);
                hitOthers();
                break;

            case "Wall":
                hitOthers();
                break;

            default:
                hitOthers();
                break;
        }
    }
    
}
