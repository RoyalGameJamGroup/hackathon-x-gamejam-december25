using UnityEngine;

public class Banana : Spell
{
    [SerializeField] float speed = 4f;
    [SerializeField] float lifeTime = 20f;

    [Header("Attack Settings")]
    [SerializeField] float radius = 3.0f;
    [SerializeField] float rotationSpeed = 360.0f;
    [SerializeField] int damage = 10;
    
    private Transform playerTransform;
    private float currentAngle = 0f;
    private float spawnTime;

    void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); 
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            transform.position = playerTransform.position + new Vector3(radius, 0, 0); 
        }
        else
        {
            Destroy(gameObject);
        }
        spawnTime = Time.time;
    }

    void Start()
    {
        PlaySpawnsSound();
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        currentAngle += rotationSpeed * Time.deltaTime;
        
        if (currentAngle > 360)
        {
            currentAngle -= 360;
        }

        float angleRadians = currentAngle * Mathf.Deg2Rad;
        
        float x = Mathf.Cos(angleRadians) * radius;
        float z = Mathf.Sin(angleRadians) * radius;

        Vector3 newPosition = playerTransform.position + new Vector3(x, 0, z);
        transform.position = newPosition;

        if (Time.time - spawnTime > lifeTime)
        {
           Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayImpactSound();
            other.gameObject.GetComponent<Enemy>()?.PoopNei(damage, Element.Physical);
        }
    }
}