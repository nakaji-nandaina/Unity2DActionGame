using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class EventStatus
{
    // JSONファイルのパス
    private static readonly string SaveFilePath = Path.Combine(Application.persistentDataPath, "eventstatus.json");

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

    public EventStatus(List<bool> _startEventFlag, List<bool> _finishedEventFlag)
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
            if (File.Exists(SaveFilePath))
            {
                string statusJson = File.ReadAllText(SaveFilePath);
                Instance = JsonUtility.FromJson<EventStatus>(statusJson);
            }
            else
            {
                Instance = new EventStatus();
            }
        }
        return Instance;
    }

    public void Save()
    {
        string statusJson = JsonUtility.ToJson(this, true);
        File.WriteAllText(SaveFilePath, statusJson);
        Debug.LogError("データを保存しました: " + SaveFilePath + "\n" + statusJson);
    }

    public void Load()
    {
        if (File.Exists(SaveFilePath))
        {
            string statusJson = File.ReadAllText(SaveFilePath);
            Instance = JsonUtility.FromJson<EventStatus>(statusJson);
            Debug.LogError("データを読み込みました: " + SaveFilePath + "\n" + statusJson);
        }
        else
        {
            Instance = new EventStatus();
            Debug.LogError("セーブファイルが見つかりませんでした: " + SaveFilePath);
        }
    }

    public void Delete()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
            Instance = new EventStatus(); // データを初期化
            Debug.LogError("保存データを削除しました: " + SaveFilePath);
        }
        else
        {
            Debug.LogError("削除するファイルが見つかりません: " + SaveFilePath);
        }
    }
}
