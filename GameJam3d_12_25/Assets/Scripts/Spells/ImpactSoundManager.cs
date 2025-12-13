using UnityEngine;
using System.Collections;

public class ImpactSoundManager : MonoBehaviour
{

    public AudioClip clipToPlay;

    void Start(){
        StartCoroutine(PlayAndDestroy(clipToPlay));
    }


    private IEnumerator PlayAndDestroy(AudioClip clip)
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        Destroy(gameObject);
    }
}
