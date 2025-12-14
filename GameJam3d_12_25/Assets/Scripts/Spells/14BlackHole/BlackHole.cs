using UnityEngine;

public class BlackHole: Spell
{
    float spawnTime;
    float lifeTime = 5f;
    int damage = 10;
    float knockbackForce = 20f;
    float spellSize = 20f;
    float triggerTime = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Time.time;
        PlaySpawnsSound();
    }

   // Update is called once per frame
    void Update()
    {

        if(Time.time - spawnTime > triggerTime){
            doBlackHole();

            lifeTime -= triggerTime;
            spawnTime = Time.time;
        }

        if(lifeTime <= 0.0f){
            Destroy(gameObject);
        }
        
    }

    void  doBlackHole(){
         Collider[] hitColliders = Physics.OverlapSphere(transform.position, spellSize);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.gameObject.GetComponent<Enemy>();
                enemy.PoopNei(damage, Element.Water);

                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                enemy.KnockbackNei(-direction * knockbackForce);
            }
        }
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().PoopNei(damage, Element.Fire);
            PlayImpactSound();
            Destroy(gameObject);
        }
    }
}
