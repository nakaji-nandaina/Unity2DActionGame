using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroyBasisSet : MonoBehaviour
{
    public static NotDestroyBasisSet instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
