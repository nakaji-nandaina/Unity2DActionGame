using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedWeapon : MonoBehaviour
{
    private BoxCollider2D Collider2D;
    [SerializeField]
    private WeaponData weapon;
    private float destTime = 0.6f;
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
        Collider2D = this.gameObject.GetComponent<BoxCollider2D>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sp = this.gameObject.GetComponent<SpriteRenderer>();
        sp.sprite = weapon.Icon;
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
            if (collision.gameObject.GetComponent<PlayerController>().weaponPouch.Pouch.Count >= collision.gameObject.GetComponent<PlayerController>().weaponPouch.max)
            {
                Instantiate(GameManager.instance.InventoryFullText, GameManager.instance.Player.transform.position, Quaternion.Euler(0, 0, 0));
                return;
            }
            collision.gameObject.GetComponent<PlayerController>().weaponPouch.AddWeapon(weapon);
            Destroy(this.gameObject.transform.Find("ぶきしんぼる").gameObject);
            Destroy(this.gameObject.transform.Find("shadow").gameObject);
            GameManager.instance.PlayAudio(audioClip);
            Destroy(Collider2D);
            got = true;
        }
    }
}
