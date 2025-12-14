using UnityEngine;

public class Freeze: Spell
{
    float spawnTime;
    [SerializeField] float speed = 12;
    [SerializeField] float length = 5f;

    [SerializeField] float spellSize = 2f;

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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spellSize);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {   
                SpawnEffect();
                hitCollider.gameObject.GetComponent<Enemy>().Freeze(length);
                hitCollider.gameObject.GetComponent<Enemy>().status = Element.Water;
                Destroy(gameObject);
            }
        }
    }
}
