using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KaidanMoveScene : MonoBehaviour
{
    public bool IsCount;
    public bool IsUse;
    public float Count;
    public float STime=2.0f;
    [SerializeField]
    private string[] Scene;
    
    private int SceneNum;
    [SerializeField]
    private Vector2[] NextPosition;
    float turnonT;
    bool isNext = false;
    // Start is called before the first frame update
    void Start()
    {
        turnonT = 1.0f;
        IsCount = false;
        IsUse = true;
        Count = STime;
        SceneNum = Random.Range(0, Scene.Length);
        GameManager.instance.UpdateSceneUI(IsCount,STime,Count);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (turnonT > 0)
        {
            turnonT -= Time.deltaTime;
        }
        if (IsCount&&turnonT<=0)
        {
            Count -= Time.deltaTime;
            GameManager.instance.UpdateSceneUI(IsCount, STime, Count);
            
            if (Count <= 0)
            {
                //GameManager.instance.setFadeout();
                //SceneManager.LoadScene(Scene[SceneNum]);
                FadeManager.Instance.LoadScene(Scene[SceneNum],1f);
                GameManager.StartSpone = new Vector2(NextPosition[SceneNum].x, NextPosition[SceneNum].y);
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                GameManager.playerX = playerObj.GetComponent<Animator>().GetFloat("X");
                GameManager.playerY = playerObj.GetComponent<Animator>().GetFloat("Y");
                

            }
        }
        else if(IsUse && !IsCount)
        {
            Count = STime;
            IsUse = false;
            GameManager.instance.UpdateSceneUI(IsCount, STime, Count);
        }
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsCount = true;
            IsUse = true;
            GameManager.instance.PlayerStateHold();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            IsCount = false;
        }
    }
}
