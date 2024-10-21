using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireLight : MonoBehaviour
{
    Light2D light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float nextlight=light.pointLightOuterRadius+Random.Range(-0.06f, 0.06f);
        nextlight = nextlight < 2.8f ? 2.8f : nextlight;
        nextlight = nextlight > 3.2f ? 3.2f : nextlight;
        light.pointLightOuterRadius = nextlight;
    }
}
