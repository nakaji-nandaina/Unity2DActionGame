using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SkyFallWeapon : MonoBehaviour
{
    bool damaged;
    bool falled;
    bool inFall;
    float FallPointY;
    Rigidbody2D rb;

    [SerializeField,Tooltip("—Ž‰ºŽžŠÔ")]
    float FallTime;
    float FallCount;
    [SerializeField,Tooltip("—Ž‰º‰¹")]
    AudioClip FallClip;

    public int at;
    [SerializeField]
    GameObject Cautionobj;
    GameObject cobj;
    [SerializeField]
    Sprite[] fragments;
    // Start is called before the first frame update
    void Start()
    {
        inFall = false;
        falled = false;
        damaged = false;
        FallCount = FallTime;
        FallPointY = this.gameObject.transform.position.y;
        if(this.gameObject.GetComponent<Rigidbody2D>())this.gameObject.AddComponent<Rigidbody2D>();
        rb=this.gameObject.GetComponent<Rigidbody2D>();
        rb.mass = 0;
        cobj = Instantiate(Cautionobj, this.gameObject.transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
        if (GetComponent<Light2D>()) GetComponent<Light2D>().intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal) return;
        if (inFall)
        {
            rb.velocity = Vector2.down*30f;
            if (this.transform.position.y <= FallPointY)falled = true;
            return;
        }
        FallCount -= Time.deltaTime;
        if (FallCount <= 0)
        {
            inFall = true;

            if (GetComponent<Light2D>()) GetComponent<Light2D>().intensity = 1;
            GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 1);
            this.gameObject.transform.position=new Vector2(this.transform.position.x,FallPointY + 20f);
        }
    }
    private void LateUpdate()
    {
        if (!falled) return;
        if (!damaged)
        {
            damaged = true;
            return;
        }
        Destroy(cobj);
        GameManager.instance.PlayAudio(FallClip);
        spriteExplosion();
        Destroy(this.gameObject);
    }

    private void spriteExplosion()
    {
        for (int i = 0; i < fragments.Length; i++)
        {
            GameObject fragobj = new GameObject();
            if (GetComponent<Light2D>())
            {
                fragobj.AddComponent<Light2D>().color = GetComponent<Light2D>().color;
                fragobj.GetComponent<Light2D>().intensity = 0.3f;
                fragobj.GetComponent<Light2D>().pointLightOuterRadius = 0.5f;
            }
            fragobj.transform.position = this.transform.position;
            fragobj.AddComponent<SpriteRenderer>().sprite = fragments[i];
            fragobj.GetComponent<SpriteRenderer>().sortingOrder = 4;
            fragobj.AddComponent<Rigidbody2D>().mass = 1;
            fragobj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(5f, 8f)), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!falled) return;
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(at, this.transform.position, 0,null);
        }
        if (collision.tag == "BossFirst")
        {
            if (collision.gameObject.GetComponent<BossfirstBody>())
            {
                collision.gameObject.GetComponent<BossfirstBody>().takeDamage(at, FallClip);
            }
            if (collision.gameObject.GetComponent<BossfirstHead>())
            {
                collision.gameObject.GetComponent<BossfirstHead>().takeDamage(at, FallClip);
            }
        }
    }
}
