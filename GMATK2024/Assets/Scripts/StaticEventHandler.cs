using System;

public static class StaticEventHandler
{
    public static event Action OnPlayerDied;

    public static void CallPlayerDiedEvent()
    {
        OnPlayerDied?.Invoke();
    }
}