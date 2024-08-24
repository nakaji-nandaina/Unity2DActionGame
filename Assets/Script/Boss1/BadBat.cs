using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadBat : MonoBehaviour
{
    [SerializeField]
    public int Maxhp { get; private set; } = 1000;
    private int hp;
    [SerializeField]
    public int At { get; private set; } = 10;
    public Animator anim;
    private Rigidbody2D rb;
    private Transform playerPos;
    [SerializeField]
    private float encountRange = 10f;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    Slider hpbar;
    [SerializeField, Tooltip("à⁄ìÆêßå¿ópï«É^ÉCÉã")]
    GameObject limWall;
    [SerializeField]
    AudioClip BossBGM;


    public enum BossState
    {
        Default,
        Encount,
        Battle,
        Stop,
        Dead,
    }
    
    public BossState currentState = BossState.Default;

    // Start is called before the first frame update
    void Start()
    {
        hp = Maxhp;
        anim = this.gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        hpbar.maxValue = Maxhp;
        hpbar.value = hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
