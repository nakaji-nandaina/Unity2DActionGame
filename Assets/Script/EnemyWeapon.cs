using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private float shotSpeed, destroyTime;
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
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = shotSpeed * weaponDirection;
        DestroyCounter();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DamagePlayer(attackDamage);
            Destroy(this.gameObject);
            GameManager.instance.PlayAudio(clips[0]);
        }
        if (collision.gameObject.tag == "Wall")
        {
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
