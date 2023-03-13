using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemExplainUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Item item;
    public void OnclickItem()
    {    
        GameManager.instance.inventoryUI.UpdateExplainUI(item);
    }
}
