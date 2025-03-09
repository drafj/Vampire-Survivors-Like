using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float velocity;
    private IStats stats;
    private IGameCycle startGame;

    private void Awake()
    {
        SetInterfaces();
    }

    private void OnEnable()
    {
        startGame.OnGameStarted.AddListener(SetVelocity);
        SetVelocity();
    }

    private void SetVelocity()
    {
        velocity = stats.SawsVelocity;
    }

    private void SetInterfaces()
    {
        stats = FindObjectsOfType<MonoBehaviour>().OfType<IStats>().FirstOrDefault();
        startGame = FindObjectsOfType<MonoBehaviour>().OfType<IGameCycle>().FirstOrDefault();
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
