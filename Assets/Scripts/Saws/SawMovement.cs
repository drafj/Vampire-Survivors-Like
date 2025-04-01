using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float velocity;
    private IStats _stats;
    private IGameCycle _startGame;

    private void OnEnable()
    {
        _startGame.OnGameStarted.AddListener(SetVelocity);
        SetVelocity();
    }

    private void SetVelocity()
    {
        velocity = _stats.SawsVelocity;
    }

    public void SetStatsManager(IStats stats)
    {
        _stats = stats;
    }

    public void SetGameCycle(IGameCycle gameCycle)
    {
        _startGame = gameCycle;
    }

    private void Rotate()
    {
        transform.RotateAround(player.position, transform.forward, velocity * Time.deltaTime);
        transform.Rotate(transform.forward, -velocity * Time.deltaTime);
    }

    void Update()
    {
        Rotate();
    }
}
