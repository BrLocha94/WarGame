using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IReceiver<GameState>
{
    [Header("External references")]
    [SerializeField]
    private Board board;

    private void Awake()
    {
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
        this.InvokeAfterFrame(() => StateMachineController.ExecuteTransition(GameState.Ready));
    }

    public void ReceiveUpdate(GameState updatedValue)
    {
        if (updatedValue == GameState.Settings)
            this.Invoke(1f, () => StateMachineController.ExecuteTransition(GameState.Initializing));
    }
}

public enum Player
{
    Null,
    Player01,
    Player02
}