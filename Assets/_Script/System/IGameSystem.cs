using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class IGameSystem : MonoBehaviour{
    public abstract void Init(GameManager manager);
    public abstract void OnSceneLoaded(SceneInfo sceneInfo);
    public abstract void CleanUp();
}

public class SceneInfo
{
    public string name;
}