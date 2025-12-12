using UnityEngine;

public class Fireball: Spell
{
    float spawnTime;
    float lifeTime = 5f;
    float speed = 10f;
    int damage = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawnTime = Time.time; 
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime);
       if (Time.time - spawnTime > lifeTime)
       {
           Destroy(gameObject);
       }
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponene<Enemy>().PoopNei(damage, Element.Fire);
        }
    }
}
