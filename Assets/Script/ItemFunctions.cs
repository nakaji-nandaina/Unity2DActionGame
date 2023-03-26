using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFunctions:MonoBehaviour
{
    public static ItemFunctions instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void HealHundred()
    {
        int point = 100;
        GameManager.instance.Player.currentHealth += point;
        if (GameManager.instance.Player.currentHealth > GameManager.instance.Player.maxHealth)
        {
            GameManager.instance.Player.currentHealth = GameManager.instance.Player.maxHealth;
        }
        GameManager.instance.UpdateHealthUI();
    }
}
