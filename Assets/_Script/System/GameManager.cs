using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ImYellowFish.Utility;

/// <summary>
/// The main manager of the game.
/// Manages all game systems including battlemanager, soundmanager, etc.
/// 
/// All the managers can be accessed here.
/// All the managers will be created when game starts, and wont be destroyed when scene changes.
/// </summary>
public class GameManager : SingletonBehaviour<GameManager> {
    // systems
    public BattleManager battleManager;

    // Initialize all the systems here.
    private void CreateAllGameSystems()
    {
        battleManager = CreateGameSystem("BattleManager") as BattleManager;
    }

    protected override void Awake()
    {
        base.Awake();

        // create all game systems
        CreateAllGameSystems();

        // register sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private List<IGameSystem> gameSystems = new List<IGameSystem>();
    /// <summary>
    /// Create a gameSystem from prefab path.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IGameSystem CreateGameSystem(string name)
    {
        string path = "Test/System/" + name;
        var prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
            Debug.LogError("Cannot find prefab for " + path);

        var go = Instantiate(prefab);
        go.transform.SetParent(transform);

        var sys = go.GetComponent<IGameSystem>();
        sys.Init(this);
        gameSystems.Add(sys);
        return sys;
    }

    private bool firstLoad = true;
    /// <summary>
    /// Inform all the gamesystems when new scene is loaded.
    /// </summary>
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
