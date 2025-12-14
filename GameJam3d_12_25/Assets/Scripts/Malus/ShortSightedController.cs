using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShortSightedController : MonoBehaviour
{
    [SerializeField] private Volume volume;

    private float previousVignetteInstensityValue;

    private void Start()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.OnPauseStateChanged += PauseManager_OnPauseStateChanged;
        }
    }

    private void OnDestroy()
    {
        PauseManager.Instance.OnPauseStateChanged -= PauseManager_OnPauseStateChanged;
    }

    public void AddShortSightedNess()
    {
        VolumeProfile volumeProfile = volume.profile;
        Vignette vignette;
        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        vignette.intensity.value += .03f;
    }

    private void PauseManager_OnPauseStateChanged(bool isRunning)
    {
        if (isRunning)
        {
            RestorePreviousState();
        }
        else
        {
            SaveState();
        }
    }

    private void SaveState()
    {
        VolumeProfile volumeProfile = volume.profile;
        Vignette vignette;
        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        previousVignetteInstensityValue = vignette.intensity.value;
        vignette.intensity.value = 0;
    }

    private void RestorePreviousState()
    {
        VolumeProfile volumeProfile = volume.profile;
        Vignette vignette;
        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        vignette.intensity.value = previousVignetteInstensityValue;
    }
}
