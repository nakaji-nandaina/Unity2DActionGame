using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject FallObj;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") Instantiate(FallObj, collision.transform.position, Quaternion.identity);
        if (collision.tag == "BossFirst") Instantiate(FallObj, collision.transform.position, Quaternion.identity);
    }
}
