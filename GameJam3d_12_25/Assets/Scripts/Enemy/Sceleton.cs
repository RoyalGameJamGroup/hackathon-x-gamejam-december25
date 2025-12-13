using UnityEngine;

public class Sceleton : Enemy
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject spawnPos;
    [SerializeField] float fireRate = 3f;
    [SerializeField] float arrowSpeed = 10f;

    private float fireTimer;

    void Start()
    {
        fireTimer = fireRate;
    }

    void Update()
    {
        if (target == null) return;

        transform.LookAt(target.transform);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    void Shoot()
    {
        GameObject newArrow = Instantiate(arrowPrefab, spawnPos.transform.position, spawnPos.transform.rotation);

        Vector3 shootDirection = transform.forward;

        Rigidbody arrowRb = newArrow.GetComponent<Rigidbody>();

        if (arrowRb != null)
        {
            arrowRb.linearVelocity = shootDirection * arrowSpeed;
        }
        else
        {
            Debug.LogError("Arrow prefab is missing a Rigidbody component!");
        }
    }
}