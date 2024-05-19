using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour
{
    public List<getItem> getItems;
    public GameObject pulldownView;
    public GameObject pulldownCont;

    //UI表示のための獲得アイテムの一時保存と保持時間のリセット
    public void AddItem(Item item)
    {
        for(int i = 0; i < getItems.Count; i++)
        {
            if (getItems[i].item == item)
            {
                getItems[i].count++;
                getItems[i].msTime = 2f;
                return;
            }
        }
        if (getItems.Count >= 5) return;
        getItem getitem=new getItem();
        getitem.item = item;getitem.count = 1;
        getitem.msTime = 2f;
        getItems.Add(getitem);
        Debug.LogError(item.itemname + "Additem");
    }
    private void Start()
    {
        getItems = new List<getItem>();
    }
    private void Update()
    {
        for(int i = getItems.Count-1; i >=0; i--)
        {
            getItems[i].msTime -= Time.deltaTime;
            if (getItems[i].msTime <= 0) getItems.RemoveAt(i);
        }
        int currentPulldownCount = pulldownView.transform.childCount;
        int currentGetitemCount = getItems.Count;
        if (currentGetitemCount > currentPulldownCount)
        {
            for(int i = 0; i < currentGetitemCount - currentPulldownCount; i++)
            {
                GameObject newpulldown = Instantiate(pulldownCont);
                newpulldown.transform.SetParent(pulldownView.transform, false);
            }
        }
        if(currentGetitemCount < currentPulldownCount)
        {
            for(int i = currentPulldownCount - 1; i >= currentGetitemCount; i--)
            {
                Destroy(pulldownView.transform.GetChild(i).gameObject);
            }
        }
        for (int i= 0; i < pulldownView.transform.childCount; i++)
        {
            GameObject getpanel = pulldownView.transform.GetChild(i).gameObject;
            getpanel.transform.GetChild(1).GetComponent<Image>().sprite = getItems[i].item.itemIcon;
            getpanel.transform.GetChild(0).GetComponent<Text>().text = "×"+getItems[i].count.ToString()+" かくとく";
        }
    }
}
public class getItem{
    
    public Item item;
    public int count;
    public float msTime =1f;
}
