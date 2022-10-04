using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateMachineController
{
    public delegate void OnGameStateChangeHandler(GameState gameState);
    public static event OnGameStateChangeHandler onGameStateChangeEvent;

    private static GameState currentGameState = GameState.Null;

    public static GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }
        private set
        {
            currentGameState = value;
            onGameStateChangeEvent?.Invoke(currentGameState);
        }
    }

    private static bool CheckCurrentState(GameState gameState) => currentGameState == gameState;

    public static void InitializeStateMachine()
    {
        CurrentGameState = GameState.Null;
    }

    public static void ExecuteTransition(GameState nextState)
    {
        if (!CheckTransition(nextState))
        {
            Debug.Log("Unauthorized transition: " + currentGameState + " to " + nextState);
            return;
        }


        CurrentGameState = nextState;
    }

    private static bool CheckTransition(GameState nextState)
    {
        switch (currentGameState)
        {
            case GameState.Null:
                if (nextState == GameState.Settings) return true;
                break;
            case GameState.Settings:
                if (nextState == GameState.Initializing) return true;
                break;
            case GameState.Initializing:
                if (nextState == GameState.Ready) return true;
                break;
            case GameState.Ready:
                if (nextState == GameState.SelectedSoldier) return true;
                if (nextState == GameState.NextPlayer) return true;
                break;
            case GameState.SelectedSoldier:
                if (nextState == GameState.Ready) return true;
                if (nextState == GameState.Moving) return true;
                if (nextState == GameState.Attacking) return true;
                if (nextState == GameState.NextPlayer) return true;
                break;
            case GameState.Moving:
                if (nextState == GameState.Ready) return true;
                break;
            case GameState.Attacking:
                if (nextState == GameState.CheckEndTurn) return true;
                break;
            case GameState.CheckEndTurn:
                if (nextState == GameState.Ready) return true;
                if (nextState == GameState.GameOver) return true;
                break;
            case GameState.NextPlayer:
                if (nextState == GameState.Ready) return true;
                break;
            case GameState.GameOver:
                if (nextState == GameState.Settings) return true;
                break;
        }

        return false;
    }
}

public enum GameState
{
    Null,
    Settings,
    Initializing,
    Ready,
    SelectedSoldier,
    Moving,
    Attacking,
    CheckEndTurn,
    NextPlayer,
    GameOver
}