using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStateChange : ButtonUiBase
{
    [SerializeField]
    private GameState targetState;

    public override void OnClick()
    {
        StateMachineController.ExecuteTransition(targetState);
    }
}
