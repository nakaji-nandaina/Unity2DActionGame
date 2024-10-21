using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [HideInInspector]
    public WeaponData WD;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int at = GameManager.instance.Player.at+WD.At;
        switch (collision.tag)
        {
            case "Enemy":
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(at, this.transform.position, WD.KbForce,WD.HitSound);
                
                break;
            case "BossFirst":
                if (collision.gameObject.GetComponent<BossfirstBody>())
                {
                    collision.gameObject.GetComponent<BossfirstBody>().takeDamage(at, WD.HitSound);
                }
                if (collision.gameObject.GetComponent<BossfirstHead>())
                {
                    collision.gameObject.GetComponent<BossfirstHead>().takeDamage(at, WD.HitSound);
                }
                break;
            case "Break":
                collision.gameObject.GetComponent<BreakObj>().BreakThis();
                break;
        }
    }
    

}
