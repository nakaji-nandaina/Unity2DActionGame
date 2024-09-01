using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnEffectedDegree : MonoBehaviour
{
    GameObject parentObj;
    [SerializeField]
    float offsetY;
    // Start is called before the first frame update
    void Start()
    {
        parentObj=transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.transform.position = new Vector3(parentObj.transform.position.x, parentObj.transform.position.y + offsetY, this.transform.position.z);
        if(transform.rotation.z!=0)Debug.Log(transform.rotation.z);
    }
}
