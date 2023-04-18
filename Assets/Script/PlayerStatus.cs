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
    int GOLD;
    [SerializeField]
    int CurrentLv;

    [SerializeField]
    List<int> ItemIds;
    [SerializeField]
    List<int> ShortcutIds;
    [SerializeField]
    List<int> ItemAmounts;
    [SerializeField]
    List<int> SkillIds;
    [SerializeField]
    List<int> SkillLvs;

    private const string SAVEKEY = "PLAYER-STATUS-SAVE";

    static PlayerStatus Instance = null;

    public int currentXp
    {
        get { return CurrentXp; }
        private set { CurrentXp = value; }
    }
   
   
    public int Gold
    {
        get { return GOLD; }
        private set { GOLD = value; }
    }
    
    public int currentLv
    {
        get { return CurrentLv; }
        private set { CurrentLv = value; }
    }

    public List<int> itemIds
    {
        get { return ItemIds; }
        private set { ItemIds = value; }
    }
    public List<int> shortcutIds
    {
        get { return ShortcutIds; }
        private set { ShortcutIds = value; }
    }

    public List<int> itemAmounts
    {
        get { return ItemAmounts; }
        private set { ItemAmounts = value; }
    }
    public List<int> skillIds
    {
        get { return SkillIds; }
        private set { SkillIds = value; }
    }

    public List<int> skillLvs
    {
        get { return SkillLvs; }
        private set { SkillLvs = value; }
    }

    public PlayerStatus()
    {
        CurrentXp = 0;
        GOLD = 0;
        CurrentLv = 1;
        ItemIds = null;
        ShortcutIds = null;
        ItemAmounts = null;
        SkillLvs = null;
        SkillIds = null;
    }
    

    public  PlayerStatus(int currentXp,int Gold,int currentLv,List<int> itemids,List<int> shortcutids,List<int>itemAmounts, List<int> skillids, List<int> skillLvs)
    {
        this.CurrentXp = currentXp;
        this.GOLD = Gold;
        this.CurrentLv = currentLv;
        this.ItemAmounts = itemAmounts;
        this.ShortcutIds = shortcutids;
        this.ItemIds = itemids;
        this.SkillIds = skillids;
        this.SkillLvs = skillLvs;
    }

    public void ReStatus(int cx, int gold, int cl,List<int> itemids, List<int> shortcutids, List<int> amounts,List<int> skillids,List<int> skilllvs)
    {
        currentXp = cx;
        Gold = gold;
        currentLv = cl;
        itemAmounts = amounts;
        itemIds = itemids;
        shortcutIds = shortcutids;
        skillIds = skillids;
        skillLvs = skilllvs;
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
        Debug.LogError(statusJson);
    }

    public void Load()
    {
        string statusJson = PlayerPrefs.GetString(SAVEKEY,JsonUtility.ToJson(new PlayerStatus()));
        Instance = JsonUtility.FromJson<PlayerStatus>(statusJson);
        Debug.LogError(statusJson);
    }

}
