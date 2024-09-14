using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class transparentEffect : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField]
    float destTime=2f;
    float destCount = 0;
    float intensity = 0;
    float outer = 0;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (GetComponent<Light2D>())
        {
            intensity = GetComponent<Light2D>().intensity;
            outer = GetComponent<Light2D>().pointLightOuterRadius;
        }
    }
    // Update is called once per frame
    void Update()
    {
        destCount += Time.deltaTime;
        float scale = 0.8f*(destTime - destCount) / destTime;
        scale = scale > 0 ? scale : 0;
        this.transform.localScale = new Vector2(scale,scale);
        sr.color = new Color(1, 1, 1, scale);
        if (GetComponent<Light2D>())
        {
            GetComponent<Light2D>().intensity = intensity * scale;
            GetComponent<Light2D>().pointLightOuterRadius = outer*scale;
        }
        if (destCount < destTime) return;
        Destroy(this.transform.gameObject);
    }
}
