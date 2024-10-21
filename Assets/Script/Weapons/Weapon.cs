using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    //惰性移動する武器のパラメータ
    private Vector2 daseiNextP;
    bool daseiKikan = false;
    private float daseiTime=1f;
    private float daseiCount = 0f;
    //一時停止で保持する速度
    Vector2 keepV;
    //爆発パラメータ
    bool isBakuhatu = false;


    // Start is called before the first frame update

    private float zanzotime = 0.1f;
    private float zanzocounter = 0;
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
        if (GameManager.instance.Player.ps != PlayerController.PS.normal)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isBakuhatu)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = keepV;
        if (WD.delay)
        {
            delay();
            return;
        }
        if (WD.cursor)
        {
            chaseCursor();
            return;
        }
        if (WD.dasei)
        {
            dasei();
            return;
        }
        if (WD.bakuhatu)
        {
            explosion();
            return;
        }
        rb.velocity = shotSpeed * weaponDirection;
        DestroyCounter();
    }

    private void delay()
    {
        float slow = Mathf.Pow(destroyTime / yukkuri, 1.5f);
        rb.velocity = shotSpeed * slow * weaponDirection;
        DestroyCounter();
    }

    private void chaseCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weaponDirection = (mousePos - this.transform.position).normalized;
        rb.velocity = shotSpeed * weaponDirection.normalized;
        if (Mathf.Abs(mousePos.x - this.transform.position.x) <= 0.1f && Mathf.Abs(mousePos.y - this.transform.position.y) <= 0.1f) rb.velocity = Vector2.zero;
        DestroyCounter();
        zanzocounter -= Time.deltaTime;
        if (zanzocounter > 0) return;
        zanzocounter = zanzotime;
        GameObject zanzo = new GameObject("zanzo");
        zanzo.AddComponent<AvoidEffect>();
        zanzo.transform.position = this.transform.position;
        zanzo.transform.rotation = this.transform.rotation;
        Sprite sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        zanzo.GetComponent<AvoidEffect>().SetSprite(sprite);
    }

    private void explosion()
    {
        destroyTime -= Time.deltaTime;
        rb.velocity = shotSpeed * weaponDirection;
        if (destroyTime > 0) return;
        bakuhatu();
    }

    private void dasei()
    {
        float Maxdis = 5f;
        float lowSpeed = WD.Speed * 0.2f;
        float dis = Vector3.Distance(this.transform.position, GameManager.instance.Player.transform.position);
        if (destroyTime == WD.DestTime)
        {
            daseiCount = 0;
            daseiNextP = new Vector2(GameManager.instance.Player.transform.position.x + weaponDirection.x * Maxdis, GameManager.instance.Player.transform.position.y + weaponDirection.y * Maxdis);
            rb.velocity = shotSpeed * weaponDirection;
            daseiKikan = false;
            //Debug.LogError(daseiNextP.ToString());
        }
        else if (dis < 1f && daseiKikan)
        {
            //Debug.LogError("ぷれいや");
            daseiCount = 0;
            shotSpeed = WD.Speed;
            rb.velocity = shotSpeed * weaponDirection;
            daseiKikan = false;
            daseiNextP = new Vector2(GameManager.instance.Player.transform.position.x + weaponDirection.x * Maxdis, GameManager.instance.Player.transform.position.y + weaponDirection.y * Maxdis);
        }
        else if (daseiKikan)
        {
            daseiCount += Time.deltaTime;
            weaponDirection = (GameManager.instance.Player.transform.position - this.transform.position).normalized;
            shotSpeed = WD.Speed * daseiCount / 2;
            shotSpeed = shotSpeed > lowSpeed ? shotSpeed : lowSpeed;
            rb.velocity = shotSpeed * weaponDirection;
            //Debug.LogError("帰還");
        }
        else
        {
            daseiCount += Time.deltaTime;
            shotSpeed = WD.Speed * (daseiTime - daseiCount) / daseiTime;
            shotSpeed = shotSpeed > lowSpeed ? shotSpeed : lowSpeed;
            rb.velocity = shotSpeed * weaponDirection;
            if (daseiCount >= daseiTime)
            {
                daseiCount = 0;
                daseiKikan = true;
            }
            //Debug.LogError("ぶっとび "+dis.ToString()+" "+ndis.ToString());
        }
        DestroyCounter();
    }

    private void LateUpdate()
    {
        if (rb.velocity.x == 0 && rb.velocity.y == 0) return;
        keepV = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool notDest = WD.penetrate || WD.cursor||WD.dasei;
        bool notwallDest = WD.cursor||WD.dasei;
        
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, this.transform.position, kbforce,clips[1]);
                if (notDest)return;
                
                bakuhatu();
                if (WD.bakuhatu) return;
                Destroy(this.gameObject);
                GameManager.instance.PlayAudio(clips[1]);
                break;
            case "BossFirst":
                if (collision.gameObject.GetComponent<BossfirstBody>())
                {
                    collision.gameObject.GetComponent<BossfirstBody>().takeDamage(attackDamage,clips[1]);
                }
                if (collision.gameObject.GetComponent<BossfirstHead>())
                {
                    collision.gameObject.GetComponent<BossfirstHead>().takeDamage(attackDamage,clips[1]);
                }
                if (notDest)return;
                bakuhatu();
                if (WD.bakuhatu) return;
                Destroy(this.gameObject);
                break;

            case "Wall":
                if (notwallDest) return;
                bakuhatu();
                if (WD.bakuhatu) return;
                Destroy(this.gameObject);
                break;

            case "Break":
                collision.gameObject.GetComponent<BreakObj>().BreakThis();
                if (notDest) return;                
                //bakuhatu();
                if (WD.bakuhatu)
                {
                    bakuhatu();
                    return;
                }
                Destroy(this.gameObject);
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) 
        {
            case "Enemy":
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, this.transform.position, kbforce, clips[1]);
                break;
            case "BossFirst":
                if (collision.gameObject.GetComponent<BossfirstBody>())
                {
                    collision.gameObject.GetComponent<BossfirstBody>().takeDamage(attackDamage, clips[1]);
                }
                if (collision.gameObject.GetComponent<BossfirstHead>())
                {
                    collision.gameObject.GetComponent<BossfirstHead>().takeDamage(attackDamage, clips[1]);
                }
                break;
        }

    }

    private void DestroyCounter()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime > 0) return;
        Destroy(this.gameObject);
        
    }
    
    private void bakuhatu()
    {
        if (!this.gameObject.GetComponent<Animator>()) return;
        if (!WD.bakuhatu) return;
        if (isBakuhatu) return;
        GameManager.instance.PlayAudio(clips[1]);
        this.gameObject.GetComponent<Animator>().SetTrigger("Break");
        if(this.gameObject.GetComponent<Light2D>()) this.gameObject.GetComponent<Light2D>().intensity = 1-GameManager.instance.GetComponent<Light2D>().intensity;
        rb.velocity = Vector2.zero;
        isBakuhatu = true;
    }

    public void destWeapon()
    {
        Destroy(this.gameObject);
    }
   
}
