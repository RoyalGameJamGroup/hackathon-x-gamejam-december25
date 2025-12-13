using UnityEngine;

public class Fireball: Spell
{
    float spawnTime;
    float lifeTime = 5f;
    float speed = 5f;
    int damage = 10;

    private Material fireballMaterial;
    private readonly string directionPropertyName = "_direction";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawnTime = Time.time; 
        PlaySpawnSoundLooped();
        // set y rotation with euler angles but keep x and z rotation 

    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(-direction.x, -direction.y) * Mathf.Rad2Deg, 0);
       transform.position = transform.position + ((new Vector3(direction.x, 0, direction.y)).normalized * speed * Time.deltaTime);
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
            other.gameObject.GetComponent<Enemy>().PoopNei(damage, Element.Fire);
            PlayImpactSound();
            Destroy(gameObject);
        }
    }
}
