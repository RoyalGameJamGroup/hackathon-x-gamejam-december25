using UnityEngine;

public class Fireball: Spell
{
    float spawnTime;
    float lifeTime = 500f;
    float speed = 0f;
    int damage = 10;

    private Material fireballMaterial;
    private readonly string directionPropertyName = "_direction";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
       spawnTime = Time.time; 
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate((new Vector3(direction.x, 0, direction.y)).normalized * speed * Time.deltaTime);
       if (Time.time - spawnTime > lifeTime)
       {
           Destroy(gameObject);
       }

       Renderer renderer = GetComponentInChildren<Renderer>(); 
        {
            // NOTE: Use .material to get an instance copy, so you don't affect other fireballs
            fireballMaterial = renderer.material; 
            fireballMaterial.SetVector(directionPropertyName, direction);
        }
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<Enemy>().PoopNei(damage, Element.Fire);
        }
    }
}
