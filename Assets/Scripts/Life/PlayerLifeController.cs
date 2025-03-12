using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] private Slider lifebar;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private GameObject gameOverPanel;
    private IDamageable lifeSystem;
    private IGameCycle gameCycle;

    private void Awake()
    {
        gameCycle = FindObjectsOfType<MonoBehaviour>().OfType<IGameCycle>().FirstOrDefault();
        lifeSystem = GetComponent<IDamageable>();
        lifeSystem.OnHitted.AddListener(SetLifebarValue);
        hpTxt.text = lifeSystem.life.ToString();
        lifeSystem.OnDeath.AddListener(GameOver);
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameCycle.OnGameEnded.Invoke();
        lifeSystem.ResetLife();
        SetLifebarValue();
    }

    private void SetLifebarValue()
    {
        lifebar.value = lifeSystem.life;
        hpTxt.text = lifeSystem.life.ToString();
    }
}
