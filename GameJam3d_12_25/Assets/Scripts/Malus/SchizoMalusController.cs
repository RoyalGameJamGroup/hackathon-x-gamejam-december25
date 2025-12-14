using System.Collections;
using UnityEngine;

public class SchizoMalusController : MonoBehaviour
{
    [SerializeField] private AudioSource schizoAudioSource;
    [SerializeField] private AudioClip[] schizoClips;

    [SerializeField, Range(0f, 1f)]
    private float chancePerSchizoLevel = 0.1f;

    public float blockedCounterMax = 10f;

    private int schizoLevel;
    private Coroutine playbackRoutine;
    private bool isBlocked = false;
    public float counterValue;

    AudioClip currentClip;

    private void Start()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.OnPauseStateChanged += PauseManager_OnPauseStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.OnPauseStateChanged -= PauseManager_OnPauseStateChanged;
        }
    }

    public void InCreaseSchizoLevel()
    {
        schizoLevel++;
    }

    private void PauseManager_OnPauseStateChanged(bool isRunning)
    {
        if (isRunning)
        {
            Unpause();
        }
        else
        {
            OnPause();
        }
    }

    private void OnPause()
    {
       schizoAudioSource.Pause();
    }

    private void Unpause()
    {
        schizoAudioSource.UnPause();       
    }

    private void Update()
    {
        if (isBlocked)
        {
            counterValue -= Time.deltaTime;
            if (counterValue < 0)
            {
                isBlocked = false;
            }
        }


        if(!schizoAudioSource.isPlaying) 
        {
            if (isBlocked) return;
            float chance = Mathf.Clamp01(schizoLevel * chancePerSchizoLevel);

            if (Random.value <= chance)
            {
                PlayRandomClip();
            }
            else
            {
                isBlocked = true;
                counterValue = blockedCounterMax;
            }
        }
    }
    

    private void PlayRandomClip()
    {
        if (schizoClips == null || schizoClips.Length == 0)
            return;

        currentClip = schizoClips[Random.Range(0, schizoClips.Length)];
        schizoAudioSource.clip = currentClip;
        schizoAudioSource.Play();
    }
}