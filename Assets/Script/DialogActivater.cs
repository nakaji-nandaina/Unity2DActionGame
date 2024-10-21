using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivater : MonoBehaviour
{
    [SerializeField, Header("会話文章"), Multiline(3)]
    private string[] lines;
    [SerializeField, Header("名前")]
    private string CharName;
    [SerializeField, Header("選択肢分岐")]
    private bool Choice=false;
    [SerializeField, Header("上選択文章"), Multiline(3)]
    private string[] YesLines;
    [SerializeField, Header("下選択文章"), Multiline(3)]
    private string[] NoLines;
    private bool canActivater;
    [SerializeField, Header("実行関数名")]
    private string funcName = "NullReturn";
    [SerializeField, Header("実行関数名")]
    private string YesfuncName = "NullReturn";
    [SerializeField, Header("実行関数名")]
    private string NofuncName = "NullReturn";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Player.ps != PlayerController.PS.normal) return;
        if(Input.GetMouseButtonDown(1) && canActivater && !GameManager.instance.dialogBox.activeInHierarchy)
        {
            GameManager.instance.ShowDialog(lines,CharName,Choice,YesLines,NoLines,this.gameObject,funcName,YesfuncName,NofuncName);
            if (this.gameObject.GetComponent<NPCanim>())
            {
                this.gameObject.GetComponent<NPCanim>().ChageDir();
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canActivater = true;

            if (this.transform.Find("hukidashi")) this.transform.Find("hukidashi").gameObject.SetActive(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canActivater = false;
            if (this.transform.Find("hukidashi")) this.transform.Find("hukidashi").gameObject.SetActive(false);
            //GameManager.instance.ShowDialogChange(canActivater);
        }
    }

    public void InitActivater(string[] _lines,bool _choice, string[] _yesLines, string[] _noLines,string _funcName,string _yesfuncName,string _nofuncName)
    {
        lines = _lines;
        Choice = _choice;
        YesLines = _yesLines;
        NoLines = _noLines;
        funcName = _funcName;
        YesfuncName = _yesfuncName;
        NofuncName = _nofuncName;
    }

}
