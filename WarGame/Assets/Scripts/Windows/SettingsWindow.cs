using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : WindowBase
{
    [SerializeField]
    private Board board;

    [SerializeField]
    private Slider rowsSlider;
    [SerializeField]
    private Text rowsText;
    [SerializeField]
    private Slider columnsSlider;
    [SerializeField]
    private Text columnsText;
    [SerializeField]
    private Slider soldiersSlider;
    [SerializeField]
    private Text soldiersText;

    private int rows = 8;
    private int columns = 8;
    private int soldiers = 5;

    public override void Activate()
    {
        OnRowChange();
        OnColumnChange();
        OnSoldiersChange();

        base.Activate();
    }

    public void OnRowChange()
    {
        rows = (int) rowsSlider.value;
        rowsText.text = "Rows: " + rows;
    }

    public void OnColumnChange()
    {
        columns = (int)columnsSlider.value;
        columnsText.text = "Columns: " + columns;
    }

    public void OnSoldiersChange()
    {
        soldiers = (int)soldiersSlider.value;
        soldiersText.text = "Soldiers: " + soldiers;
    }

    public void OnClick()
    {
        board.SetCustomBoardConfig(rows, columns, soldiers);
        StateMachineController.ExecuteTransition(GameState.Initializing);
    }
}
