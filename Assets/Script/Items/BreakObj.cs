using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObj : MonoBehaviour
{
    List<GameObject> myParts = new List<GameObject>();
    private bool IsBreak;
    private float BreakTime;
    private float miniTime;
    protected AudioSource BreakSE;
    [SerializeField]
    AudioClip[] clips;
    private int i;
    [SerializeField]
    private BoxCollider2D area;
    [SerializeField]
    private bool isDrop=false;
    private DropItem dropitem;
    private GameObject shadow;
    [SerializeField]
    private List<Sprite> partImages;

    private int partnum;

    [SerializeField]
    private Item parts;
    // Start is called before the first frame update
    void Start()
    {
        if (isDrop)
        {
            dropitem = GetComponent<DropItem>();
        }
        
        BreakSE = GetComponent<AudioSource>();
        BreakTime = 0.1f;
        miniTime = 0.05f;
        IsBreak = false;
        i = 0;
        // 自分の子要素をチェック
        foreach (Transform child in gameObject.transform)
        {
            // パーツに Rigidbody2D を追加して Kinematic にしておく
            if(!child.gameObject.GetComponent<Rigidbody2D>())child.gameObject.AddComponent<Rigidbody2D>();
            child.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            child.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            child.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            //child.gameObject.transform.position = Vector2.zero;
            // 子要素リストにパーツを追加
            if (child.gameObject.tag == "NoBreak" || child.gameObject.name == "Area") continue;
            if (child.gameObject.tag == "Shadow")
            {
                shadow = child.gameObject;
                shadow.GetComponent<SpriteRenderer>().sortingOrder = 1;
                continue;
            }
        }
        partnum = Random.Range(12, 18);
        for(int i = 0; i < partnum; i++)
        {
            GameObject child = new GameObject();
            child.transform.SetParent(this.transform);
            child.transform.localPosition = Vector2.zero;
            child.gameObject.AddComponent<Rigidbody2D>();
            child.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            child.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            child.gameObject.AddComponent<SpriteRenderer>();
            child.gameObject.GetComponent<SpriteRenderer>().sprite = partImages[i % partImages.Count];
            child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            child.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            myParts.Add(child.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBreak)
        {
            BreakTime -= Time.deltaTime;
            miniTime -= Time.deltaTime;
            if (BreakTime <= 0)
            {
                IsBreak = false;
                foreach (GameObject obj in myParts)
                {
                    obj.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    obj.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
                    obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    if (this.gameObject.tag == "Enemy" )
                    {
                        obj.gameObject.AddComponent<AbsorbParts>();
                        obj.gameObject.GetComponent<AbsorbParts>().parts = parts;

                    }
                }
            }
            else if(miniTime <= 0&&i==0)
            {
                miniTime = 0.5f;
                i = 1;
                foreach (GameObject obj in myParts)
                {
                    // 飛ばすパワーと回転をランダムに設定
                    Vector2 forcePower = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(3.5f, 4.0f));
                    
                    // パーツをふっとばす！
                    obj.GetComponent<Rigidbody2D>().isKinematic = false;
                    obj.GetComponent<Rigidbody2D>().AddForce(forcePower, ForceMode2D.Impulse);
                    
                    obj.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
                }
            }
            else if(miniTime <= 0)
            {
                miniTime = 10;
                foreach (GameObject obj in myParts)
                {
                    obj.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.05f;

                }
            }
        
            foreach (GameObject obj in myParts)
            {
                obj.transform.position = new Vector3(Mathf.Clamp(obj.transform.position.x, area.bounds.min.x , area.bounds.max.x ),
                    Mathf.Clamp(obj.transform.position.y, area.bounds.min.y , area.bounds.max.y ), obj.transform.position.z);
            }
            
        }
        
    }

    public void BreakThis()
    {
        if (isDrop)
        {
            dropitem.Drop(this.gameObject.transform.position);
        }
        GameManager.instance.PlayAudio(clips[0]);
        this.GetComponent<SpriteRenderer>().sortingOrder = -2;
        this.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        Destroy(this.GetComponent<BoxCollider2D>());
        Destroy(this.GetComponent<EnemyController>());
        if (shadow != null) Destroy(shadow);
        IsBreak = true;
        BreakTime = 1.8f;
        miniTime = 0.7f;
        // 各パーツをふっとばす
        foreach (GameObject obj in myParts)
        {

            // 飛ばすパワーと回転をランダムに設定
            Vector2 forcePower = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(2.0f, 5.0f));
            obj.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            // パーツをふっとばす！
            obj.GetComponent<Rigidbody2D>().isKinematic = false;
            obj.GetComponent<Rigidbody2D>().AddForce(forcePower, ForceMode2D.Impulse);
            
            obj.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        }
    }
}

