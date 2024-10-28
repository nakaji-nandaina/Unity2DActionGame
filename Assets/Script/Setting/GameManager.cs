using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class TBuff
{
    public enum BuffType
    {
        attack,
        diffence,
    }
    public float buffvalue;
    public float bufftime;
    public int id;
    public BuffType bufftype;
}
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;

    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Slider xpSlider;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text xpText;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Slider SceneSlider;
    [SerializeField]
    private Text moneyText;

    //会話システムUI
    public GameObject dialogBox;
    public Text dialogText;
    public GameObject nameSpace;
    public Text CharName;
    public GameObject choiceBox;
    [HideInInspector]
    public bool Choice;
    [HideInInspector]
    public bool YesChoice;
    [HideInInspector]
    public bool NoChoice;
    [HideInInspector]
    public bool Isdialog;
    [HideInInspector]
    public bool NormalDialog;

    public GameObject SceneSlide;
    private KaidanMoveScene kaidan;
    //前のシーンのプレイヤーの状態を格納する変数
    [HideInInspector]
    public static Vector2 StartSpone;
    [HideInInspector]
    public static float playerX,playerY;
    [HideInInspector]
    public static int currentXP,nextXP,currentlevel,maxHealth, currentHealth ,currentMoney = 0;
    [HideInInspector]
    public static List<int> itemId, shortcutId, itemAmount, skillId, skillLv, weaponId,questId;
    [HideInInspector]
    public static List<Quest> boardQuest;
    [HideInInspector]
    public static List<List<int>> questContent;
    [HideInInspector]
    public static int mainWeapon;
    [HideInInspector]
    public static List<TBuff> tbuffs;
    [HideInInspector]
    public static List<bool> startEventFlag = new List<bool>(new bool[100]);
    [HideInInspector]
    public static List<bool> finishedEventFlag = new List<bool>(new bool[100]);
    [HideInInspector]
    public static bool load = true;

    //会話システム関係
    private string[] dialogLines;
    private string[] yesDialogLines;
    private string[] noDialogLines;
    private int currentLine;

    private bool justStarted;
    private bool isWriting;
    private float writingSpeed;
    private float writingDef = 0.05f;

    private string DialogFuncName;
    private string DialogYesFuncName;
    private string DialogNoFuncName;
    private bool canWrite;
    private DialogState ds;

    public GameObject Impulse;

    //効果音BGM関係
    private AudioSource audioSource;
    private AudioSource BGMSource;
    private AudioClip ChangedBGM;
    [SerializeField]
    private AudioClip talkdot;
    public AudioClip[] clips;
    //フェードアウト用
    [HideInInspector]
    public bool isFadeout = false;
    [HideInInspector]
    public bool isFadein = false;

    //ゲームオーバー画面
    public GameObject GameOverUI;

    //敵のダメージUI
    public GameObject damageText;
    //インベントリフルUI
    public GameObject InventoryFullText;
    //バフ表示UI
    [Tooltip("バフ表示コンテナ")]
    public GameObject BuffContainer;
    [Tooltip("バフ表示パネル")]
    public GameObject BuffPanel;
    [Tooltip("攻撃バフ画像")]
    public Sprite AttackUpImage;
    [Tooltip("攻撃バフ画像")]
    public Sprite DiffenceUpImage;
    [Tooltip("回避画像")]
    public Sprite AvoidCooltimeImage;

    //ミニマップ階段位置表示
    public GameObject nextPointUI;
    public Vector2 nextPoint;

    //インベントリ関係
    [HideInInspector]
    public InventoryUI inventoryUI;
    [HideInInspector]
    public WeaponPouchUI weaponUI;
    [HideInInspector]
    public WeaponStore weaponStore;
    [HideInInspector]
    public QuestUI questUI;
    [HideInInspector]
    public QuestBoard questBoard;
    [HideInInspector]
    public NoticeUI pulldownNotice;

    public GameObject[] shortcutButtons;

    //イベント関係
    public EventManager eventManager;

    //敵の死亡演出
    [HideInInspector]
    public BloodSpread bloodSpread;

    [HideInInspector]
    public TutorialManager tutorialManager;

    public PlayerController Player
    {
        get { return player; }
    }

    public enum DialogState
    {
        wait,
        write,
        conv,
        choice,
    }

    private void changeDs(DialogState nextds)
    {
        switch (nextds)
        {
            case DialogState.wait:
                ds = nextds;
                dialogText.text = "";
                dialogBox.SetActive(false);
                nameSpace.SetActive(false);
                choiceBox.SetActive(false);
                //NormalDialog = false;
                player.changePS(PlayerController.PS.normal);
                break;
            case DialogState.write:
                ds = nextds;
                dialogText.text = "";
                writingSpeed = writingDef;
                StartCoroutine(IEWrite(dialogLines[currentLine]));
                break;
            case DialogState.conv:
                ds = nextds;
                break;
            case DialogState.choice:
                ds = nextds;
                choiceBox.SetActive(true);
                break;
        }
    }
    //public Texture2D CursolImage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (load)
            {
                //保存されたデータの削除
                //PlayerPrefs.DeleteAll();
                load = false;
                player.LoadPlayer();
            }
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

    }

    void Start()
    {
        tutorialManager = GetComponent<TutorialManager>();
        bloodSpread = GetComponent<BloodSpread>();
        audioSource = GetComponent<AudioSource>();
        settingBGM();
        //Debug.LogError(GameObject.FindGameObjectWithTag("BGM"));
        //isWriting = false;
        writingSpeed = writingDef;
        PlayerState();
        UpdateMoneyUI(currentMoney);
        YesChoice = false;
        NoChoice = false;
        canWrite = true;
        //NormalDialog = false;
        DialogFuncName = "NullReturn";
        inventoryUI = GetComponent<InventoryUI>();
        weaponUI = GetComponent<WeaponPouchUI>();
        weaponStore = GetComponent<WeaponStore>();
        questUI = GetComponent<QuestUI>();
        questBoard = GetComponent<QuestBoard>();
        pulldownNotice = GetComponent<NoticeUI>();
        settingLight();
        settingNextPoint();
        settingEvent();
    }

    // Update is called once per frame
    void Update()
    {
        nextPointControll();
        DialogControll();
        
    }

    private void settingEvent()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartScene":
                settingStartSceneEvent();
                break;
            case "StartScene_2":
                settingStartScene_2Event();
                break;
        }
    }
    private void settingStartSceneEvent()
    {
        if (!finishedEventFlag[1])
        {
            nextPoint = new Vector2(-12, 11);
            nextPointUI.SetActive(true);
        }
    }
    private void settingStartScene_2Event()
    {
        //王様との会話イベント
        if (!finishedEventFlag[1])
        {
            nextPoint = new Vector2(-13, 28);
            nextPointUI.SetActive(true);
        }
        //ダンジョン初挑戦
        else if (!finishedEventFlag[2])
        {
            nextPoint = new Vector2(-12, 11);
            nextPointUI.SetActive(true);
        }
    }
    private void settingBGM()
    {
        GameObject BGMObj =new GameObject();
        BGMSource = BGMObj.AddComponent<AudioSource>();
        BGMSource.volume = 0.1f;
        audioSource.volume = 0.4f;
        BGMSource.loop = true;
        if (player.database.GetSceneBGM(SceneManager.GetActiveScene().name) == null) return;
        BGMSource.clip=player.database.GetSceneBGM(SceneManager.GetActiveScene().name);
        BGMSource.Play();
    }

    private void settingLight()
    {
        Light2D light = GetComponent<Light2D>();
        if (light==null) light =this.gameObject.AddComponent<Light2D>();
        float br = player.database.GetSceneBright(SceneManager.GetActiveScene().name);
        if (br == -1)
        {
            light.intensity = 0.9f;
            player.GetComponent<Light2D>().intensity = 0.2f;
            return;
        }
        light.intensity = br;
        player.GetComponent<Light2D>().intensity = 1-br;
    }

    private void settingNextPoint()
    {
        nextPoint = player.database.GetSceneGoal(SceneManager.GetActiveScene().name);
        if (nextPoint != Vector2.zero)nextPointUI.SetActive(true);
    }
    private void nextPointControll()
    {
        if (!nextPointUI.activeInHierarchy) return;
        // プレイヤーと目的地点の距離を計算
        Vector2 np = nextPoint - (Vector2)player.transform.position;
        float distance = np.magnitude;

        np = np.normalized;

        // 距離が近ければ半径とサイズを縮小
        float maxRadius = 130f; // 最大半径
        float minRadius = 0f; // 最小半径
        float maxDistance = 20f; // この距離よりも近ければ縮小開始
        // 距離に基づいて半径を計算
        float radius = Mathf.Lerp(minRadius, maxRadius, Mathf.Clamp01(distance / maxDistance));

        // 距離に応じてUIのサイズも調整
        float maxScale = 1f;
        float minScale = 0.3f;
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.Clamp01(distance / maxDistance));

        np = np.normalized;

        nextPointUI.GetComponent<RectTransform>().anchoredPosition= np * radius;
        nextPointUI.transform.rotation=Quaternion.Euler(0,0, -Mathf.Atan2(np.x, np.y)*180/Mathf.PI);
        nextPointUI.transform.localScale = new Vector3(scale, scale, 1); // サイズを縮小
        //Debug.LogError(nextPoint);
    }

    private void DialogControll()
    {
        switch (ds)
        {
            case DialogState.wait:
                return;
            case DialogState.write:
                if (Input.GetMouseButtonUp(0))
                {
                    if (justStarted)
                    {
                        Debug.LogError("convstart");
                        justStarted = false;
                        break;
                    }
                    writingSpeed = 0f;
                    //changeDs(DialogState.conv);
                }
                break;
            case DialogState.conv:
                if (!Input.GetMouseButtonUp(0)) return;
                currentLine++;
                Debug.LogError("Next");
                if (currentLine >= dialogLines.Length && !Choice)
                {
                    changeDs(DialogState.wait);
                    Invoke(DialogFuncName, 0);
                    DialogFuncName = "NullReturn";
                    Debug.LogError("Stop");
                }
                else if (currentLine >= dialogLines.Length && Choice)
                {
                    changeDs(DialogState.choice);
                }
                else
                {
                    changeDs(DialogState.write);
                    //dialogText.text = dialogLines[currentLine];
                }
         
                break;
            case DialogState.choice:
                if (Input.GetMouseButtonUp(0))
                {
                    
                    if (YesChoice)
                    {
                        dialogText.text = "";
                        YesChoice = false;
                        NoChoice = false;
                        choiceBox.SetActive(false);
                        Choice = false;
                        dialogLines = yesDialogLines;
                        currentLine = 0;
                        writingSpeed = writingDef;
                        changeDs(DialogState.write);
                        Invoke(DialogYesFuncName, 0);
                        //DialogYesFuncName = "NullReturn";
                    }
                    else if (NoChoice)
                    {
                        dialogText.text = "";
                        YesChoice = false;
                        NoChoice = false;
                        choiceBox.SetActive(false);
                        Choice = false;
                        dialogLines = noDialogLines;
                        currentLine = 0;
                        writingSpeed = writingDef;
                        changeDs(DialogState.write);
                        Invoke(DialogNoFuncName, 0);
                        //DialogNoFuncName = "NullReturn";
                    }
                }
                break;

        }
    }

    public void UpdateEventFlag(List<bool> _startEventFlag,List<bool> _finishedEventFlag)
    {
        startEventFlag = _startEventFlag;
        finishedEventFlag = _finishedEventFlag;
    }

    public void SaveEventFlag()
    {
        EventStatus.GetInstance().ReStatus(startEventFlag, finishedEventFlag);
        EventStatus.GetInstance().Save();
    }

    public void PlayerStateHold()
    {
        currentXP = player.currentXP;
        nextXP = player.nextXP;
        currentlevel = player.currentLevel;
        currentHealth = player.currentHealth;
        itemId = player.database.GetItemIds(player.inventory);
        shortcutId = player.database.GetItemIds(player.ShortCut);
        itemAmount = player.database.GetItemAmounts(player.inventory);
        skillId = player.database.GetSkillIds(player.skills);
        skillLv = player.database.GetSkillIds(player.skills);
        mainWeapon = player.mainWeapon;
        weaponId = player.database.GetWeaponIds(player.weaponPouch);
        questId = player.database.GetQuestIds(player.orderQuest);
        boardQuest = player.BoardQuests;
        questContent = player.database.GetQuestContent(player.orderQuest);

        tbuffs = player.tbuffs;
        Debug.LogError("状態保存");
    }
    private void PlayerState()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        //player.nextXP = nextXP;
        //player.currentXP = currentXP;
        if (StartSpone != null && StartSpone != Vector2.zero)
        {
            playerObj.transform.position = StartSpone;
            playerObj.GetComponent<Animator>().SetFloat("X", playerX);
            playerObj.GetComponent<Animator>().SetFloat("Y", playerY);
        }
    }

    public void UpdateSceneUI(bool IsCount, float STime, float Count)
    {
        SceneSlide.SetActive(IsCount);
        SceneSlider.maxValue = STime;
        SceneSlider.value = Count;
    }
    public void UpdateHealthUI()
    {
        currentHealth = player.currentHealth;
        maxHealth = player.maxHealth;
        hpSlider.maxValue = player.maxHealth;
        hpSlider.value = player.currentHealth;
        string hptext = Convert.ToString(player.currentHealth) + "/" + Convert.ToString(player.maxHealth);
        hpText.text = hptext;
    }

    public void UpdateXPUI()
    {
        currentXP = player.currentXP;
        nextXP = player.nextXP;
        currentlevel = player.currentLevel;
        xpSlider.maxValue = nextXP;
        xpSlider.value = currentXP;
        float xpRate = (float)currentXP / (float)nextXP * 100f;
        string xptext = Convert.ToString(Math.Floor(10 * xpRate) / 10) + "%";
        xpText.text = xptext;
        levelText.text = "Lv" + Convert.ToString(currentlevel);
    }

    public void UpdateMoneyUI(int money)
    {
        currentMoney = money;
        moneyText.text = Convert.ToString(currentMoney) + "G";
    }


    public void ShowDialog(string[] lines, string Name, bool yesno, string[] YesLines, string[] NoLines, GameObject target, string funcName,string YesfuncName,string NofuncName)
    {   
        dialogText.text = "";
        dialogLines = lines;
        currentLine = 0;  
        dialogBox.SetActive(true);
        if (Name != "")
        {
            nameSpace.SetActive(true);
            CharName.text = Name;
        }
        justStarted = true;
        Choice = yesno;
        yesDialogLines = YesLines;
        noDialogLines = NoLines;
        DialogFuncName = funcName;
        DialogYesFuncName = YesfuncName;
        DialogNoFuncName = NofuncName;
        player.changePS(PlayerController.PS.conversation);
        changeDs(DialogState.write);
    }
    public void ShowDialogChange(bool x)
    {
        dialogBox.SetActive(x);
        nameSpace.SetActive(x);
    }

    public void ChoiceYesDialog()
    {
        YesChoice = true;
        Debug.Log("Yes");
    }
    public void ChoiceNoDialog()
    {
        NoChoice = true;
        Debug.Log("No");
    }

    




    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void StopBGM()
    {
        if (BGMSource == null) return;
        BGMSource.Stop();
        //BGMSource.gameObject.SetActive(false);
        //Debug.LogError(BGMSource.isPlaying);
    }

    public void ChangeBGM(AudioClip clip,float f)
    {
        if (BGMSource == null) return;
        if (BGMSource.clip == clip) return;
        ChangedBGM = BGMSource.clip;
        BGMSource.clip = clip;
        BGMSource.Play();
    }
    public void TurnBGM()
    {
        if (BGMSource == null) return;
        if (ChangedBGM == null) return;
        BGMSource.clip = ChangedBGM;
        BGMSource.Play();
    }

    private IEnumerator IEWrite(string s)
    {
        //isWriting = true;
        for (int i = 0; i < s.Length; i++)
        {
            if (writingSpeed != 0&& s.Substring(i, 1)!=" " && s.Substring(i, 1) != "." && s.Substring(i, 1) != "。" && s.Substring(i, 1) != "、")
            {
                audioSource.PlayOneShot(talkdot);
            }
            dialogText.text += s.Substring(i, 1);
            yield return new WaitForSeconds(writingSpeed);
        }
        changeDs(DialogState.conv);
    }


    private void NullReturn()
    {

    }
    private void RoopConv()
    {
        Choice = true;
    }
    private void SaveData()
    {
        player.SavePlayer();
    }
    private void OpenQuestBoard()
    {
        player.OpenQuestBoard();
    }
    private void QuestReward()
    {
        List<string> convs=player.orderQuest.RewardString();
        //配列初期化
        dialogLines = convs.Count % 3 != 0 ?  new string[convs.Count/3+1]: new string[convs.Count / 3];
        for(int i = 0; i < convs.Count; i++)
        {
            dialogLines[i / 3] += convs[i] + "\n";
        }
        dialogText.text = "";
        nameSpace.SetActive(true);
        currentLine = 0;
        dialogBox.SetActive(true);
        player.orderQuest.CompleteQuests();
        player.changePS(PlayerController.PS.conversation);
        changeDs(DialogState.write);
    }

    private void OpenCraftWeaponUI()
    {
        Player.OpenWeaponCraft();
    }

    //イベント#0の会話後処理
    private void FinishStartConv()
    {
        eventManager.EndEihei();
    }
    //イベント#1の会話処理
    private void FirstKingConv()
    {
        eventManager.KingAfterFirstFunc();
        settingStartScene_2Event();
    }
}