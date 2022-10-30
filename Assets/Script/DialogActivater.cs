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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && canActivater && !GameManager.instance.dialogBox.activeInHierarchy)
        {
            GameManager.instance.ShowDialog(lines,CharName,Choice,YesLines,NoLines);
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

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canActivater = false;

            GameManager.instance.ShowDialogChange(canActivater);
        }
    }

}
