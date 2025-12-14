using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FilterMalusController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private float previousFilterInstensityValue;


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

    public void IncreaseFilters()
    {
        VolumeProfile volumeProfile = volume.profile;
        LensDistortion lensDistortion;
        ChromaticAberration chromaticAberration;
        if (!volume.profile.TryGet(out lensDistortion))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        if (!volume.profile.TryGet(out chromaticAberration))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        lensDistortion.intensity.value += .2f;
        chromaticAberration.intensity.value += .2f;

    }


    private void PauseManager_OnPauseStateChanged(bool isRunning)
    {
        if (isRunning)
        {
            RestorePreviousValue();
        }
        else
        {
            OnPause();
        }
    }

    private void RestorePreviousValue()
    {
        VolumeProfile volumeProfile = volume.profile;
        LensDistortion lensDistortion;
        ChromaticAberration chromaticAberration;
        if (!volume.profile.TryGet(out lensDistortion))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        if (!volume.profile.TryGet(out chromaticAberration))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        lensDistortion.intensity.value = previousFilterInstensityValue;
        chromaticAberration.intensity.value = previousFilterInstensityValue;
    }

    private void OnPause()
    {
        VolumeProfile volumeProfile = volume.profile;
        LensDistortion lensDistortion;
        ChromaticAberration chromaticAberration;
        if (!volume.profile.TryGet(out lensDistortion))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        if (!volume.profile.TryGet(out chromaticAberration))
        {
            Debug.LogError("Vignette override not found in Volume Profile.");
            return;
        }
        previousFilterInstensityValue = lensDistortion.intensity.value;
        lensDistortion.intensity.value = 0;
        chromaticAberration.intensity.value = 0;

    }


}
