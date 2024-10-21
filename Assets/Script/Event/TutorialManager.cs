using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject TutorialPanel;
    [SerializeField]
    private List<GameObject> TutorialObjs;
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
        }
    }

    private IEnumerator WaitTutorial(int ind,float time)
    {
        yield return new WaitForSeconds(5f);
        CloseTutorial(ind);
    }
}
