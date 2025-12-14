using UnityEngine;

public class Shield: Spell
{
    int damage = 10;
    int maxHits = 10;
    int hits = 0;


    void Start()
    {
        PlaySpawnsSound();
    }
    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            hits++;
            hits++;
            SpawnEffect();
            other.gameObject.GetComponent<Enemy>().Kill();          
            PlayImpactSound();
        }
        else if(other.gameObject.CompareTag("Projectile"))
        {
            hits++;
            SpawnEffect();
            Destroy(other.gameObject);
            PlayImpactSound();
        }
        
        Renderer renderer = GetComponentInChildren<Renderer>(); 
        {
            float dissolvePercent = (float)hits / (float)maxHits;
            renderer.material.SetFloat("_Dissolve", 1.0f * dissolvePercent);
        }

        if(hits >= maxHits)
        {
            Destroy(this.gameObject);
        }
    }
}
