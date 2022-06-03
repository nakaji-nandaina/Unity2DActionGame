using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dropItem;
    [SerializeField]
    private int dropP=50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop(Vector2 sponePos)
    {
        int r = Random.Range(1, 101);
        if (r <= dropP)
        {
            int i = Random.Range(0, dropItem.Length);
            sponePos = this.transform.position;
            GameObject ShotObj = Instantiate(dropItem[i], sponePos, Quaternion.Euler(0, 0, 0));
        }
    }
}
