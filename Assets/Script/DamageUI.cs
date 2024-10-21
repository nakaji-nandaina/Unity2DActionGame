using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DamageUI : MonoBehaviour
{
    [SerializeField]
    public Text Dtext;
    private float removeTime;
    private float motherTime;
    private Rigidbody2D rb;
    private Vector3 enemyPos;
    private GameObject enemy;
    private Transform playerPos;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = Dtext.GetComponent<Rigidbody2D>();
        removeTime = 0.5f;
        motherTime = removeTime;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        removeTime -= Time.deltaTime;
        float a = Mathf.Pow(removeTime / motherTime, 1.5f);
        float b = Mathf.Pow(0.5f, 1.5f);
        float c = Mathf.Pow(removeTime * 2 / motherTime, 1.5f);
        if (removeTime > motherTime / 2)
        {
            Dtext.GetComponent<RectTransform>().anchoredPosition = 1 / (this.gameObject.GetComponent<RectTransform>().localScale.x) * (enemy.transform.position - this.transform.position + Vector3.up) + 250f * Vector3.up - 20f*Vector3.right + 500f * a * Vector3.down;
        }
        else if (removeTime <= motherTime / 2)
        {
            Dtext.GetComponent<RectTransform>().anchoredPosition = 1 / (this.gameObject.GetComponent<RectTransform>().localScale.x) * (enemy.transform.position - this.transform.position + Vector3.up) - 20f * Vector3.right + 500f * b * Vector3.down+250f*Vector3.up;
        }
        if (removeTime <= 0)
        {
            Destroy(this.gameObject);
        }

    }
    public void DamageSet(float damageNum,Vector3 DamagePos,GameObject enemyObj)
    {
        Dtext.text = Convert.ToString(damageNum);
        //Dtext.transform.position += DamagePos;
        enemy = enemyObj;
        enemyPos = DamagePos;
        //Dtext.transform.position = new Vector2((0),(0));
    }
}
