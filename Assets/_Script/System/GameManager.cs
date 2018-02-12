using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ImYellowFish.Utility;

public class GameManager : SingletonBehaviour<GameManager> {
    
    protected override void Awake()
    {
        base.Awake();

        // create all game systems
        CreateAllGameSystems();

        // register sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void CreateAllGameSystems()
    {

    }

    private List<IGameSystem> gameSystems = new List<IGameSystem>();
    public IGameSystem CreateGameSystem(string name)
    {
        var prefab = Resources.Load<GameObject>(name);
        if (prefab == null)
            Debug.LogError("Cannot find prefab for " + name);

        var go = Instantiate(prefab);
        var sys = go.GetComponent<IGameSystem>();
        sys.Init(this);
        gameSystems.Add(sys);
        return sys;
    }

    private bool firstLoad = true;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        if (firstLoad)
        {
            firstLoad = false;
            return;
        }

        Debug.Log("Invoke OnSceneLoaded.");

        var sceneInfo = new SceneInfo() { name = scene.name };
        foreach (var sys in gameSystems)
        {
            sys.OnSceneLoaded(sceneInfo);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        foreach (var sys in gameSystems)
        {
            sys.CleanUp();
        }
    }
}
