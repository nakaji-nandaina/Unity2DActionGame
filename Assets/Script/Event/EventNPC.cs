using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventNPC : MonoBehaviour
{
    public List<Route> Routes;
    public float MoveSpeed=5f;
    public float AnimSpeed = 2f;
    public string FuncName;
    int nowPoint = 0;
    Rigidbody2D rb;

    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveRoute();
    }
    void MoveRoute()
    {
        if (Routes.Count <= nowPoint)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if(Mathf.Abs(Routes[nowPoint].x-this.transform.position.x)<0.05f&& Mathf.Abs(Routes[nowPoint].y - this.transform.position.y) < 0.05f)
        {
            nowPoint++;
            Debug.LogError(nowPoint.ToString());
            if (Routes.Count == nowPoint)
            {
                Invoke(FuncName, 0);
                return;
            }
        }
        rb.velocity = new Vector2(Routes[nowPoint].x - this.transform.position.x, Routes[nowPoint].y - this.transform.position.y).normalized * MoveSpeed;
        if (this.GetComponent<Animator>()) AnimDir(rb.velocity.x,rb.velocity.y);
    }
    void AnimDir(float x,float y)
    {
        Animator npcAnim = this.GetComponent<Animator>();
        npcAnim.speed = AnimSpeed;
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x <= 0)
            {
                npcAnim.SetFloat("X", -1f);
                npcAnim.SetFloat("Y", 0f);
            }
            else
            {
                npcAnim.SetFloat("X", 1f);
                npcAnim.SetFloat("Y", 0f);
            }
            return;
        }
        if (y <= 0)
        {
            npcAnim.SetFloat("X", 0f);
            npcAnim.SetFloat("Y", -1f);
        }
        else
        {
            npcAnim.SetFloat("X", 0f);
            npcAnim.SetFloat("Y", 1f);
        }
    }

    public void InitNPC(List<int> x, List<int> y,string func)
    {
        Routes = new List<Route>();
        for(int i = 0; i < x.Count; i++)
        {
            Route r=new Route();
            r.x = x[i];r.y = y[i];
            Routes.Add(r);
        }
        FuncName = func;
        nowPoint = 0;
        Debug.LogError("InitNPC"+nowPoint.ToString());
    }
    private void DeleteNPC()
    {
        Destroy(this.gameObject);
    }

    //Event‚Ì‹N“®
    private void FirstEihei()
    {
        GameManager.instance.eventManager.GoalEihei();
    } 

}

[Serializable]
public class Route
{
    public int x, y;
}