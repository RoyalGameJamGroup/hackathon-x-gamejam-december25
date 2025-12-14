using UnityEngine;

public class Spell : MonoBehaviour
{
    public AudioClip[] spawnSounds;
    public AudioClip[] impactSounds;

    public AudioSource audioSource;
    public GameObject impactAudioGO;

    public GameObject effectPrefab;

    public Vector2 direction;
    public GameObject lastSpell;
    

    protected void PlaySpawnsSound(){
        int randomIndex = Random.Range(0, spawnSounds.Length);
        audioSource.PlayOneShot(spawnSounds[randomIndex]);
    }

    protected void PlaySpawnSoundLooped(){
        int randomIndex = Random.Range(0, spawnSounds.Length);
        audioSource.clip = spawnSounds[randomIndex];
        audioSource.loop = true;
        audioSource.Play();
    }

    protected void PlayImpactSound(){
        int randomIndex = Random.Range(0, impactSounds.Length);
        
        impactAudioGO.GetComponent<ImpactSoundManager>().clipToPlay = impactSounds[randomIndex];
        Instantiate(impactAudioGO);
    }

    public void SpawnEffect(){
        if(effectPrefab != null){
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }

}
