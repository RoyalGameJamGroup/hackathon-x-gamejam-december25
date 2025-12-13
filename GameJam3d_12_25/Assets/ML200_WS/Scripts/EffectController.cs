using System.Collections;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public float pitchVariance = 0.1f;
    public bool destroyAfterClip = true;
    public float manualLifetime = 2.0f;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        float randomPitch = 1.0f + Random.Range(-pitchVariance, pitchVariance);
        _audioSource.pitch = randomPitch;

        _audioSource.Play();
        float waitTime;

        if (destroyAfterClip && _audioSource.clip != null)
        {
            waitTime = _audioSource.clip.length / _audioSource.pitch;
        }
        else
        {
            waitTime = manualLifetime;
        }

        StartCoroutine(KillObjectRoutine(waitTime));
    }

    IEnumerator KillObjectRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
