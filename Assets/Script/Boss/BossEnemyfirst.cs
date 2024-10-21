using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemyfirst : MonoBehaviour
{
    //ã§í 
    [SerializeField]
    public int Maxhp { get; private set; } = 1000;
    private int hp;
    [SerializeField]
    public int At { get; private set; } = 10;
    public Animator anim;
    public Sprite[] Breaks;
    private Rigidbody2D rb;
    private Transform playerPos;
    [SerializeField]
    private float encountRange = 10f;
    [SerializeField]
    Slider hpbar;
    [SerializeField, Tooltip("à⁄ìÆêßå¿ópï«É^ÉCÉã")]
    GameObject limWall;
    [SerializeField]
    AudioClip BossBGM;
    public Item breakItem;
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
    [field:SerializeField]
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

    [SerializeField]
    private GameObject lastPoint;

    public GameObject BodyShotWeapon;
    [SerializeField]
    private Sprite angry;

    [HideInInspector]
    public int childnum;

    public List<GameObject> bloods;

    public enum BattleState
    {
        Syukai,
        Hansya,
        toLast,
        Last,
    }
    public BattleState battleState = BattleState.Syukai;

    void Start()
    {
        hp = Maxhp;
        anim = this.gameObject.GetComponent<Animator>();
        rb = headobj.GetComponent<Rigidbody2D>();
        headPos = headobj.GetComponent<Transform>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        moveSpeed = firstSpeed;
        childnum = this.gameObject.transform.childCount;
        hpbar.maxValue = Maxhp;
        hpbar.value = hp;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal) return;
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
        switch (nextstate) 
        {
            case BossState.Dead:
                DeadEffect();
                currentState = nextstate;
                return;
            case BossState.Encount:
                EncountEffect();
                currentState = nextstate;
                return;
            case BossState.Battle:
                FirstDir();
                currentState = nextstate;
                return;
        }

    }

    private void ChangeBattleState(BattleState nextstate)
    {
        switch (nextstate) {
            case BattleState.Hansya:
                break;
            case BattleState.toLast:
                break;
            case BattleState.Last:
                //headobj.GetComponent<Animator>().applyRootMotion = true;
                headobj.GetComponent<SpriteRenderer>().sprite=angry;
                break;
        }
        battleState = nextstate;

    }

    private void EncountEffect()
    {
        limWall.SetActive(true);
        hpbar.gameObject.SetActive(true);
        GameManager.instance.ChangeBGM(BossBGM,1);
    }
    private void DeadEffect()
    {
        limWall.SetActive(false);
        hpbar.gameObject.SetActive(false);
        GameManager.instance.TurnBGM();
        Destroy(this.gameObject);
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
    public void lastDir()
    {
        if (1f > Vector3.Distance(headPos.position, lastPoint.transform.position))
        {
            dirx = 0;
            diry = 0;
            ChangeBattleState(BattleState.Last);
            return;
        }
        dirx = lastPoint.transform.position.x - headPos.position.x;
        diry = lastPoint.transform.position.y - headPos.position.y;
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
                if (childnum == 1) ChangeBattleState(BattleState.toLast);
                return;
            case BattleState.toLast:
                lastDir();
                rb.velocity = new Vector2(dirx, diry).normalized * moveSpeed;
                return;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState != BossState.Battle) return;
        hp -= damage;
        Debug.Log(hp);
        if (hp <= Maxhp / 2 && battleState == BattleState.Syukai)
        {
            Debug.Log("é¸âÒ");
            hp = Maxhp / 2;
            ChangeBattleState(BattleState.Hansya);
        }
        if (hp <= 0)
        {
            hp = 0;
            ChangeState(BossState.Dead);
            return;
        }
        hpbar.value = hp;
        
    }

    public void playerhit(Vector2 bodypos)
    {
        GameManager.instance.Player.KnockBack(bodypos);
        GameManager.instance.Player.DamagePlayer(At);
    }    
}
