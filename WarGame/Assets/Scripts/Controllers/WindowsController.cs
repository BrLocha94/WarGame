using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsController : MonoBehaviour, IReceiver<GameState>
{
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private WindowBase settingsWindow;
    [SerializeField]
    private WindowBase loadingWindow;
    [SerializeField]
    private WindowBase gameOverWindow;

    public void ReceiveUpdate(GameState updatedValue)
    {
        background?.SetActive(false);
        settingsWindow?.Deactivate();
        loadingWindow?.Deactivate();
        gameOverWindow?.Deactivate();

        if(updatedValue == GameState.Null)
        {
            background?.SetActive(true);
            return;
        }

        if(updatedValue == GameState.Settings)
        {
            background?.SetActive(true);
            settingsWindow?.Activate();
            return;
        }

        if(updatedValue == GameState.Initializing)
        {
            background?.SetActive(true);
            loadingWindow?.Activate();
            return;
        }

        if (updatedValue == GameState.GameOver)
        {
            background?.SetActive(true);
            gameOverWindow?.Activate();
            return;
        }
    }
}
