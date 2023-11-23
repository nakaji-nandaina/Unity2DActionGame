using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidEffect : MonoBehaviour
{
    private float timer = 0.2f;
    private float counter;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = GameManager.instance.Player.transform.position;
        sr = this.gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = GameManager.instance.Player.GetComponent<SpriteRenderer>().sprite;
        sr.sortingOrder = 3;
        sr.color = new Color(255, 255, 255, 0.8f);
        counter = timer;
    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0) Destroy(this.gameObject);
        sr.color = new Color(255, 255, 255, (counter / timer)*0.8f);
        Debug.LogWarning(sr.color);
    }
}
