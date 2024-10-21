using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dropItem;
    [SerializeField]
    private int[] dropP;
    private int dropCount;
    [SerializeField]
    private int max=100;
    // Start is called before the first frame update
    void Start()
    {
        dropCount = dropItem.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop(Vector2 sponePos)
    {
        int r = Random.Range(1, max+1);
        int now = 0;
        for(int i=0; i<dropCount; i++)
        {
            now += dropP[i];
            if (now >= r)
            {
                sponePos = this.transform.position;
                GameObject DropObj = Instantiate(dropItem[i], sponePos, Quaternion.Euler(0, 0, 0));
                return;
            }
        }
        
    }
}
