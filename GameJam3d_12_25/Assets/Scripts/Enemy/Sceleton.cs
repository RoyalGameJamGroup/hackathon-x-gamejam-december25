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
        base.Start();
        fireTimer = fireRate;
    }

    protected override void Update()
    {
        base.Update(); // Runs Knockback logic

        if (target == null) return;

        // 1. Rotation (Happens even if frozen? Usually no, let's block it if frozen)
        if (!IsStatusImpaired())
        {
            Vector3 lookAtTarget = target.transform.position;
            lookAtTarget.y = transform.position.y;
            transform.LookAt(lookAtTarget);
        }

        // 2. Logic blocked by Freeze/Knockback
        if (IsStatusImpaired()) return;

        // 3. Movement Logic
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget > minEngagementDistance)
        {
            float currentSpeed = speed * gameManager.speedMult;
            MoveWithCollisionCheck(target.transform.position, currentSpeed);
        }

        // 4. Shooting Logic
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

        Arrow arrowScript = newArrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.direction = new Vector2(shootDirection.x, shootDirection.z);
            arrowScript.damage = (int)(damage * gameManager.damageMult);
        }
    }
}