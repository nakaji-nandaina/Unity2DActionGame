using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMa : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameMa instance;

    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Slider SceneSlider;

    public GameObject dialogBox;
    public Text dialogText;
    public GameObject nameSpace;
    public Text CharName;
    public GameObject SceneSlide;
    private KaidanMoveScene kaidan;
    //前のシーンのプレイヤーの状態を格納する変数
    public static Vector2 StartSpone;
    public static float playerX;
    public static float playerY;

    public static int currentXP;
    public static int nextXP;

    private string[] dialogLines;

    private int currentLine;

    private bool justStarted;
    private bool isWriting;
    private float writingSpeed;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            currentXP = 0;
            nextXP = 100;
        }
    }

    void Start()
    {
        isWriting = false;
        writingSpeed = 0.1f;
        PlayerState();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (!justStarted)
                {
                    if (!isWriting)
                    {
                        currentLine++;
                        dialogText.text = "";
                        if (currentLine >= dialogLines.Length)
                        {
                            dialogBox.SetActive(false);
                            nameSpace.SetActive(false);
                        }
                        else
                        {
                            writingSpeed = 0.1f;
                            StartCoroutine(IEWrite(dialogLines[currentLine]));
                            //dialogText.text = dialogLines[currentLine];
                        }
                    }
                    else
                    {
                        writingSpeed = 0f;
                    }
                    
                }
                else
                {
                    justStarted = false;
                }
            }
        }
        player.kaiwaNow = dialogBox.activeInHierarchy;
    }

    private void PlayerState()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
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
        hpSlider.maxValue = player.maxHealth;
        hpSlider.value = player.currentHealth;
        string hptext = Convert.ToString(player.currentHealth) + "/" + Convert.ToString(player.maxHealth);
        hpText.text = hptext;
    }

    public void ShowDialog(string[] lines, string Name)
    {
        dialogLines = lines;
        currentLine = 0;
        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);
        if (Name != "")
        {
            nameSpace.SetActive(true);
            CharName.text = Name;
        }
        justStarted = true;
    }
    public void ShowDialogChange(bool x)
    {
        dialogBox.SetActive(x);
        nameSpace.SetActive(x);
    }

    private

    IEnumerator IEWrite(string s)
    {
        isWriting = true;
        for (int i=0; i < s.Length; i++){
            dialogText.text += s.Substring(i, 1);
            yield return new WaitForSeconds(writingSpeed);
        }
        isWriting = false;
    }

}