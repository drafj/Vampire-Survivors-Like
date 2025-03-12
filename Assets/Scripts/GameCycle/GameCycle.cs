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
    private int enemiesCount = 1;
    private int maxEnemiesPerLevel = 4;
    private UnityEvent _onGameStarted = new UnityEvent();
    private UnityEvent _onGameEnded = new UnityEvent();
    private Coroutine actualGameCycleCo;
    private IPool enemyPool;
    private IPoolWithParams coinPool;
    private bool inGameplay;
    private bool firstTime = true;
    private float levelTime = 30;

    private void Awake()
    {
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<IPool>();
        coinPool = GameObject.FindGameObjectWithTag("CoinPool").GetComponent<IPoolWithParams>();
        _onGameEnded.AddListener(EndGame);
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        inGameplay = true;
        enemyPool.DespawnObjects();
        coinPool.DespawnObjects();
        _onGameStarted.Invoke();
        ActivateGameplayObjects();
        if (actualGameCycleCo != null)
            StopCoroutine(actualGameCycleCo);
        actualGameCycleCo = StartCoroutine(GameCycleCo());
    }

    private void EndGame()
    {
        inGameplay = false;
        DeactivateGameplayObjects();
        if (actualGameCycleCo != null)
            StopCoroutine(actualGameCycleCo);
        Time.timeScale = 0;
    }

    public void LevelUp()
    {
        if (enemiesCount < maxEnemiesPerLevel)
        enemiesCount++;
        enemyPool.IncreaseEnemiesLife();
        StartGame();
    }

    public void Restart()
    {
        enemiesCount = 1;
        enemyPool.ResetEnemiesLife();
        StartGame();
    }

    private void EndLevel()
    {
        Time.timeScale = 0;
        upgradeMenu.SetActive(true);
        inGameplay = false;
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
            enemyPool.SpawnObject();
        }
        player.enabled = true;
    }

    private void DeactivateGameplayObjects()
    {
        for (int i = 0; i < gameplayObjects.Count; i++)
        {
            gameplayObjects[i].SetActive(false);
        }
        enemyPool.DespawnObjects();
        coinPool.DespawnObjects();
        player.enabled = false;
    }

    private void Update()
    {
        if (!PlayerInput.actions["StartGame"].WasPressedThisFrame()) return;
        if (inGameplay || firstTime)
        {
            if (firstTime) firstTime = false;
            StartGame();
        }
    }
}
