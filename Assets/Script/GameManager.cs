using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    public static List<int> itemId, shortcutId, itemAmount, skillId, skillLv, weaponId;
    public static int mainWeapon;
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

    private DialogState ds;

    //効果音BGM関係
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip talkdot;
    //フェードアウト用
    [HideInInspector]
    public bool isFadeout = false;
    [HideInInspector]
    public bool isFadein = false;

    //敵のダメージUI
    public GameObject damageText;

    //インベントリ関係
    [HideInInspector]
    public InventoryUI inventoryUI;
    [HideInInspector]
    public WeaponPouchUI weaponUI;

    public GameObject[] shortcutButtons;

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
                NormalDialog = false;
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
            //Debug.LogError("Load");
            //DontDestroyOnLoad(this.gameObject);
            instance = this;
            if (load)
            {
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
        
        audioSource = GetComponent<AudioSource>();
        //isWriting = false;
        writingSpeed = writingDef;
        PlayerState();
        UpdateMoneyUI(currentMoney);
        YesChoice = false;
        NoChoice = false;
        NormalDialog = false;
        DialogFuncName = "NullReturn";
        inventoryUI = GetComponent<InventoryUI>();
        weaponUI = GetComponent<WeaponPouchUI>();
        //Cursor.SetCursor(CursolImage, Vector2.zero, CursorMode.Auto);
        //alfa = fadeImage.color.a;
        //setFadein();
    }

    // Update is called once per frame
    void Update()
    {

        DialogControll();
        /*
        Fadeout();
        Fadein();
        Debug.Log(Convert.ToString(alfa));
        */
    }

    private void DialogControll()
    {
        switch (ds)
        {
            case DialogState.wait:
                return;
            case DialogState.write:
                if (Input.GetMouseButtonUp(1))
                {
                    if (justStarted)
                    {
                        Debug.Log("convstart");
                        justStarted = false;
                        break;
                    }
                    writingSpeed = 0f;
                    changeDs(DialogState.conv);
                }
                break;
            case DialogState.conv:
                if (Input.GetMouseButtonUp(1))
                {
                    currentLine++;
                    Debug.Log("Next");
                    if (currentLine >= dialogLines.Length && !Choice)
                    {
                        changeDs(DialogState.wait);
                        Debug.Log("Stop");
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
                        //StartCoroutine(IEWrite(dialogLines[currentLine]));
                        changeDs(DialogState.write);
                        Invoke(DialogFuncName, 0);
                        DialogFuncName = "NullReturn";
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
                        //StartCoroutine(IEWrite(dialogLines[currentLine]));
                        changeDs(DialogState.write);
                        DialogFuncName = "NullReturn";
                    }
                }
                break;
        }
        /***
        if (dialogBox.activeInHierarchy && NormalDialog)
        {
            if (Input.GetMouseButtonUp(1) || Choiced)
            {
                Choiced = false;
                if (justStarted)
                {
                    justStarted = false;
                    return;
                }
                if (!isWriting)
                {
                    currentLine++;
                    if (currentLine >= dialogLines.Length && !Choice)
                    {
                        dialogText.text = "";
                        dialogBox.SetActive(false);
                        nameSpace.SetActive(false);
                        choiceBox.SetActive(false);
                        NormalDialog = false;
                        player.changePS(PlayerController.PS.normal);
                    }
                    else if (currentLine >= dialogLines.Length && Choice)
                    {
                        choiceBox.SetActive(true);
                        Debug.Log(YesChoice + "Yesc");
                        Debug.Log(NoChoice + "Noc");
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
                            StartCoroutine(IEWrite(dialogLines[currentLine]));
                            Invoke(DialogFuncName, 0);
                            DialogFuncName = "NullReturn";
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
                            StartCoroutine(IEWrite(dialogLines[currentLine]));
                            DialogFuncName = "NullReturn";
                        }

                    }
                    else
                    {
                        dialogText.text = "";
                        writingSpeed = writingDef;
                        StartCoroutine(IEWrite(dialogLines[currentLine]));
                        //dialogText.text = dialogLines[currentLine];
                    }
                }
                else
                {
                    writingSpeed = 0f;
                }


            }
        }
        
        if (dialogBox.activeInHierarchy)
        {
            switch (player.ps)
            {
                case PlayerController.PS.conversation:
                    break;
                default:
                    player.changePS(PlayerController.PS.conversation);
                    break;
            }
            player.changePS(PlayerController.PS.conversation);
        }
        ***/
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
        Debug.LogError("状態保存");
    }
    private void PlayerState()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player.nextXP = nextXP;
        player.currentXP = currentXP;
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


    public void ShowDialog(string[] lines, string Name, bool yesno, string[] YesLines, string[] NoLines, GameObject target, bool isfunc, string funcName)
    {
        NormalDialog = true;
        
        dialogText.text = "";
        dialogLines = lines;
        currentLine = 0;
        /***
        writingSpeed = writingDef;
        StartCoroutine(IEWrite(dialogLines[currentLine]));
        ***/
        //dialogText.text = dialogLines[currentLine];
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

    /*
    private void Fadeout()
    {
        if (isFadeout)
        {
            
            alfa += 0.005f;
            fadeCount = -0.002f;
            //fadeImage.color = new Color(0, 0, 0, alfa);
            Debug.Log("フェード中");
        }
        if (alfa>=1)
        {
            
            alfa = 1;
            Debug.Log("フェードおわり");
            fadeCount =fadeTime;
            isFadeout = false;
            Time.timeScale = 1;
        }
    }
    private void Fadein()
    {
        if (isFadein)
        {
            isFadein = true;
            
            alfa -= 0.02f;
            fadeCount -= 0.002f;
            //fadeImage.color = new Color(0, 0, 0, alfa);
            if (alfa <= 0)
            {
                
                fadeCount = fadeTime;
                isFadein = false;
                Time.timeScale = 1;

            }
        }
        
    }

    public void setFadeout()
    {
        
        alfa = 0f;
        isFadeout = true;
        //fadeImage.color = new Color(0, 0, 0, alfa);
        Time.timeScale = 0;
        fadeCount = fadeTime;
        Fadeout();
    }
    public void setFadein()
    {
        
        isFadein = true;
        alfa = 1;
        //fadeImage.color = new Color(0, 0, 0, 1);
        Fadein();
        Time.timeScale = 0;
        fadeCount = fadeTime;
    }
    */




    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private

    IEnumerator IEWrite(string s)
    {
        //isWriting = true;
        for (int i = 0; i < s.Length; i++)
        {
            if (writingSpeed != 0)
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
    private void SaveData()
    {
        player.SavePlayer();
    }

}