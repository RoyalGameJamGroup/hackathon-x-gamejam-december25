using UnityEngine;

public class Sceleton : Enemy
{
    [Header("Ranged Attack")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject spawnPos;
    [SerializeField] float fireRate = 3f;
    
    [Header("Movement")]
    [SerializeField] float minEngagementDistance = 5f;

    private float fireTimer;

    void Start()
    {
        fireTimer = fireRate;
    }

    void Update()
    {
        base.Update();
        if (target == null) return;

        transform.LookAt(target.transform);
        
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        float step = speed * Time.deltaTime;

        if (distanceToTarget > minEngagementDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                step
            );
        }
        else
        {
            Vector3 directionAway = transform.position - target.transform.position;
            
            Vector3 retreatPosition = transform.position + directionAway.normalized;
            
            transform.position = Vector3.MoveTowards(
                transform.position,
                retreatPosition,
                step
            );
        }

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    void Shoot()
    {
        Vector3 shootDirection = transform.forward;

        GameObject newArrow = Instantiate(arrowPrefab, spawnPos.transform.position, Quaternion.LookRotation(shootDirection));

        newArrow.GetComponent<Arrow>().direction = new Vector2(shootDirection.x, shootDirection.z);
        newArrow.GetComponent<Arrow>().damage = damage;
    }
}