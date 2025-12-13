using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] public float spawnTime;
    [SerializeField] public float lifeTime = 5f;
    [SerializeField] public float speed = 2f;
    public int damage;

    public Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
       spawnTime = Time.time; 
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate((new Vector3(direction.x, 0, direction.y)).normalized * speed * Time.deltaTime, Space.World);
       if (Time.time - spawnTime > lifeTime)
       {
           Destroy(gameObject);
       }
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>()?.PoopNei(damage);
        }
        Destroy(gameObject);
    }
}

