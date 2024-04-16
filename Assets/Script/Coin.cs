using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int moneyValue = 10;
    private Animator coinanim;
    [SerializeField] 
    AudioClip[] clips;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        coinanim = GetComponent<Animator>();
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
            if(this.gameObject.transform.Find("coinしんぼる"))Destroy(this.gameObject.transform.Find("coinしんぼる").gameObject);
            rb.AddForce(Vector2.up*0.1f);
            GameManager.instance.PlayAudio(clips[0]);
            GameManager.instance.UpdateMoneyUI(moneyValue+GameManager.currentMoney);
        }
    }
    private void Destcoin(){
        Destroy(this.gameObject);
    }

}
