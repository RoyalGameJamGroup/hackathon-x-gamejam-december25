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

        Vector3 lookAtTarget = target.transform.position;
        lookAtTarget.y = transform.position.y; // keep only horizontal rotation
        transform.LookAt(target.transform);
        
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        step = speed * Time.deltaTime;
        moveDir = (target.transform.position - transform.position).normalized;

        if (distanceToTarget > minEngagementDistance)
        {
            if(!movementBlocked){
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.transform.position,
                    step
                );
            }
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