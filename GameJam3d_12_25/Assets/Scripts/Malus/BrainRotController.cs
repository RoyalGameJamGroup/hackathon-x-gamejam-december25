using System.Collections.Generic;
using UnityEngine;

public class BrainrotController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> videoPlayerObjects = new();

    // Stores the objects that were active before deactivation
    private readonly List<GameObject> previouslyActive = new();

    private void Start()
    {
        if(PauseManager.Instance!= null)
        {
            PauseManager.Instance.OnPauseStateChanged += PauseManager_OnPauseStateChanged;
        }
    }

    private void OnDestroy()
    {
        PauseManager.Instance.OnPauseStateChanged -= PauseManager_OnPauseStateChanged;
    }

    private void PauseManager_OnPauseStateChanged(bool isRunning)
    {
        if (isRunning)
        {
            RestorePreviousState();
        }
        else
        {
            DeactivateAllAndStoreState();
        }
    }


    /// <summary>
    /// Activates one random inactive VideoPlayer GameObject.
    /// If all are active, nothing happens.
    /// </summary>
    public void ActivateRandomVideoPlayer()
    {
        List<GameObject> inactivePlayers = new();

        foreach (var go in videoPlayerObjects)
        {
            if (go != null && !go.activeSelf)
            {
                inactivePlayers.Add(go);
            }
        }

        if (inactivePlayers.Count == 0)
        {
            // All players are already active
            return;
        }

        int index = Random.Range(0, inactivePlayers.Count);
        inactivePlayers[index].SetActive(true);
    }

    /// <summary>
    /// Deactivates all VideoPlayer GameObjects
    /// and stores which ones were active.
    /// </summary>
    public void DeactivateAllAndStoreState()
    {
        previouslyActive.Clear();

        foreach (var go in videoPlayerObjects)
        {
            if (go == null)
                continue;

            if (go.activeSelf)
            {
                previouslyActive.Add(go);
                go.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Restores the activation state saved by DeactivateAllAndStoreState.
    /// </summary>
    public void RestorePreviousState()
    {
        foreach (var go in previouslyActive)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
        }

        previouslyActive.Clear();
    }
}
