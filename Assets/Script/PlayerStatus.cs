using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    // Start is called before the first frame update
    [SerializeField]
    int CurrentXp;
    [SerializeField]
    int NextXp;
    [SerializeField]
    int MaxHP;
    [SerializeField]
    int GOLD;
    [SerializeField]
    int AttackPOINT;
    [SerializeField]
    int CurrentLv;

    private const string SAVEKEY = "PLAYER-STATUS-SAVE";

    static PlayerStatus Instance = null;

    public int currentXp
    {
        get { return CurrentXp; }
        private set { CurrentXp = value; }
    }
    public int nextXp
    {
        get { return NextXp; }
        private set { NextXp = value; }
    }
    public int MaxHp
    {
        get { return MaxHP; }
        private set { MaxHP = value; }
    }
    public int Gold
    {
        get { return GOLD; }
        private set { GOLD = value; }
    }
    public int AttackPoint
    {
        get { return AttackPOINT; }
        private set { AttackPOINT = value; }
    }
    public int currentLv
    {
        get { return CurrentLv; }
        private set { CurrentLv = value; }
    }
    public PlayerStatus()
    {
        CurrentXp = 0;
        NextXp = 100;
        MaxHP = 100;
        GOLD = 0;
        AttackPOINT = 10;
        CurrentLv = 1;
    }
    

    public  PlayerStatus(int currentXp, int nextXp, int MaxHp, int Gold,int AttackPoint, int currentLv)
    {
        this.CurrentXp = currentXp;
        this.NextXp = nextXp;
        this.MaxHP = MaxHp;
        this.GOLD = Gold;
        this.AttackPOINT = AttackPoint;
        this.CurrentLv = currentLv;
    }

    public void ReStatus(int cx, int nx, int mh, int gold, int ap, int cl)
    {
        currentXp = cx;
        nextXp = nx;
        MaxHp = mh;
        Gold = gold;
        AttackPoint = ap;
        currentLv = cl;
    }

    

    public static PlayerStatus GetInstance()
    {
        if (Instance == null)
        {
            string statusJson = PlayerPrefs.GetString(SAVEKEY, JsonUtility.ToJson(new PlayerStatus()));
            Instance = JsonUtility.FromJson<PlayerStatus>(statusJson);
        }
        return Instance;
    }
   
    public void Save()
    {
        PlayerPrefs.SetString(SAVEKEY, JsonUtility.ToJson(this));
        PlayerPrefs.Save();
        string statusJson = PlayerPrefs.GetString(SAVEKEY, JsonUtility.ToJson(new PlayerStatus()));
        //Debug.LogError(statusJson);
    }

    public void Load()
    {
        string statusJson = PlayerPrefs.GetString(SAVEKEY,JsonUtility.ToJson(new PlayerStatus()));
        Instance = JsonUtility.FromJson<PlayerStatus>(statusJson);
        //Debug.LogError(statusJson);
    }

}
