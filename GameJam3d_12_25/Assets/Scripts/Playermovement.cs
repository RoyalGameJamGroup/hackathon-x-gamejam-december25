using Unity.VisualScripting;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float Turnspeed = 180.0f;   // rotation speed
    public float Speed = 3f;       // movement speed
    public float CollisionRadius = 2f;
    public LayerMask obstacleMask;
    public Transform moveTarget;
    public Vector3 lastLookDirection = Vector3.forward;

    public bool runForever = false;

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
        float distanceToTarget = moveDir.magnitude;

        // Normalize for movement direction
        moveDir.Normalize();
        float step;
        if (!runForever)
        {
            // Prevent overshoot:
            // if the next frame's movement is greater than the remaining distance,
            // just snap to the target instead of passing it.
            float moveStep = Speed * Time.deltaTime;

            step = Mathf.Min(moveStep, distanceToTarget);
        }else
        {
            Debug.Log("????????????????");
            step = Speed * Time.deltaTime;
            moveTarget.position += 2 * step * moveDir;
        }
        

        // Check if movement is blocked
        if (Physics.SphereCast(
            transform.position,
            CollisionRadius,
            moveDir,
            out RaycastHit hit,
            step,
            obstacleMask
        ))
        {
            // Stop right before the obstacle
            float allowedDistance = Mathf.Max(0f, hit.distance - 0.01f);
            transform.position += moveDir * allowedDistance;
            return;
        }
        else
        {
            // Free movement
            transform.position += moveDir * step;
        }
            
    }

   

   private void RotateTowardsTarget()
{
    if (moveTarget == null) return;

    Vector3 direction = moveTarget.position - transform.position;
    direction.y = 0f; // Keep rotation only on the Y-axis (horizontal plane)

    if (direction.sqrMagnitude > 0.0001f)
    {
        lastLookDirection = direction.normalized;
    }
    
    Quaternion targetRotation = Quaternion.LookRotation(lastLookDirection);

    float maxDegreesDelta = Turnspeed * Time.deltaTime; 

 
    transform.rotation = Quaternion.RotateTowards(
        transform.rotation, 
        targetRotation,     
        maxDegreesDelta     
    );
} 
}
