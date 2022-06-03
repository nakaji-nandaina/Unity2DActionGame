using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCanim : MonoBehaviour
{
    [SerializeField]
    private Animator npcAnim;
    [SerializeField]
    private float animX, animY;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        npcAnim.SetFloat("X", animX);
        npcAnim.SetFloat("Y", animY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChageDir()
    {
        Debug.Log("向き変えるよ");
        Vector3 dir = player.transform.position - this.transform.position;
        if (Mathf.Abs(dir.x)<=Mathf.Abs(dir.y))
        {
            if (dir.y <= 0)
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
        else
        {
            if (dir.x <= 0)
            {
                npcAnim.SetFloat("X", -1f);
                npcAnim.SetFloat("Y", 0f);
            }
            else
            {
                npcAnim.SetFloat("X", 1f);
                npcAnim.SetFloat("Y", 0f);
            }
        }
    }
}
