using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int moneyValue = 10;
    private Animator coinanim;
    protected AudioSource coinSE;
    [SerializeField] 
    AudioClip[] clips;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        coinanim = GetComponent<Animator>();
        coinSE = GetComponents<AudioSource>()[0];
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            coinanim.SetTrigger("Got");
            
            rb.AddForce(Vector2.up*1000);
            coinSE.PlayOneShot(clips[0]);
            GameManager.instance.UpdateMoneyUI(moneyValue);
        }
    }
    private void Destcoin(){
        Destroy(this.gameObject);
    }

}
