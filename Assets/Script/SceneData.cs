using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SceneData", menuName = "CreateSceneData")]

public class SceneData : ScriptableObject
{
    public string SceneName;
    public float SceneBrightness;

}
