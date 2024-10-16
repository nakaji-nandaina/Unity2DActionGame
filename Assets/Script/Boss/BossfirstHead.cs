using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossfirstHead : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject boss;
    private BossEnemyfirst bossfirst;
    private float currentT = 0f;
    private float nextT = 1f;
    Rigidbody2D rb;
    private float speed = 5f;
    Vector2 dir;
    private EnemyShotManager enemyshot;
    private Animator anim;
    float damagedTime=0.2f;
    float damagedCount=0f;
    [SerializeField]
    GameObject[] shot;
    [SerializeField]
    Sprite[] sprites;
    SpriteRenderer sr;
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        this.gameObject.AddComponent<EnemyShotManager>();
        enemyshot = this.gameObject.GetComponent<EnemyShotManager>();
        speed = 5f;
        currentT = 0;
        nextT = 1f;
        rb = this.GetComponent<Rigidbody2D>();
        bossfirst = boss.GetComponent<BossEnemyfirst>();
        sr = GetComponent<SpriteRenderer>();

        dir.x = Random.Range(-1f, 1f);
        dir.y = Random.Range(-1f, 1f);
    }
    private void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        damagedCount = 0 > damagedCount - Time.deltaTime ? 0 : damagedCount - Time.deltaTime;
        switch (bossfirst.currentState)
        {
            case BossEnemyfirst.BossState.Battle:
                battle();
                break;
        }
    }

    private void battle()
    {
        if(bossfirst.battleState!=BossEnemyfirst.BattleState.Last) spriteDir();
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Hansya:
                rb.velocity = new Vector2(dir.x, dir.y).normalized * speed;
                break;
            case BossEnemyfirst.BattleState.Last:
                rb.velocity = new Vector2(0, 0);
                this.gameObject.transform.Rotate(0, 0, 2f);
                currentT += Time.deltaTime;
                if (currentT < nextT) return;
                nextT = 0.1f;
                currentT = 0f;
                fourShot();
                break;
        }
    }

    private void spriteDir()
    {
        Vector3 Ppos = GameManager.instance.Player.transform.position;
        if (Ppos.y > this.transform.position.y)
        {
            if (Ppos.x < this.transform.position.x)sr.sprite = sprites[0];
            else sr.sprite = sprites[1];
            return;
        }
        if (Ppos.x < this.transform.position.x) sr.sprite = sprites[2];
        else sr.sprite = sprites[3];
    }

    private void fourShot()
    {
        for(int i = 0; i < 4; i++)
        {
            Vector2 dir = shot[i].gameObject.transform.position - this.gameObject.transform.position;
            enemyshot.EmemyShot(shot[i].gameObject.transform.position, this.gameObject.transform.position, dir, bossfirst.BodyShotWeapon,bossfirst.At);
        }
    }

    private void Shot()
    {
        //Debug.Log(bossfirst.BodyShotWeapon);
        Vector3 Ppos = GameManager.instance.Player.transform.position;
        Vector3 Tpos = this.gameObject.transform.position;
        Vector2 attackDir = Ppos - Tpos;
        enemyshot.EmemyShot(Ppos, Tpos, attackDir, bossfirst.BodyShotWeapon,bossfirst.At);
    }

    private void Bound()
    {
        dir.x = Random.Range(-1f, 1f);
        dir.y = Random.Range(-1f, 1f);
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
        GameManager.instance.PlayAudio(clip);
        damagedCount = damagedTime;
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Syukai:
                bossfirst.TakeDamage(at);
                anim.SetTrigger("Damaged");
                break;
            case BossEnemyfirst.BattleState.Hansya:
                break;
            case BossEnemyfirst.BattleState.Last:
                anim.SetTrigger("Damaged");
                bossfirst.TakeDamage(at);
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
