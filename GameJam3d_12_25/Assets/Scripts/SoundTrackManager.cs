using System.Collections;
using UnityEngine;

public class SoundTrackManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] tracks;

    private int lastTrackIndex = -1;

    private void Start()
    {
        if (audioSource == null || tracks == null || tracks.Length == 0)
            return;
        StartNextTrack();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
            StartNextTrack();
    }

    private void StartNextTrack()
    {
        int nextIndex = GetNextIndex();
        lastTrackIndex = nextIndex;   
        PlayTrack(nextIndex);
    }

    private int GetNextIndex()
    {
        int nextTrackIndex = Random.Range(0, tracks.Length);
        while(nextTrackIndex == lastTrackIndex)
        {
            nextTrackIndex = Random.Range(0, tracks.Length);
        }
        return nextTrackIndex;
    }

    private void PlayTrack(int index)
    {
        audioSource.clip = tracks[index];
        audioSource.Play();
    }

   
}