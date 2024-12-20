﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryBox;
    public GameObject contentHolder;
    public GameObject buttonprefab;
    public GameObject useButton;
    //public GameObject[] shortcutButtons;
    public Text Name;
    public Text explain;
    public Item selecteditem;
    
    public void UpdateShortCutInventoryUI(InventoryObject shortcut)
    {
        for(int i = 0; i < 7; i++)
        {
            Item item = shortcut.Container[i].item;
            int num = GameManager.instance.Player.inventory.numItem(item);
            shortcut.Container[i].amount = num;

            if (item != null&&num!=0)
            {
                Debug.Log(num.ToString() + " " + selecteditem);
                GameManager.instance.shortcutButtons[i].GetComponentInChildren<Image>().sprite = item.itemIcon;
                GameManager.instance.shortcutButtons[i].GetComponentInChildren<Image>().color = new Color(255, 255, 255, 1);
                GameManager.instance.shortcutButtons[i].GetComponentInChildren<Text>().text = num.ToString();
            }
            else
            {
                shortcut.Container[i].item = null;
                shortcut.Container[i].amount = 0;
                GameManager.instance.shortcutButtons[i].GetComponentInChildren<Image>().color =new Color(0,0,0,0);
                GameManager.instance.shortcutButtons[i].GetComponentInChildren<Text>().text = "";
            }
        }
    }

    public void UpdateShortCutUI(int num,InventoryObject inventory,InventoryObject shortcut)
    {
        if (selecteditem == null) return;
        Debug.Log(num.ToString() + " " +selecteditem);
        //GameManager.instance.shortcutButtons[num].GetComponentInChildren<Image>().sprite = selecteditem.itemIcon;
        int amount = inventory.numItem(selecteditem);
        //GameManager.instance.shortcutButtons[num].GetComponentInChildren<Text>().text = amount.ToString();
        if (selecteditem.type == Item.Type.UserItem)
        {
            shortcut.Container[num].item = selecteditem;
            shortcut.Container[num].amount = amount;
            UpdateShortCutInventoryUI(shortcut);
        }
    }
    public void UpdateExplainUI(Item item)
    {
        selecteditem = item;
        Name.text = item.itemname;
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

    public void ShortcutUseItem(int num,InventoryObject inventory,InventoryObject shortcut)
    {
        Item item = shortcut.Container[num].item;
        if (!inventory.existItem(item)) return;
        if(!ItemEffect(item))return;
        inventory.UsedItem(item, 1);
        shortcut.ShortCutUsedItem(item, 1);
        UpdateInventoryUI(inventory);
        UpdateShortCutInventoryUI(shortcut);
        
    }

    public void useItem(InventoryObject inventory)
    {
        Debug.Log("アイテム使った");
        Debug.Log(selecteditem);
        //Debug.Log(selecteditem.GetType().ToString());
        if (!inventory.existItem(selecteditem)) return;//アイテムが存在するか確認
        if(!ItemEffect(selecteditem))return;//アイテムの効果が適用可能か確認し使用するorしない

        Debug.Log("tukatta");
        inventory.UsedItem(selecteditem, 1);
        if (!inventory.existItem(selecteditem))CloseInventory();
        UpdateInventoryUI(inventory);
        UpdateShortCutInventoryUI(GameManager.instance.Player.ShortCut);
    }
    public bool ItemEffect(Item _item)
    {
        bool used = true;
        switch (_item.GetType().ToString())
        {
            case nameof(HealItem):
                if (GameManager.instance.Player.currentHealth == GameManager.instance.Player.maxHealth)
                {
                    used = false;
                    break;
                }                
                HealItem healitem = (HealItem)_item;
                ItemFunctions.instance.HealPlayer(healitem.healvalue);
                break;
            case nameof(AttackUPItem):
                ItemFunctions.instance.AttackUpPlayer(_item);
                break;
        }
        return used;
    }

    public void CloseInventory()
    {
        selecteditem = null;
        Name.text = "あいてむ";
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
