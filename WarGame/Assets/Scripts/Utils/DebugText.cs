using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : OnlyEditorObject, IReceiver<GameState>
{
    [SerializeField]
    private Text target;

    public void ReceiveUpdate(GameState updatedValue)
    {
        target.text = updatedValue.ToString();
    }
}
