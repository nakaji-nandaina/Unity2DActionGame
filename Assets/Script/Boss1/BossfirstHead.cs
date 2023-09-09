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
    private void Start()
    {
        this.gameObject.AddComponent<EnemyShotManager>();
        enemyshot = this.gameObject.GetComponent<EnemyShotManager>();
        speed = 5f;
        currentT = 0;
        nextT = 1f;
        rb = this.GetComponent<Rigidbody2D>();
        bossfirst = boss.GetComponent<BossEnemyfirst>();
        
        dir.x = Random.Range(-1f, 1f);
        dir.y = Random.Range(-1f, 1f);
    }
    private void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal) return;
        switch (bossfirst.currentState)
        {
            case BossEnemyfirst.BossState.Battle:
                battle();
                break;
        }
    }

    private void battle()
    {
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Hansya:
                rb.velocity = new Vector2(dir.x, dir.y).normalized * speed;
                break;
            case BossEnemyfirst.BattleState.Last:
                rb.velocity = new Vector2(0, 0);
                currentT += Time.deltaTime;
                if (currentT < nextT) return;
                nextT = Random.Range(0.5f, 1f);
                currentT = 0f;
                Shot();
                break;
        }
    }

    private void Shot()
    {
        //Debug.Log(bossfirst.BodyShotWeapon);
        Vector3 Ppos = GameManager.instance.Player.transform.position;
        Vector3 Tpos = this.gameObject.transform.position;
        Vector2 attackDir = Ppos - Tpos;
        enemyshot.EmemyShot(Ppos, Tpos, attackDir, bossfirst.BodyShotWeapon);
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

    public void takeDamage(int at)
    {
        if (bossfirst.currentState != BossEnemyfirst.BossState.Battle) return;
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Syukai:
                bossfirst.TakeDamage(at);
                break;
            case BossEnemyfirst.BattleState.Hansya:
                break;
            case BossEnemyfirst.BattleState.Last:
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
