using System;

public static class StaticEventHandler
{
    public static event Action OnPlayerDied;

    public static void CallPlayerDiedEvent()
    {
        OnPlayerDied?.Invoke();
    }

    public static event Action<bool> OnGamePaused;

    public static void CallGamePausedEvent(bool isGamePaused)
    {
        OnGamePaused?.Invoke(isGamePaused);
    }

    public static event Action OnGameOver;

    public static void CallGameOverEvent()
    {
        OnGameOver?.Invoke();
    }

    public static event Action OnLoadNextLevel;

    public static void CallLoadNextLevelEvent()
    {
        OnLoadNextLevel?.Invoke();
    }
}