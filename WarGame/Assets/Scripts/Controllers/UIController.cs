using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IReceiver<GameState>, IReceiver<Player>
{
    [SerializeField]
    private Board board;
    [SerializeField]
    private ButtonUiBase buttonNextTurn;
    [SerializeField]
    private DetailsScreenBase<Soldier> soldierDetails;
    [SerializeField]
    private DetailsScreenBase<Tile> targetDetails;
    [SerializeField]
    private Text currentPlayer;

    private void OnEnable()
    {
        board.onSoldierSelectedEvent += OnSoldierSelected;
        board.onTargetSelectedEvent += OnTargetSelected;
    }

    private void OnDisable()
    {
        board.onSoldierSelectedEvent -= OnSoldierSelected;
        board.onTargetSelectedEvent -= OnTargetSelected;
    }

    private void OnSoldierSelected(Soldier soldier)
    {
        if (soldier != null)
            soldierDetails.Activate(soldier);
        else
            soldierDetails.Deactivate();
    }

    private void OnTargetSelected(Tile tile)
    {
        if (tile != null)
            targetDetails.Activate(tile);
        else
            targetDetails.Deactivate();
    }

    public void ReceiveUpdate(GameState updatedValue)
    {
        if(updatedValue == GameState.Ready)
        {
            buttonNextTurn.Activate();
            return;
        }

        buttonNextTurn.Deactivate();
    }

    public void ReceiveUpdate(Player updatedValue)
    {
        currentPlayer.text = updatedValue.ToString();
    }
}
