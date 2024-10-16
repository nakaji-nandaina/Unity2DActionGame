﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AbsorbParts : MonoBehaviour
{
    private float abSpeed = 10.0f;
    private Transform playerPos;
    private Vector2 moveDir=Vector2.zero;
    private Rigidbody2D rb;
    private BoxCollider2D box;
    public Item parts; 

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<BoxCollider2D>();
        box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, playerPos.position) < 5.0)
        {
            moveDir = playerPos.position - this.transform.position;
            moveDir.Normalize();
            rb.velocity = moveDir * abSpeed;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (parts){
                collision.gameObject.GetComponent<PlayerController>().inventory.AddItem(parts, 1);
                GameManager.instance.pulldownNotice.AddItem(parts);
                //EditorUtility.SetDirty(collision.gameObject.GetComponent<PlayerController>().inventory);
            }
            Destroy(this.gameObject);
        }
    }
}
