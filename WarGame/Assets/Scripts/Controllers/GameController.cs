using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>, IReceiver<GameState>
{
    public Action<Player> onCurrentPlayerUpdate;

    [Header("External references")]
    [SerializeField]
    private Board board;

    public Player currentPlayer { get; private set; } = Player.Null;

    protected override void ExecuteOnAwake()
    {
        base.ExecuteOnAwake();

        StateMachineController.InitializeStateMachine();
    }

    private void OnEnable()
    {
        board.onBoardCreatedEvent += OnBoardCreated;
    }

    private void OnDisable()
    {
        board.onBoardCreatedEvent -= OnBoardCreated;
    }

    private void Start()
    {
        this.InvokeAfterFrame(() => StateMachineController.ExecuteTransition(GameState.Settings));
    }
    private void OnBoardCreated()
    {
        currentPlayer = Player.Player01;
        onCurrentPlayerUpdate?.Invoke(currentPlayer);

        this.InvokeAfterFrame(() => StateMachineController.ExecuteTransition(GameState.Ready));
    }

    public void ReceiveUpdate(GameState updatedValue)
    {
        if (updatedValue == GameState.Settings)
        {
            this.Invoke(1f, () => StateMachineController.ExecuteTransition(GameState.Initializing));
            return;
        }

        if (updatedValue == GameState.NextPlayer)
        {
            if (currentPlayer == Player.Player01)
                currentPlayer = Player.Player02;
            else
                currentPlayer = Player.Player01;

            onCurrentPlayerUpdate?.Invoke(currentPlayer);
            this.Invoke(1f, () => StateMachineController.ExecuteTransition(GameState.Ready));
            return;
        }
    }
}

public enum Player
{
    Null,
    Player01,
    Player02
}