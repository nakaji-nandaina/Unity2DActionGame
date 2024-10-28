using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEvent : MonoBehaviour
{
    [Tooltip("�V�[���ǂݍ��ݎ��ɌĂ΂��֐�")]
    public string StartEventFuncName="NullReturn";
    // Start is called before the first frame update
    void Start()
    {
        Invoke(StartEventFuncName, 0);
    }
    private void NullReturn()
    {
        return;
    }
    private void SetFirstItem()
    {
        GameManager.instance.eventManager.SetFirstItem(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
