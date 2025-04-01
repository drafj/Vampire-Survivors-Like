using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameCycle : MonoBehaviour, IGameCycle
{
    public UnityEvent OnGameStarted { get { return _onGameStarted; } }
    public UnityEvent OnGameEnded { get { return _onGameEnded; } }
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private List<GameObject> gameplayObjects = new List<GameObject>();
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private Slider gameCycleBar;
    [SerializeField] private SpriteRenderer player;
    private int initialEnemyLife = 100;
    public int actualLevelEnemyLife = 100;
    private int enemiesCount = 1;
    private int maxEnemiesPerLevel = 40;
    private UnityEvent _onGameStarted = new UnityEvent();
    private UnityEvent _onGameEnded = new UnityEvent();
    private Coroutine actualGameCycleCo;
    private IPool _enemyPool;
    private IPool _coinPool;
    private float levelTime = 30;

    public event Action OnLevelUp;
    public event Action OnResetLevel;

    private void Awake()
    {
        _onGameEnded.AddListener(EndGame);
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        _enemyPool.DespawnObjects();
        _coinPool.DespawnObjects();
        _onGameStarted.Invoke();
        ActivateGameplayObjects();
        if (actualGameCycleCo != null)
            StopCoroutine(actualGameCycleCo);
        actualGameCycleCo = StartCoroutine(GameCycleCo());
    }

    public void SetEnemyPool(IPool enemyPool)
    {
        _enemyPool = enemyPool;
    }

    public void SetCoinsPool(IPool coinPool)
    {
        _coinPool = coinPool;
    }

    private void EndGame()
    {
        DeactivateGameplayObjects();
        if (actualGameCycleCo != null)
            StopCoroutine(actualGameCycleCo);
        Time.timeScale = 0;
    }

    public void LevelUp()
    {
        if (enemiesCount < maxEnemiesPerLevel)
        enemiesCount++;
        actualLevelEnemyLife += initialEnemyLife;
        OnLevelUp?.Invoke();
        StartGame();
    }

    public void Restart()
    {
        enemiesCount = 1;
        actualLevelEnemyLife = initialEnemyLife;
        OnResetLevel?.Invoke();
        StartGame();
    }

    private void EndLevel()
    {
        Time.timeScale = 0;
        upgradeMenu.SetActive(true);
        DeactivateGameplayObjects();
        if (actualGameCycleCo != null)
            StopCoroutine(actualGameCycleCo);

    }

    private IEnumerator GameCycleCo()
    {
        float timeLeft = levelTime;
        gameCycleBar.maxValue = levelTime;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            gameCycleBar.value = timeLeft;
            yield return null;
        }
        EndLevel();
    }

    private void ActivateGameplayObjects()
    {
        for (int i = 0; i < gameplayObjects.Count; i++)
        {
            gameplayObjects[i].SetActive(true);
        }
        for (int i = 0; i < enemiesCount; i++)
        {
            _enemyPool.SpawnObject();
        }
        player.enabled = true;
    }

    private void DeactivateGameplayObjects()
    {
        for (int i = 0; i < gameplayObjects.Count; i++)
        {
            gameplayObjects[i].SetActive(false);
        }
        _enemyPool.DespawnObjects();
        _coinPool.DespawnObjects();
        player.enabled = false;
    }

    private void Update()
    {
        if (PlayerInput.actions["EndGame"].WasPressedThisFrame())
        {
            EndLevel();
        }
    }
}
