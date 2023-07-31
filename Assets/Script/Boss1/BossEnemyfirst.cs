using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyfirst : MonoBehaviour
{
    //ã§í 
    [SerializeField]
    private int hp;
    [SerializeField]
    private int at;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform playerPos;
    [SerializeField]
    private float encountRange = 10f;

    public enum BossState 
    { 
        Default,
        Encount,
        Battle,
        Muteki,
        Stop,
        Dead,
    }
    public BossState currentState = BossState.Default;

    //ç°âÒÇÃÇ›
    [SerializeField]
    private GameObject headobj;
    private Transform headPos;
    private bool Ishitwall=false;
    [SerializeField]
    private float firstSpeed = 2.0f;
    private float moveSpeed;
    [SerializeField]
    private float highestspeed = 15f;
    float dirx, diry = 0f;
    [SerializeField]
    private GameObject[] RoutePoint;
    private int nextPoint = 0;

    public GameObject BodyShotWeapon;

    public enum BattleState
    {
        Syukai,
        Hansya,
    }
    public BattleState battleState = BattleState.Syukai;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        rb = headobj.GetComponent<Rigidbody2D>();
        headPos = headobj.GetComponent<Transform>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        moveSpeed = firstSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Player.ps == PlayerController.PS.stop)
        {
            currentState = BossState.Stop;
        }
        switch (currentState) 
        {
            case BossState.Default:
                if(encountRange> Vector3.Distance(headPos.position, playerPos.position))
                {
                    ChangeState(BossState.Encount);
                    Debug.Log(Vector3.Distance(headPos.position, playerPos.position));
                }
                return;
            case BossState.Encount:
                ChangeState(BossState.Battle);
                return;
            case BossState.Battle:
                Battle();
                return;
            case BossState.Muteki:
                return;
            case BossState.Stop:
                return;
            case BossState.Dead:
                return;
        }

    }
    private void ChangeState(BossState nextstate)
    {
        currentState = nextstate;
        switch (nextstate) 
        {
            case BossState.Dead:
                DeadEffect();
                return;
            case BossState.Encount:
                EncountEffect();
                return;
            case BossState.Battle:
                FirstDir();
                return;
        }

    }
    private void EncountEffect()
    {

    }
    private void DeadEffect()
    {

    }
    private void FirstDir()
    {
        dirx = RoutePoint[nextPoint].transform.position.x - headPos.position.x;
        diry = RoutePoint[nextPoint].transform.position.y - headPos.position.y;
        rb.velocity = new Vector2(dirx, diry).normalized * moveSpeed;
    }

    public void ReflectDir()
    {
        float dx=rb.velocity.x;
        float dy=rb.velocity.y;
        
        if (rb.velocity.x < 0) dx += Random.Range(0.1f, 1f);
        else dx += Random.Range(-1f, -0.1f);
        if (rb.velocity.x < 0) dy += Random.Range(0.1f, 1f);
        else dy += Random.Range(-1f, -0.1f);
        
        rb.velocity = new Vector2(dx, dy).normalized * moveSpeed;
        dirx = dx;
        diry = dy;
    }

    public void syukaiDir()
    {
        if(1f>Vector3.Distance(headPos.position, RoutePoint[nextPoint].transform.position)){
            nextPoint = (nextPoint + 1) % 4;
            Debug.Log(nextPoint);
        }
        dirx = RoutePoint[nextPoint].transform.position.x - headPos.position.x;
        diry= RoutePoint[nextPoint].transform.position.y - headPos.position.y;
    }

    private void Battle()
    {
        switch (battleState)
        {
            case BattleState.Syukai:
                //Debug.Log("here");
                syukaiDir();
                rb.velocity = new Vector2(dirx, diry).normalized * moveSpeed;
                return;
            case BattleState.Hansya:
                rb.velocity = new Vector2(dirx, diry).normalized * moveSpeed;
                return;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState == BossState.Battle)
        {
            hp -= damage;
            if (damage <= 0)
            {
                ChangeState(BossState.Dead);
            }
        }
    }

    public void playerhit()
    {
        GameManager.instance.Player.KnockBack(transform.position);
        GameManager.instance.Player.DamagePlayer(at);
    }

    /*
    private void OnTrigEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.Player.KnockBack(transform.position);
            GameManager.instance.Player.DamagePlayer(at);
            ReflectDir();
        }
        if (collision.gameObject.tag == "Weapon")
        {
            if (currentState != BossState.Battle) return;
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            TakeDamage(weapon.attackDamage);
        }
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("hit");
            ReflectDir();
        }
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.Player.KnockBack(transform.position);
            GameManager.instance.Player.DamagePlayer(at);
            //ReflectDir();
        }
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("hit");
            ReflectDir();
        }
    }
    
}
