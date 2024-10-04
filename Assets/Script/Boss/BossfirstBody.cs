using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossfirstBody : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject boss;
    private BossEnemyfirst bossfirst;
    private EnemyShotManager enemyshot;
    private float currentT;  //0f
    private float nextT;     //rand
    Rigidbody2D rb;
    private float speed;     //5f
    private bool isJoint;
    Vector2 dir;
    private int maxhp;
    private int hp;
    Animator anim;
    float damagedTime = 0.2f;
    float damagedCount = 0f;
    [SerializeField]
    Sprite[] sprites;
    SpriteRenderer sr;
    bool isDead = false;
    bool isBreak = false;
    List<GameObject> parts;
    private float BreakTime;
    private float miniTime;
    bool breaked;
    [SerializeField]
    private BoxCollider2D area;
    private void Start()
    {
        breaked = false;
        parts = new List<GameObject>();
        BreakTime = 1.2f;
        miniTime = 0.7f;
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        bossfirst = boss.GetComponent<BossEnemyfirst>();
        enemyshot = this.gameObject.GetComponent<EnemyShotManager>();
        if (enemyshot == null)
        {
            this.gameObject.AddComponent<EnemyShotManager>();
            enemyshot = this.gameObject.GetComponent<EnemyShotManager>();
        }

        currentT = 0f;
        nextT = Random.Range(3f, 5f);
        dir.x = Random.Range(-1f, 1f);
        dir.y = Random.Range(-1f, 1f);
        speed = 5f;
        isJoint = true;
        sr = GetComponent<SpriteRenderer>();
        maxhp = 200;
        hp = maxhp;
    }
    private void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isDead)
        {   
            rb.velocity = Vector2.zero;
            if (breaked) return;
            BreakTime -= Time.deltaTime;
            miniTime -= Time.deltaTime;
            if (BreakTime <= 0)
            {
                foreach (GameObject obj in parts)
                {
                    breaked = true;
                    obj.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    obj.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
                    obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    obj.gameObject.AddComponent<AbsorbParts>();
                    obj.gameObject.GetComponent<AbsorbParts>().parts = bossfirst.breakItem;
                }
            }
            else if (miniTime <= 0 && !isBreak)
            {
                miniTime = 0.5f;
                isBreak = true;
                foreach (GameObject obj in parts)
                {
                    // 飛ばすパワーと回転をランダムに設定
                    Vector2 forcePower = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(3.5f, 4.0f));

                    // パーツをふっとばす！
                    obj.GetComponent<Rigidbody2D>().isKinematic = false;
                    obj.GetComponent<Rigidbody2D>().AddForce(forcePower, ForceMode2D.Impulse);

                    obj.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
                }
            }
            else if (miniTime <= 0)
            {
                miniTime = 10;
                foreach (GameObject obj in parts)
                {
                    obj.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.05f;

                }
            }

            foreach (GameObject obj in parts)
            {
                obj.transform.position = new Vector3(Mathf.Clamp(obj.transform.position.x, area.bounds.min.x, area.bounds.max.x),
                    Mathf.Clamp(obj.transform.position.y, area.bounds.min.y, area.bounds.max.y), obj.transform.position.z);
            }
            return;
        }
        damagedCount = 0 > damagedCount - Time.deltaTime ? 0 : damagedCount - Time.deltaTime;
        spriteDir();
        switch (bossfirst.currentState)
        {
            case BossEnemyfirst.BossState.Battle:
                battle();
                break;
        }
    }

    private void battle()
    {
        currentT += Time.deltaTime;
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Syukai:
                if (currentT < nextT) return;
                nextT = Random.Range(3f, 5f);
                currentT = 0f;
                Shot();
                break;
            case BossEnemyfirst.BattleState.Hansya:
                if (isJoint)
                {

                    Destroy(GetComponent<DistanceJoint2D>());
                    isJoint = false;
                    //return;
                    Debug.Log("jointOut");
                }
                rb.velocity = new Vector2(dir.x, dir.y).normalized * speed;
                //射出
                if (currentT < nextT) return;
                nextT = Random.Range(2f, 4f);
                currentT = 0f;
                Shot();
                break;
        }
    }
    private void spriteDir()
    {
        Vector3 Ppos = GameManager.instance.Player.transform.position;
        if (Ppos.y > this.transform.position.y)
        {
            if (Ppos.x < this.transform.position.x) sr.sprite = sprites[0];
            else sr.sprite = sprites[1];
            return;
        }
        if (Ppos.x < this.transform.position.x) sr.sprite = sprites[2];
        else sr.sprite = sprites[3];
    }
    private void Bound()
    {
        dir.x = Random.Range(-1f, 1f);
        dir.y = Random.Range(-1f, 1f);
    }

    private void Shot()
    {
        //Debug.Log(bossfirst.BodyShotWeapon);
        Vector3 Ppos = GameManager.instance.Player.transform.position;
        Vector3 Tpos = this.gameObject.transform.position;
        Vector2 attackDir = Ppos - Tpos;
        enemyshot.EmemyShot(Ppos, Tpos, attackDir, bossfirst.BodyShotWeapon, bossfirst.At);
    }

    private void hitOthers()
    {
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Hansya:
                Bound();
                break;
        }
    }

    public void takeDamage(int at, AudioClip clip)
    {
        if (bossfirst.currentState != BossEnemyfirst.BossState.Battle) return;
        if (damagedCount > 0) return;
        damagedCount = damagedTime;
        GameManager.instance.PlayAudio(clip);
        switch (bossfirst.battleState)
        {
            case BossEnemyfirst.BattleState.Syukai:
                bossfirst.TakeDamage(at);
                anim.SetTrigger("Damaged");
                break;
            case BossEnemyfirst.BattleState.Hansya:
                hp -= at;
                anim.SetTrigger("Damaged");
                Debug.LogError(hp);
                if (hp <= 0 && !isDead)
                {
                    BreakBoss();
                    //Destroy(this.gameObject);
                }
                break;
        }
    }
    void DeleteChildByName(string childName)
    {
        // 子オブジェクトを全て取得
        foreach (Transform child in transform)
        {
            // 名前が一致するかチェック
            if (child.gameObject.name == childName)
            {
                // 子オブジェクトを削除
                Destroy(child.gameObject);
                break; // 一つ見つけたらループを抜ける
            }
        }
    }
    private void BreakBoss()
    {
        Destroy(GetComponent<CapsuleCollider2D>());
        bossfirst.childnum--;
        this.GetComponent<SpriteRenderer>().sortingOrder = -10;
        this.GetComponent<SpriteRenderer>().sortingLayerName="Default";
        DeleteChildByName("shadow");
        DeleteChildByName("てきしんぼる");
        isDead = true;
        int rand = Random.Range(0, 6);
        GameObject blood = Instantiate(bossfirst.bloods[rand]);
        blood.transform.localPosition = this.transform.position;
        //Debug.LogError(isDead);
        for (int i = 0; i < bossfirst.Breaks.Length; i++)
        {
            GameObject newpart = new GameObject(i.ToString());
            newpart.transform.SetParent(this.transform);

            Vector2 pos = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            // 親の位置にオブジェクトを移動
            newpart.transform.localPosition = pos;
            newpart.AddComponent<SpriteRenderer>().sprite = bossfirst.Breaks[i];

            newpart.GetComponent<SpriteRenderer>().sortingOrder = 2;
            newpart.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            newpart.AddComponent<Rigidbody2D>();
            Vector2 forcePower = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(2.0f, 5.0f));
            newpart.GetComponent<Rigidbody2D>().isKinematic = false;
            newpart.GetComponent<Rigidbody2D>().AddForce(forcePower, ForceMode2D.Impulse);
            newpart.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            parts.Add(newpart);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                bossfirst.playerhit(this.transform.position);
                hitOthers();
                break;

            case "Wall":
                hitOthers();
                break;

            default:
                hitOthers();
                break;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                bossfirst.playerhit(this.transform.position);
                hitOthers();
                break;

            case "Wall":
                hitOthers();
                break;

            default:
                hitOthers();
                break;
        }
    }

}
