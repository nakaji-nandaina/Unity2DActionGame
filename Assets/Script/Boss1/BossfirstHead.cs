using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossfirstHead : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject boss;
    private BossEnemyfirst bossfirst;
    /*
    private void Start()
    {
        bossfirst = boss.GetComponent<BossEnemyfirst>();
    }
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