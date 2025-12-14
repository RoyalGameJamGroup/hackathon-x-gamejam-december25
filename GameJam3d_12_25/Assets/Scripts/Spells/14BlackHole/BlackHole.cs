using UnityEngine;

public class BlackHole: Spell
{
    float spawnTime;
    public float lifeTime = 5f;
    public int damage = 10;
    public float spellSize = 20f;
    public float triggerTime = 0.5f;

    public GameObject sparks;

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

                Vector3 direction = (hitCollider.transform.position - transform.position);
                enemy.KnockbackNei(-direction);

                Instantiate(sparks, enemy.transform.position, Quaternion.identity);
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
