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
       PlaySpawnsSound();
    }

    // Update is called once per frame
    void Update()
    {
       transform.position += (new Vector3(direction.x, 0, direction.y)).normalized * speed * Time.deltaTime;
       transform.Rotate(new Vector3(0,1,0), 90 * Time.deltaTime *speed * 1.5f);

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
