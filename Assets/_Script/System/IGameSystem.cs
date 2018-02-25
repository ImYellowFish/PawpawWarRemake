using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ImYellowFish.Utility;

/// <summary>
/// Subcomponents of the GameManager.
/// </summary>
public class IGameSystem : SlaveComponent<GameManager>{
    /// <summary>
    /// The relating game manager
    /// </summary>
    public GameManager gameManager
    {
        get { return host; }
    }
    
    /// <summary>
    /// Called when a new scene is loaded.
    /// </summary>
    public virtual void OnSceneLoaded(SceneInfo sceneInfo)
    {

    }
}

/// <summary>
/// Passed to OnSceneLoaded callback
/// </summary>
public class SceneInfo
{
    public string name;
}