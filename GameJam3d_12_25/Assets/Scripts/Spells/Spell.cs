using UnityEngine;

public class Spell : MonoBehaviour
{
    public AudioClip[] spawnSounds;
    public AudioClip[] impactSounds;

    public AudioSource audioSource;

    public Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void PlaySpawnsSound(){
        int randomIndex = Random.Range(0, spawnSounds.Length);
        audioSource.PlayOneShot(spawnSounds[randomIndex]);
    }

    protected void PlayImpactSound(){
        int randomIndex = Random.Range(0, impactSounds.Length);
        audioSource.PlayOneShot(impactSounds[randomIndex]);
    }
}
