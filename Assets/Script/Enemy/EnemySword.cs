using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [HideInInspector]
    public int at;
    [HideInInspector]
    public int kb;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        collision.gameObject.GetComponent<PlayerController>().KnockBack(this.transform.position);
        collision.gameObject.GetComponent<PlayerController>().DamagePlayer(at);
    }
}
