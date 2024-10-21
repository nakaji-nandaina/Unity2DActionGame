using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickYes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void clickyes()
    {
        GameManager.instance.ChoiceYesDialog();
    }
    public void clickno()
    {
        GameManager.instance.ChoiceNoDialog();
    }
}
