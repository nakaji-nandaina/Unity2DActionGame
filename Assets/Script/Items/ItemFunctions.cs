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
    public void HealPlayer(int healpoint)
    {
        GameManager.instance.Player.currentHealth += healpoint;
        GameManager.instance.PlayAudio(GameManager.instance.clips[0]);
        if (GameManager.instance.Player.currentHealth > GameManager.instance.Player.maxHealth)
        {
            GameManager.instance.Player.currentHealth = GameManager.instance.Player.maxHealth;
        }
        GameManager.instance.UpdateHealthUI();
    }
    public void AttackUpPlayer(Item item)
    {
        TBuff buff = new TBuff();
        buff.bufftype = TBuff.BuffType.attack;
        buff.id=GameManager.instance.Player.database.GetItemId(item);
        AttackUPItem atitem = (AttackUPItem)item;
        buff.bufftime = atitem.uptime;
        buff.buffvalue = atitem.upvalue;
        GameManager.instance.Player.AddBuff(buff);
    }
}
