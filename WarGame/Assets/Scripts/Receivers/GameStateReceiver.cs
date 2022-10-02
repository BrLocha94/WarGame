using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public sealed class UnityGameStateEvent : UnityEvent<GameState> { }

public sealed class GameStateReceiver : ReceiverBase<GameState>
{
    [SerializeField]
    private UnityGameStateEvent onReceive;

    protected override void RegisterReceiver()
    {
        StateMachineController.onGameStateChangeEvent += OnReceiveUpdate;

        OnReceiveUpdate(StateMachineController.CurrentGameState);
    }

    protected override void UnregisterReceiver()
    {
        StateMachineController.onGameStateChangeEvent -= OnReceiveUpdate;
    }

    protected override void OnReceiveUpdate(GameState param)
    {
        onReceive?.Invoke(param);
    }
}
