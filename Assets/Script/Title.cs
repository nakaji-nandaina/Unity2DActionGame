using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Title : MonoBehaviour
{
    [SerializeField]
    Button StartButton;
    private void Start()
    {
        StartButton.onClick.AddListener(ChangeScene);
    }
    private void ChangeScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
