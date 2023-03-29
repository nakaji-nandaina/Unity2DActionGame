using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortCutItem : MonoBehaviour
{
    [SerializeField]
    private int num;
    // Start is called before the first frame update
    public void Onclick()
    {
        if (GameManager.instance.inventoryUI.InventoryBox.activeInHierarchy)
        {
            GameManager.instance.inventoryUI.UpdateShortCutUI(num, GameManager.instance.Player.inventory, GameManager.instance.Player.ShortCut);
        }
        else
        {
            GameManager.instance.inventoryUI.ShortcutUseItem(num, GameManager.instance.Player.inventory, GameManager.instance.Player.ShortCut);
        }
    }
}
