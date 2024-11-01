using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject TutorialPanel;
    [SerializeField]
    private List<GameObject> TutorialObjs;
    /*
    #0 移動 ・回避
    #1 攻撃・アクション
    #2 セーブ
    #3 インベントリ
    */

    public void OpenTutorial(int ind,float time)
    {
        TutorialPanel.SetActive(true);
        TutorialObjs[ind].SetActive(true);
        StartCoroutine(WaitTutorial(ind,time));
    }
    public void CloseTutorial(int ind)
    {
        TutorialObjs[ind].SetActive(false);
        TutorialPanel.SetActive(false);
        switch (ind) {
            case 0:
                OpenTutorial(1, 5f);
                break;
            case 1:
                OpenTutorial(2, 5f);
                break;
        }
    }

    private IEnumerator WaitTutorial(int ind,float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            if (GameManager.instance.Player.ps==PlayerController.PS.normal)
            {
                elapsed += Time.deltaTime;
            }
            yield return null;
        }

        CloseTutorial(ind);
    }
}
