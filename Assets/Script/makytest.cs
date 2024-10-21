using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makytest : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform playerPos;
    [SerializeField]
    private float moveSpeed, waitTime, walkTime;
    private float moveCounter, waitCounter;
    private Vector2 moveDir;

    [SerializeField, Tooltip("追いかけ速度")]
    private float chaseSpeed;
    [SerializeField]
    private float rangeToChase, rangeToWalk, chaseWaitTime, chaseTime;
    private float chaseCounter, chaseWaitCounter;

    private bool isChaseing;
    // Start is called before the first frame update
    void Start()
    {
        chaseWaitCounter = chaseWaitTime;
        waitCounter = waitTime;
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        ChaseOrNot();
        if (isChaseing) { 
            Chase(); 
        }
        else
        {
            notChase();
        }
    }
    private void ChaseOrNot()
    {
        if (Vector3.Distance(transform.position, playerPos.position) < rangeToChase)
        {
            isChaseing = true;
        }
        if (Vector3.Distance(transform.position, playerPos.position) >= rangeToWalk)
        {
            isChaseing = false;
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
                //moveDir = playerPos.position-this.transform.position;
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
        if (chaseWaitCounter >= 0)
        {
            chaseWaitCounter -= Time.deltaTime;
            rb.velocity = Vector2.zero;
            if (chaseWaitCounter <= 0)
            {
                chaseCounter = chaseTime;
                moveDir = playerPos.position - this.transform.position;
                moveDir.Normalize();
            }
        }
        else
        {
            chaseCounter -= Time.deltaTime;
            rb.velocity = moveDir * chaseSpeed;
            if (chaseCounter <= 0)
            {
                chaseWaitCounter = chaseWaitTime;
            }
        }
    }

}