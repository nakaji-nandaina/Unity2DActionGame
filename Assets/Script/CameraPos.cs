using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    Vector2 playerpos;
   
    private void Update()
    {
        playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;
        this.gameObject.transform.position = new Vector3(playerpos.x, playerpos.y, this.transform.position.z);
    }

}
