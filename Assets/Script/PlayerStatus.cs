using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
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
    [SerializeField]
    List<int> WeaponIds;
    [SerializeField]
    List<int> QuestIds;
    [SerializeField]
    List<int> QuestContent1;
    [SerializeField]
    List<int> QuestContent2;
    [SerializeField]
    List<int> QuestContent3;
    [SerializeField]
    int MainWeapon;

    // JSONファイルのパス
    private static readonly string SaveFilePath = Path.Combine(Application.persistentDataPath, "playerstatus.json");

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
    public List<int> weaponIds
    {
        get { return WeaponIds; }
        private set { WeaponIds = value; }
    }

    public List<int> questIds
    {
        get { return QuestIds; }
        private set { QuestIds = value; }
    }

    public List<int> questContent1
    {
        get { return QuestContent1; }
        private set { QuestContent1 = value; }
    }

    public List<int> questContent2
    {
        get { return QuestContent2; }
        private set { QuestContent2 = value; }
    }

    public List<int> questContent3
    {
        get { return QuestContent3; }
        private set { QuestContent3 = value; }
    }

    public int mainWeapon
    {
        get { return MainWeapon; }
        private set { MainWeapon = value; }
    }

    public PlayerStatus()
    {
        CurrentXp = 0;
        GOLD = 0;
        CurrentLv = 1;
        ItemIds = new List<int>();
        ShortcutIds = new List<int>();
        ItemAmounts = new List<int>();
        SkillLvs = new List<int>();
        SkillIds = new List<int>();
        WeaponIds = new List<int>();
        QuestIds = new List<int>();
        QuestContent1 = new List<int>();
        QuestContent2 = new List<int>();
        QuestContent3 = new List<int>();
        MainWeapon = 0;
    }

    public PlayerStatus(int currentXp, int Gold, int currentLv, List<int> itemids, List<int> shortcutids, List<int> itemAmounts, List<int> skillids, List<int> skilllvs, List<int> weaponids, List<int> questids, List<int> questcontent1, List<int> questcontent2, List<int> questcontent3, int mainweapon)
    {
        this.CurrentXp = currentXp;
        this.GOLD = Gold;
        this.CurrentLv = currentLv;
        this.ItemAmounts = itemAmounts;
        this.ShortcutIds = shortcutids;
        this.ItemIds = itemids;
        this.SkillIds = skillids;
        this.SkillLvs = skilllvs;
        this.WeaponIds = weaponids;
        this.QuestIds = questids;
        this.QuestContent1 = questcontent1;
        this.QuestContent2 = questcontent2;
        this.QuestContent3 = questcontent3;
        this.MainWeapon = mainweapon;
    }

    public void ReStatus(int cx, int gold, int cl, List<int> itemids, List<int> shortcutids, List<int> amounts, List<int> skillids, List<int> skilllvs, List<int> weaponids, List<int> questids, List<int> questcontent1, List<int> questcontent2, List<int> questcontent3, int mainweapon)
    {
        currentXp = cx;
        Gold = gold;
        currentLv = cl;
        itemAmounts = amounts;
        itemIds = itemids;
        shortcutIds = shortcutids;
        skillIds = skillids;
        skillLvs = skilllvs;
        weaponIds = weaponids;
        questIds = questids;
        questContent1 = questcontent1;
        questContent2 = questcontent2;
        questContent3 = questcontent3;
        mainWeapon = mainweapon;
    }

    public static PlayerStatus GetInstance()
    {
        if (Instance == null)
        {
            if (File.Exists(SaveFilePath))
            {
                string statusJson = File.ReadAllText(SaveFilePath);
                Instance = JsonUtility.FromJson<PlayerStatus>(statusJson);
            }
            else
            {
                Instance = new PlayerStatus();
            }
        }
        return Instance;
    }

    public void Save()
    {
        string statusJson = JsonUtility.ToJson(this, true);
        File.WriteAllText(SaveFilePath, statusJson);
        Debug.LogError("データを保存しました: " + SaveFilePath + "\n" + statusJson);
    }

    public void Load()
    {
        if (File.Exists(SaveFilePath))
        {
            string statusJson = File.ReadAllText(SaveFilePath);
            Instance = JsonUtility.FromJson<PlayerStatus>(statusJson);
            Debug.LogError("データを読み込みました: " + SaveFilePath + "\n" + statusJson);
        }
        else
        {
            Instance = new PlayerStatus();
            Debug.LogError("セーブファイルが見つかりませんでした: " + SaveFilePath);
        }
    }
    public void Delete()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
            Instance = new PlayerStatus(); // データを初期化
            Debug.LogError("保存データを削除しました: " + SaveFilePath);
        }
        else
        {
            Debug.LogError("削除するファイルが見つかりません: " + SaveFilePath);
        }
    }
}
