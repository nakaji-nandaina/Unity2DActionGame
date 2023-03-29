using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedItem : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider2D collider2D;
    [SerializeField]
    private Item item;
    private float destTime=0.6f;
    private float destCount;
    private bool got = false;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    [SerializeField]
    private AudioClip audioClip;
    private AudioSource audioSource;
    private void Start()
    {
        destCount = destTime;
        collider2D = this.gameObject.GetComponent<BoxCollider2D>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sp = this.gameObject.GetComponent<SpriteRenderer>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (got)
        {
            if (destCount == destTime)
            {
                rb.velocity = new Vector2(0, 2);
            }
            destCount -= Time.deltaTime;
            float alfa = destCount / destTime;
            sp.color = new Color(255, 255, 255, alfa);
            if (destCount <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().inventory.AddItem(item, 1);
            audioSource.PlayOneShot(audioClip);
            Destroy(collider2D);
            got = true;
        }
    }
}