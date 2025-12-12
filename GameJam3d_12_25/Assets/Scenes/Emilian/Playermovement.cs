using Unity.VisualScripting;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float Turnspeed = 5f;   // rotation speed
    public float Speed = 3f;       // movement speed
    public Transform moveTarget;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetTarget();
        }
        Move();
        RotateTowardsTarget();
    }

    private void SetTarget()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Mouse position in screen space
        Vector3 mouseScreenPos = Input.mousePosition;

        // Distance from camera to the movement plane (XZ plane at player's Y)
        float planeY = transform.position.y;
        float distanceToPlane = cam.transform.position.y - planeY;

        mouseScreenPos.z = distanceToPlane;

        // Convert to world position
        Vector3 worldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        // Ensure the target is exactly on the movement plane
        worldPos.y = planeY;

        // Create target if it doesn't exist
        if (moveTarget == null)
        {
            GameObject targetObj = new GameObject("MoveTarget");
            moveTarget = targetObj.transform;
        }

        moveTarget.position = worldPos;
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
