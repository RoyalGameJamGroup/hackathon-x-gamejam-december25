using UnityEngine;

public class Freeze: Spell
{
    float spawnTime;
    float lifeTime = 500f;
    float speed = 1f;
    int damage = 5;

    float spellSize = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawnTime = Time.time; 
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate((new Vector3(direction.x, 0, direction.y)).normalized * speed * Time.deltaTime);
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // do a sphere cast to find all enemies in range
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, spellSize);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Enemy"))
                {
                    hitCollider.gameObject.GetComponent<Enemy>().PoopNei(damage, Element.Water);
                }
            }
            Destroy(gameObject);
        }
    }
}
