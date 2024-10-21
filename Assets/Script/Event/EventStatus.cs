using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventStatus
{
    private const string SAVEKEY = "EVENT-STATUS-SAVE";
    [SerializeField]
    List<bool> StartEventFlag;
    [SerializeField]
    List<bool> FinishedEventFlag;

    private static EventStatus Instance = null;

    public List<bool> startEventFlag
    {
        get { return StartEventFlag; }
        private set { StartEventFlag = value; }
    }

    public List<bool> finishedEventFlag
    {
        get { return FinishedEventFlag; }
        private set { FinishedEventFlag = value; }
    }

    public EventStatus()
    {
        startEventFlag = new List<bool>(new bool[100]);
        finishedEventFlag = new List<bool>(new bool[100]);
    }

    public EventStatus(List<bool> _startEventFlag,List<bool> _finishedEventFlag)
    {
        this.StartEventFlag = _startEventFlag;
        this.FinishedEventFlag = _finishedEventFlag;
    }

    public void ReStatus(List<bool> _startEventFlag, List<bool> _finishedEventFlag)
    {
        startEventFlag = _startEventFlag;
        finishedEventFlag = _finishedEventFlag;
    }

    public static EventStatus GetInstance()
    {
        if (Instance == null)
        {
            string statusJson = PlayerPrefs.GetString(SAVEKEY, JsonUtility.ToJson(new EventStatus()));
            Instance = JsonUtility.FromJson<EventStatus>(statusJson);
        }
        return Instance;
    }

    public void Save()
    {
        PlayerPrefs.SetString(SAVEKEY, JsonUtility.ToJson(this));
        PlayerPrefs.Save();
        string statusJson = PlayerPrefs.GetString(SAVEKEY, JsonUtility.ToJson(new EventStatus()));
        Debug.LogError(statusJson);
    }

    public void Load()
    {
        string statusJson = PlayerPrefs.GetString(SAVEKEY, JsonUtility.ToJson(new EventStatus()));
        Instance = JsonUtility.FromJson<EventStatus>(statusJson);
    }
}
