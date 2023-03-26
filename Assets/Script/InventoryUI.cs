using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject InventoryBox;
    public GameObject contentHolder;
    public GameObject buttonprefab;
    public GameObject useButton;
    public Text name;
    public Text explain;
    public Item selecteditem;
    
    public void UpdateExplainUI(Item item)
    {
        selecteditem = item;
        name.text = item.itemname;
        explain.text = item.information;
        if (item.type == Item.Type.UserItem)
        {
            useButton.SetActive(true);
        }
        else
        {
            useButton.SetActive(false);
        }
    }

    public void useItem(InventoryObject inventory)
    {
        if(inventory.UsedItem(selecteditem, 1))
        {
            string func =selecteditem.funcname;
            ItemFunctions.instance.Invoke(func, 0);
            if (!inventory.existItem(selecteditem))
            {
                CloseInventory();
            }
            UpdateInventoryUI(inventory);
        }

    }

    public void CloseInventory()
    {
        selecteditem = null;
        name.text = "あいてむ";
        explain.text = "あいてむのせつめい";
        useButton.SetActive(false);
    }
    public void UpdateInventoryUI(InventoryObject inventory)
    {
        int currentButtonCount = contentHolder.transform.childCount;
        int currentItemCount = inventory.Container.Count;

        Debug.Log(currentItemCount);
        Debug.Log(currentButtonCount);

        //アイテム表示欄の追加・削除
        if (currentButtonCount < currentItemCount)
        {
            Debug.Log("アイテム追加");
            int num = currentItemCount-currentButtonCount;
            for(int i = 0; i < num; i++)
            {
                GameObject newButtonObject = Instantiate(buttonprefab);
                newButtonObject.transform.SetParent(contentHolder.transform, false);
            }
        }
        else if(currentButtonCount>currentItemCount)
        {
            Debug.Log("アイテム削除");
            for(int i = currentButtonCount-1; i >= currentItemCount; i--)
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
