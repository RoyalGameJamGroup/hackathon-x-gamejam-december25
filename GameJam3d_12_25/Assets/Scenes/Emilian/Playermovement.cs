using Unity.VisualScripting;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float Turnspeed = 5f;   // rotation speed
    public float Speed = 3f;       // movement speed
    public Transform moveTarget;

    private void Update()
    {
        Move();
        RotateTowardsTarget();
    }

    private void Move()
    {
        if (moveTarget == null) return;

        // Direction from player to target
        Vector3 moveDir = moveTarget.position - transform.position;
        float distance = moveDir.magnitude;

        // Normalize for movement direction
        moveDir.Normalize();

        // Prevent overshoot:
        // if the next frame's movement is greater than the remaining distance,
        // just snap to the target instead of passing it.
        float moveStep = Speed * Time.deltaTime;

        if (moveStep >= distance)
        {
            // Arrive exactly at the target
            transform.position = moveTarget.position;
        }
        else
        {
            // Move normally
            transform.position += moveDir * moveStep;
        }
    }

    private void RotateTowardsTarget()
    {
        if (moveTarget == null) return;

        // Direction to look at
        Vector3 direction = moveTarget.position - transform.position;

        // If direction is zero (we're exactly on the target), do nothing
        if (direction.sqrMagnitude < 0.0001f) return;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Turnspeed * Time.deltaTime
        );
    }
}
