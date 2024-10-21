using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryFullUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text Dtext;
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
        player = GameManager.instance.Player.gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        removeTime -= Time.deltaTime;
        float a = Mathf.Pow(removeTime / motherTime, 1.5f);
        float b = Mathf.Pow(0.5f, 1.5f);
        float c = Mathf.Pow(removeTime * 2 / motherTime, 1.5f);
        if (removeTime > 0)
        {
            Dtext.GetComponent<RectTransform>().anchoredPosition = 1 / (this.gameObject.GetComponent<RectTransform>().localScale.x) * (player.transform.position - this.transform.position + Vector3.up) + 250f * Vector3.up - 20f * Vector3.right + 500f * a * Vector3.down;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
