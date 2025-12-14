using UnityEngine;

public class Shockwave: Spell
{
    float spawnTime;
    public float lifeTime = 5f;
    public float speed = 1f;
    public int damage = 1;
    public float knockbackForce = 10f;

    public float spellSize = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawnTime = Time.time; 

       doShockwave();
    }

    // Update is called once per frame
    void Update()
    {
       if (Time.time - spawnTime > lifeTime)
       {
           Destroy(gameObject);
       } 
    }

    void doShockwave(){

        PlaySpawnsSound();
        Renderer renderer = GetComponentInChildren<Renderer>(); 
        {
            renderer.material.SetFloat("_StartTime", Time.time);
            renderer.material.SetFloat("_Speed", 30.0f);
            renderer.material.SetFloat("_Scale", 10.0f );
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spellSize);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                SpawnEffect();
                
                Enemy enemy = hitCollider.gameObject.GetComponent<Enemy>();
                enemy.PoopNei(damage, Element.Water);

                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                enemy.KnockbackNei(direction * knockbackForce);
            }
        }
    }

}
