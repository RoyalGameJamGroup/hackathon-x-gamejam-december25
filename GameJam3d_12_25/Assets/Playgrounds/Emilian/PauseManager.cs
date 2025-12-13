using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public bool isRunning;
    public event Action<bool> OnPauseStateChanged;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    public void SetGameRunningState(bool isRunningValue)
    {
        if (isRunningValue == isRunning)
        {
            return;
        }
        isRunning = isRunningValue;

        if (isRunning)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
        OnPauseStateChanged?.Invoke(isRunning);
    }
}
