using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpread : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> RedBloods, BlueBloods;
    public void RedSpread(Vector2 pos)
    {
        int randid = Random.Range(0, RedBloods.Count);
        GameObject rb=Instantiate(RedBloods[randid]);
        rb.transform.position = pos;
    }
    public void BlueSpread(Vector2 pos)
    {
        int randid = Random.Range(0, BlueBloods.Count);
        GameObject rb = Instantiate(BlueBloods[randid]);
        rb.transform.position = pos;
    }
}
