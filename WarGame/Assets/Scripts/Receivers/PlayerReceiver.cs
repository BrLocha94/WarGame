using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public sealed class UnityPlayerEvent : UnityEvent<Player> { }

public sealed class PlayerReceiver : ReceiverBase<Player>
{
    [SerializeField]
    private UnityPlayerEvent onReceive;

    protected override void RegisterReceiver()
    {
        if(GameController.instance == null)
        {
            this.InvokeAfterFrame(() => Register());
            return;
        }

        Register();
    }

    private void Register()
    {
        GameController.instance.onCurrentPlayerUpdate += OnReceiveUpdate;

        OnReceiveUpdate(GameController.instance.currentPlayer);
    }

    protected override void UnregisterReceiver()
    {
        if (GameController.instance == null) return;

        GameController.instance.onCurrentPlayerUpdate -= OnReceiveUpdate;
    }

    protected override void OnReceiveUpdate(Player param)
    {
        onReceive?.Invoke(param);
    }
}
