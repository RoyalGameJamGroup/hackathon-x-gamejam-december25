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

    void Update()
    {
        base.Update(); 

        if (target == null) return;

        Vector3 lookAtTarget = target.transform.position;
        lookAtTarget.y = transform.position.y; // keep only horizontal rotation
        transform.LookAt(target.transform);

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        switch (currentState)
        {
            case AttackState.Chase:
                step = speed * Time.deltaTime;
                
                if(distanceToTarget < stepBackRadius)
                {
                    moveDir = (transform.position - target.transform.position).normalized;
                    retreatTarget = transform.position + moveDir * stepBackDistance;

                    currentState = AttackState.SteppingBack;
                    stateTimer = 0f;
                }else{
                    moveDir = (target.transform.position - transform.position).normalized;
                    if(!movementBlocked){
                        transform.position = Vector3.MoveTowards(
                            transform.position,
                            target.transform.position,
                            step
                        );
                    }
                }
                break;

            case AttackState.SteppingBack:
                step = stepBackSpeed * Time.deltaTime;
                if(!movementBlocked){
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        retreatTarget,
                        step
                    );
                }
                
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
        // Only trigger if we are chasing and not already in an attack sequence
        if (other.gameObject.CompareTag("Player"))
        {
            
            other.gameObject.GetComponent<PlayerHealth>()?.PoopNei(damage);
        }
    }
}