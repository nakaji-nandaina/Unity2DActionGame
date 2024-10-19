using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "CreateEnemy/Enemy")]

public class Enemy : ScriptableObject
{
    public String enemyName;
    public Sprite enemyImage;
    public int huntedNum=0;
    [Multiline(10)]
    public String Info;
    public AudioClip attackClip;
}
