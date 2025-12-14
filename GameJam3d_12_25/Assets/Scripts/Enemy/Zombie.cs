using UnityEngine;

public class Zombie : Enemy
{
    private enum AttackState
    {
        Chase,
        SteppingBack,
        Pausing
    }

    [Header("Attack Settings")]
    [SerializeField] float stepBackDistance = 2.0f;
    [SerializeField] float stepBackSpeed = 3.0f;
    [SerializeField] float pauseTime = 0.2f;
    [SerializeField] float stepBackRadius = 1.0f;

    private AttackState currentState = AttackState.Chase;
    private float stateTimer = 0f;
    private Vector3 retreatTarget;

    private void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update(); // Runs Knockback logic

        // 1. Stop AI if impaired
        if (target == null || IsStatusImpaired()) return;

        // 2. Rotation
        Vector3 lookAtTarget = target.transform.position;
        lookAtTarget.y = transform.position.y;
        transform.LookAt(lookAtTarget);

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        switch (currentState)
        {
            case AttackState.Chase:
                if (distanceToTarget < stepBackRadius)
                {
                    // Calculate retreat position
                    Vector3 directionAway = (transform.position - target.transform.position).normalized;
                    retreatTarget = transform.position + directionAway * stepBackDistance;

                    currentState = AttackState.SteppingBack;
                    stateTimer = 0f;
                }
                else
                {
                    // Chase the player using collision helper
                    float currentSpeed = speed * gameManager.speedMult;
                    MoveWithCollisionCheck(target.transform.position, currentSpeed);
                }
                break;

            case AttackState.SteppingBack:
                // Move towards retreat point
                MoveWithCollisionCheck(retreatTarget, stepBackSpeed);

                // Check if reached (allow small margin of error)
                if (Vector3.Distance(transform.position, retreatTarget) < 0.1f)
                {
                    currentState = AttackState.Pausing;
                    stateTimer = 0f;
                }
                break;

            case AttackState.Pausing:
                stateTimer += Time.deltaTime;
                if (stateTimer >= pauseTime)
                {
                    currentState = AttackState.Chase;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger damage if we are chasing
        if (other.gameObject.CompareTag("Player") && currentState == AttackState.Chase)
        {
            other.gameObject.GetComponent<PlayerHealth>()?.PoopNei((int)(damage * gameManager.damageMult));
        }
    }
}