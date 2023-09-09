using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WD;
    public int attackDamage;
    private float shotSpeed,destroyTime;
    private Rigidbody2D rb;
    public Vector2 weaponDirection;
    private float yukkuri;
    public float kbforce;
    protected AudioSource weaponAudio;
    AudioClip[] clips;


    // Start is called before the first frame update
    void Start()
    {
        //attackDamage = WD.At;
        shotSpeed = WD.Speed;
        destroyTime = WD.DestTime;
        clips = new AudioClip[2];
        clips[0] = WD.ShotSound;
        clips[1] = WD.HitSound;
        //kbforce = WD.KbForce;

        rb = GetComponent<Rigidbody2D>();
        yukkuri = destroyTime*1f;
        GameManager.instance.PlayAudio(clips[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (WD.delay)
        {
            float slow = Mathf.Pow(destroyTime / yukkuri, 1.5f);
            rb.velocity = shotSpeed * slow * weaponDirection;
            DestroyCounter();
            return;
        }
        rb.velocity = shotSpeed * weaponDirection;
        DestroyCounter();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, this.transform.position, kbforce);
                if (WD.penetrate) return;
                Destroy(this.gameObject);
                GameManager.instance.PlayAudio(clips[1]);
                break;
            case "BossFirst":
                if (collision.gameObject.GetComponent<BossfirstBody>())
                {
                    collision.gameObject.GetComponent<BossfirstBody>().takeDamage(attackDamage);
                }
                if (collision.gameObject.GetComponent<BossfirstHead>())
                {
                    collision.gameObject.GetComponent<BossfirstHead>().takeDamage(attackDamage);
                }
                if (WD.penetrate) return;
                Destroy(this.gameObject);
                break;

            case "Wall":
                Destroy(this.gameObject);
                break;

            case "Break":
                collision.gameObject.GetComponent<BreakObj>().BreakThis();
                if (WD.penetrate) return;
                Destroy(this.gameObject);
                break;
        }

    }
    private void DestroyCounter()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
        {
            
            Destroy(this.gameObject);
        }
    }
    
   
}
