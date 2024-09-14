using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackEffect : MonoBehaviour
{
    [SerializeField]
    GameObject[] effects;
    [SerializeField]
    float width, height;
    [SerializeField]
    float effectTime;
    float count = 0;
    
    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if (effectTime > count) return;
        count = 0;
        int effectId = Random.Range(0, effects.Length);
        float x = Random.Range(-width, width);
        float y = Random.Range(-height, height);
        Debug.LogError(effectId.ToString()+"Effect");
        Instantiate(effects[effectId],new Vector3(this.transform.position.x+x,this.transform.position.y+y,this.transform.position.z), Quaternion.identity);
    }
}
