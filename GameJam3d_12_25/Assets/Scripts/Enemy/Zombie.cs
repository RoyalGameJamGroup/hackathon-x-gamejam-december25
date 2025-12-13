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
    [SerializeField] float stepBackDistance = 1.0f;
    [SerializeField] float pauseTime = 0.2f;

    private AttackState currentState = AttackState.Chase;
    private float stateTimer = 0f;
    private Vector3 retreatTarget;

    void Update()
    {
        if (target == null) return;

        transform.LookAt(target.transform);

        // The movement amount is consistent for both states
        float step = speed * Time.deltaTime;

        switch (currentState)
        {
            case AttackState.Chase:
                // Move towards the target at the defined 'speed'
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.transform.position,
                    step
                );
                break;

            case AttackState.SteppingBack:
                // Move TOWARDS the calculated 'retreatTarget' at the defined 'speed'
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    retreatTarget,
                    step
                );

                // Check if the zombie has reached the retreat target
                if (transform.position == retreatTarget)
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
        Debug.Log("Zombie Triggered by: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player") && currentState == AttackState.Chase)
        {
            other.gameObject.GetComponent<PlayerHealth>()?.PoopNei(damage);

            // Calculate the retreat position based on the current position and the desired distance
            Vector3 directionAway = (transform.position - other.transform.position).normalized;
            retreatTarget = transform.position + directionAway * stepBackDistance;

            currentState = AttackState.SteppingBack;
            stateTimer = 0f;
        }
    }
}