using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    [SerializeField]
    AudioClip clip;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.ChangeBGM(clip,0);
        }
    }
}
