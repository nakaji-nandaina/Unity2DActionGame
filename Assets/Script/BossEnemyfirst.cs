using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyfirst : MonoBehaviour
{
    [SerializeField]
    private int hp;
    [SerializeField]
    private int at;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform playerPos;
    [SerializeField]
    private float encountRange = 10f;

    private enum BossState 
    { 
        Default,
        Encount,
        Battle,
        Muteki,
        Dead,
    }
    BossState currentState = BossState.Default;


    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) 
        {
            case BossState.Default:
                if(encountRange>Mathf.Sqrt(Mathf.Pow(Mathf.Abs(this.gameObject.transform.position.x-playerPos.position.x),2)+ Mathf.Pow(Mathf.Abs(this.gameObject.transform.position.y - playerPos.position.y),2)))
                {
                    ChangeState(BossState.Encount);
                }
                return;
            case BossState.Encount:
                return;
            case BossState.Battle:
                Battle();
                return;
            case BossState.Muteki:
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
        }

    }
    private void EncountEffect()
    {

    }
    private void DeadEffect()
    {

    }

    private void Battle()
    {

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.KnockBack(transform.position);
            player.DamagePlayer(at);
        }
    }
}
