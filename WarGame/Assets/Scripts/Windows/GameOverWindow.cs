using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : WindowBase
{
    [SerializeField]
    private Text targetText;

    public override void Activate()
    {
        targetText.text = "Vitoria do " + GameController.instance.currentPlayer;

        base.Activate();
    }

    public void OnClick()
    {
        StateMachineController.ExecuteTransition(GameState.Settings);
    }

}
