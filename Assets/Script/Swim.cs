using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Debug.LogError("êÖ");
            GameManager.instance.Player.shadowobj.SetActive(false);
            GameManager.instance.Player.WaterMask.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            GameManager.instance.Player.shadowobj.SetActive(true);
            GameManager.instance.Player.WaterMask.SetActive(false);
        }
    }

}
