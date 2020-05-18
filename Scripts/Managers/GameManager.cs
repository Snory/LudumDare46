using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }; //delegate?
public class GameManager : Singleton<GameManager>
{
    private static GameManager _instance;
    [SerializeField]
    private string _currentLevelName = string.Empty;
    private List<AsyncOperation> _loadOperation;

    [SerializeField]
    private GameObject[] _systemPrefabs;
    private List<GameObject> _instancedSystemPrefabs;
    public EventGameState OnEventGameStateChange;

    public bool LastEnemy;
    int _countOfEnemies;


    public enum GameState { PREGAME, RUNNING, PAUSED, ACTION };
    private GameState _currentGameState = GameState.PREGAME;
    public GameState CurrentGameState { get { return _currentGameState; } private set { _currentGameState = value; } }


    private void CheckInput()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartLevel(_currentLevelName);

        }
    }

    private void Update()
    {
        CheckInput();
    }


    private void Start()
    {
        OnEventGameStateChange = new EventGameState();
        InstantiateSystemPrefabs();
        _loadOperation = new List<AsyncOperation>();
        Init();

    }

    public void StartLevel(string levelName)
    {
        UnloadLevel(_currentLevelName);
        LoadLevel(levelName);
        LastEnemy = false;
        _countOfEnemies = 0;

    }

    public void AddEnemy()
    {
        _countOfEnemies += 1;
    }

    public bool WithoutEnemy()
    {
        return _countOfEnemies  == 0 ? true : false;
    }


    public void LoadLevel(string levelName)
    {

        Debug.Log($"Load level before dao: {levelName}");
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        ao.completed += OnLoadOperationCompleted;
        _loadOperation.Add(ao);
        _currentLevelName = levelName;
        Debug.Log($"Load level: {levelName}");
        Debug.Log($"Current level: {_currentLevelName}");

    }

    internal void RemoveEnemy()
    {
        _countOfEnemies -= 1;
    }

    public void UnloadLevel(string levelName)
    {

        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (_loadOperation.Contains(ao))
        {
            ao.completed += OnUnLoadOperationCompleted;
            _loadOperation.Remove(ao);
            _currentLevelName = String.Empty;
        }

    }

    private void OnUnLoadOperationCompleted(AsyncOperation obj)
    {
        UpdateState(GameState.PREGAME);
    }

    private void InstantiateSystemPrefabs()
    {
        _instancedSystemPrefabs = new List<GameObject>();
        GameObject prefabeInstance;
        for (int i = 0; i < _systemPrefabs.Length; i++)
        {
            prefabeInstance = Instantiate(_systemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabeInstance);
        }
    }

    private void OnLoadOperationCompleted(AsyncOperation obj)
    {
        UpdateState(GameState.RUNNING);
    }

    protected override void OnDestroy()
    {
        for (int i = 0; i < _instancedSystemPrefabs.Count; i++)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
        base.OnDestroy();

    }


    public void UpdateState(GameState stage)
    {
        GameState _previousGameState = _currentGameState;
        _currentGameState = stage;

        switch (_currentGameState)
        {
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            case GameState.ACTION:
                Time.timeScale = 0.2f;
                break;

            default: break;
        }

        OnEventGameStateChange.Invoke(_previousGameState, _currentGameState);
    }


    public void Init()
    {
        LoadLevel("MainMenu");
    }


    public void StartGame()
    {
        Debug.Log("LevelLoaded");
        LoadLevel("EntryLevel");
    }

}
