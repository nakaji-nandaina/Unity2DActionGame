using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakTwoms : MonoBehaviour
{
    // Start is called before the first frame update
    float breaktime = 2f;
    float breakcount = 0f;
    void Start()
    {
        breaktime = 2f;
        breakcount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        breakcount += Time.deltaTime;
        if (breakcount >= breaktime) Destroy(this.gameObject);
    }
    public void setBreakTime(float t)
    {
        breaktime = t;
        breakcount = 0;
    }
}
