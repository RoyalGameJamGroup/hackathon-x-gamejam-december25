using UnityEngine;

public class Shockwave: Spell
{
    float spawnTime;
    float lifeTime = 5f;
    float speed = 1f;
    int damage = 1;
    float knockbackForce = 10f;

    float spellSize = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawnTime = Time.time; 

       doShockwave();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void doShockwave(){
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
                Enemy enemy = hitCollider.gameObject.GetComponent<Enemy>();
                enemy.PoopNei(damage, Element.Water);

                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                enemy.KnockbackNei(direction * knockbackForce);
            }
        }
    }

}
