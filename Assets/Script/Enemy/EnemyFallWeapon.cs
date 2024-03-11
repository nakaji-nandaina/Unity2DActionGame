using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallWeapon : MonoBehaviour
{
    bool isPlayerStay=false;
    [SerializeField]
    float fallTime;
    float fallCount;
    Animator anim;
    int At=10;
    [SerializeField]
    AudioClip FallClip;
    float ypos;
    Rigidbody2D rb;
    bool falled;
    bool isbreak;
    [SerializeField]
    GameObject Cautionobj;
    GameObject cobj;
    [SerializeField]
    Sprite[] fragments;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerStay = false;
        fallCount = fallTime;
        anim = GetComponent<Animator>();
        ypos = this.transform.position.y;
        falled = false;
        isbreak = false;
        rb = GetComponent<Rigidbody2D>();
        cobj=Instantiate(Cautionobj, this.gameObject.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal) return;
        fallCount -= Time.deltaTime;
        if (isbreak)return;
        if (falled)
        {
            rb.velocity = Vector2.down*10;
            if (this.transform.position.y <= ypos)
            {
                if(isPlayerStay) GameManager.instance.Player.DamagePlayer(At);
                isbreak=true;
                rb.velocity = Vector2.zero;
                GameManager.instance.PlayAudio(FallClip);
                anim.SetBool("Break", true);
            }
            return;
        }
        if (fallCount <= 0)
        {
            this.transform.position = new Vector2(this.transform.position.x, ypos+10);
            anim.SetBool("Fall", true);
            falled = true;
           
        }
    }

    
    public void SetFall(int at)
    {
        At = at;
    }

    public void SprashFrag()
    {
        for(int i = 0; i < fragments.Length; i++)
        {
            GameObject fragobj = new GameObject();
            fragobj.transform.position = this.transform.position;
            fragobj.AddComponent<SpriteRenderer>().sprite = fragments[i];
            fragobj.GetComponent<SpriteRenderer>().sortingOrder = 4;
            fragobj.AddComponent<Rigidbody2D>().mass = 1;
            fragobj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2f,2f), Random.Range(5f,8f)), ForceMode2D.Impulse);
            DestCoroutine(fragobj);
        }
    }

    private void Dest()
    {
        Destroy(cobj);
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerStay = false;
        }
    }
    IEnumerator DestCoroutine(GameObject obj)
    {

        yield return new WaitForSeconds(0.2f);
        obj.GetComponent<Rigidbody2D>().mass = 0;
        obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Destroy(obj);
    }

}
