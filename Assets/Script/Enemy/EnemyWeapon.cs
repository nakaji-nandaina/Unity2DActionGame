using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private float shotSpeed, destroyTime;
    [SerializeField]
    bool Bakuhatu;
    private Rigidbody2D rb;
    private Vector2 weaponDirection;
    private float yukkuri;
    private Animator anim;
    protected AudioSource weaponAudio;
    [SerializeField]
    AudioClip[] clips;

    bool isdest = false;
    bool hitplayer = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = shotSpeed * weaponDirection;
        DestroyCounter();
        if (anim == null) return;
        if (isdest) rb.velocity = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Bakuhatu) collision.gameObject.GetComponent<PlayerController>().KnockBack(transform.position);
            collision.gameObject.GetComponent<PlayerController>().DamagePlayer(attackDamage);
            hitplayer = true;
            destFunc();
        }
        if (collision.gameObject.tag == "Wall")
        {
            destFunc();
        }

    }
    private void DestroyCounter()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
        {

            destFunc();
        }
    }
    public void moveDirection(Vector2 direction,int enemyat)
    {
        weaponDirection = direction;
        attackDamage = enemyat;
    }

    private void destFunc()
    {
        if (isdest) return;
        if (!Bakuhatu)
        {
            if(hitplayer)GameManager.instance.PlayAudio(clips[0]);
            Destroy(this.gameObject); 
            return;
        }
        GameManager.instance.PlayAudio(clips[1]);
        anim.SetTrigger("Break");
        rb.velocity = Vector2.zero;
        
        isdest = true;
    }
    
    public void destWeapon()
    {
        Destroy(this.gameObject);
    }
}
