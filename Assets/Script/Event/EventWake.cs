using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EventWake : MonoBehaviour
{

    void Start()
    {
        EventManager eventManager = GameManager.instance.eventManager;
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartScene":
                //�����N���C�x���g
                eventManager.setStartEventFlag(0);
                if (!GameManager.finishedEventFlag[0]) eventManager.FirstGameEvent();
                break;
        }
    }

}
