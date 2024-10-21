using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool IsDestory;
    private float DTime;

    [SerializeField]
    private AudioClip doorAudio;
    // Start is called before the first frame update
    void Start()
    {
        IsDestory = false;
        DTime = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDestory)
        {
            DTime -= Time.deltaTime;
            if (DTime <= 0)
            {
                GameManager.instance.PlayAudio(doorAudio);
                Destroy(this.gameObject);
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsDestory = true;
        //ここに.wavいれる
    }
}
