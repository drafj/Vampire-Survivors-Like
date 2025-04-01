using System;
using UnityEngine.Events;

public interface IGameCycle
{
    public UnityEvent OnGameStarted { get; }
    public UnityEvent OnGameEnded { get; }

    event Action OnLevelUp;
    event Action OnResetLevel;
}
