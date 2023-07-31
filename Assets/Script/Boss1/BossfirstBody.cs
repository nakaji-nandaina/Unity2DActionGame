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
    private float currentT = 0f;
    private float nextT = Random.Range(3f, 5f);
    private void Start()
    {

        bossfirst = boss.GetComponent<BossEnemyfirst>();
        enemyshot = this.gameObject.GetComponent<EnemyShotManager>();
        if (enemyshot == null)
        {
            this.gameObject.AddComponent<EnemyShotManager>();
            enemyshot= this.gameObject.GetComponent<EnemyShotManager>();
        }
    }
    private void Update()
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
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
        }
    }

    private void Shot()
    {
        Debug.Log(bossfirst.BodyShotWeapon);
        Vector3 Ppos=GameManager.instance.Player.transform.position;
        Vector3 Tpos = this.gameObject.transform.position;
        Vector2 attackDir = Ppos - Tpos;
        enemyshot.EmemyShot(Ppos, Tpos, attackDir, bossfirst.BodyShotWeapon);
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bossfirst.playerhit();
        }
        if (collision.gameObject.tag == "Wall")
        {
            bossfirst.ReflectDir();
            Debug.Log("hit");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bossfirst.playerhit();
        }
        if (collision.gameObject.tag == "Wall")
        {
            bossfirst.ReflectDir();
            Debug.Log("hit");
        }
    }
    */
}
