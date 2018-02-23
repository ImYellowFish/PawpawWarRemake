using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Subcomponents of the GameManager.
/// </summary>
public abstract class IGameSystem : MonoBehaviour{
    public abstract void Init(GameManager manager);
    public abstract void OnSceneLoaded(SceneInfo sceneInfo);
    public abstract void CleanUp();
}

/// <summary>
/// Passed to OnSceneLoaded callback
/// </summary>
public class SceneInfo
{
    public string name;
}