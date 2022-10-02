using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReceiverBase<T> : MonoBehaviour
{
    private void OnEnable()
    {
        RegisterReceiver();
    }

    private void OnDisable()
    {
        UnregisterReceiver();
    }

    protected abstract void RegisterReceiver();
    protected abstract void UnregisterReceiver();
    protected abstract void OnReceiveUpdate(T param);
}
