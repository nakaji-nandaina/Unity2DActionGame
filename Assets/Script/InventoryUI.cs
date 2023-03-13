using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject InventoryBox;
    public GameObject contentHolder;
    public GameObject buttonprefab;
    public Text name;
    public Text explain;
    

    public void UpdateExplainUI(Item item)
    {
        name.text = item.itemname;
        explain.text = item.information;
    }
    public void UpdateInventoryUI(InventoryObject inventory)
    {
        int currentButtonCount = contentHolder.transform.childCount;
        int currentItemCount = inventory.Container.Count;

        //アイテム表示欄の追加・削除
        if (currentButtonCount < currentItemCount)
        {
            int num = currentItemCount;
            for(int i = 0; i < num; i++)
            {
                GameObject newButtonObject = Instantiate(buttonprefab);
                newButtonObject.transform.SetParent(contentHolder.transform, false);
            }
        }
        else if(currentButtonCount>currentItemCount)
        {
            for(int i = currentItemCount - 1; i > currentItemCount; i++)
            {
                Destroy(contentHolder.transform.GetChild(i).gameObject);
            }
        }

        for(int i=0; i < currentItemCount; i++)
        {
            InventorySlot inventorySlot= inventory.Container[i];
            int amount = inventorySlot.amount;
            Item itemData = inventorySlot.item;
            GameObject buttonObject = contentHolder.transform.GetChild(i).gameObject;
            buttonObject.GetComponent<ItemExplainUI>().item = itemData;
            Text text = buttonObject.GetComponentInChildren<Text>();
            text.text = itemData.itemname+" "+"×"+amount.ToString();
            Image image= buttonObject.transform.GetChild(1).gameObject.GetComponent<Image>();
            image.sprite = itemData.itemIcon;
        }
    }

}
