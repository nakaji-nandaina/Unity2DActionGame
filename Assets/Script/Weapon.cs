using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public int attackDamage;
    [SerializeField]
    private float shotSpeed,destroyTime;
    private Rigidbody2D rb;
    private Vector2 weaponDirection;
    private float yukkuri;

    protected AudioSource weaponAudio;
    [SerializeField]
    AudioClip[] clips;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        yukkuri = destroyTime*1f;
        GameManager.instance.PlayAudio(clips[0]);
    }

    // Update is called once per frame
    void Update()
    {
        float slow = Mathf.Pow(destroyTime/yukkuri , 1.5f);
        rb.velocity = shotSpeed*slow * weaponDirection;
        DestroyCounter();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, this.transform.position);
            Destroy(this.gameObject);
            GameManager.instance.PlayAudio(clips[1]);
        }
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Break")
        {
            collision.gameObject.GetComponent<BreakObj>().BreakThis();
            Destroy(this.gameObject);
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
    public void moveDirection(Vector2 direction)
    {
        weaponDirection = direction;
    }
   
}
