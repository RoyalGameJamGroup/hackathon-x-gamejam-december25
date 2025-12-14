using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] public GameObject target;
    [SerializeField] public float speed = 2.0f;
    [SerializeField] protected int damage = 10;

    // IMPORTANT: Make sure this LayerMask includes your Walls, but EXCLUDES the Floor!
    // If it includes the floor, set your 'CollisionRadius' smaller or lift the cast origin higher.
    [SerializeField] public LayerMask obstacleMask;
    [SerializeField] public float CollisionRadius = 0.5f;

    [Header("Status")]
    public int health = 50;
    [HideInInspector] public int maxhealth;
    public Element status;
    [SerializeField] protected int scoreValue = 10;

    [Header("UI")]
    public GameObject healthBar;
    public float barOffset = 1.0f;

    [Header("References")]
    public GameManager gameManager;

    // Movement & Physics internals
    private Vector3 knockbackForce;
    private float knockBackTimer = 0f;

    // Status flags
    protected bool frozen = false;
    protected bool isBeingKnockedBack = false;

    protected virtual void Start()
    {
        if (gameManager != null)
        {
            maxhealth = (int)(health * gameManager.healthMult);
        }
        else
        {
            maxhealth = health;
            Debug.LogWarning("GameManager reference missing on Enemy.");
        }

        GameObject healthBars = GameObject.FindWithTag("HealthBars");
        if (healthBars != null && healthBar != null)
        {
            var bar = Instantiate(healthBar, healthBars.transform);
            bar.GetComponent<HealthBar>().SetMonitor(this, transform, barOffset);
        }
    }

    protected virtual void Update()
    {
        if (Time.time < knockBackTimer)
        {
            isBeingKnockedBack = true;
            ApplyKnockbackPhysics();
        }
        else
        {
            isBeingKnockedBack = false;
        }
    }

    protected bool IsStatusImpaired()
    {
        return frozen || isBeingKnockedBack;
    }

    protected void MoveWithCollisionCheck(Vector3 targetPosition, float currentSpeed)
    {
        float step = currentSpeed * Time.deltaTime;
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < 0.01f) return;

        float actualStep = Mathf.Min(step, distance);

        // Lift origin up slightly to avoid floor friction logic
        Vector3 origin = transform.position + Vector3.up * CollisionRadius;

        if (!Physics.SphereCast(origin, CollisionRadius * 0.9f, direction, out RaycastHit hit, actualStep, obstacleMask))
        {
            transform.position += direction * actualStep;
        }
    }

    private void ApplyKnockbackPhysics()
    {
        // 1. Force horizontal movement only (prevent being knocked into the sky/floor)
        knockbackForce.y = 0;

        float deltaStep = Time.deltaTime;
        Vector3 proposedMove = knockbackForce * deltaStep;
        float distance = proposedMove.magnitude;
        Vector3 direction = proposedMove.normalized;

        if (distance < 0.001f) return;

        // 2. CRITICAL FIX: Lift the cast origin. 
        // If pivot is at feet (0,0,0), SphereCast hits the floor. We lift it up.
        Vector3 origin = transform.position + Vector3.up * CollisionRadius;

        // 3. HYBRID CHECK: Raycast (Center) + SphereCast (Shoulders)
        // Use a slightly smaller radius (0.9f) for the sphere to prevent getting snagged on corners
        bool hitWall = false;

        // A. Center Raycast (Precise)
        if (Physics.Raycast(origin, direction, distance, obstacleMask))
        {
            hitWall = true;
        }
        // B. SphereCast (Wide)
        else if (Physics.SphereCast(origin, CollisionRadius * 0.9f, direction, out RaycastHit hit, distance, obstacleMask))
        {
            hitWall = true;
        }

        if (hitWall)
        {
            // Hit a wall - Stop immediately
            knockBackTimer = 0f;
        }
        else
        {
            // Clear - Move
            transform.position += proposedMove;
        }
    }

    public void PoopNei(int damage, Element el)
    {
        health -= damage;
        status = el;
        if (health <= 0)
        {
            gameManager?.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    public void Freeze(float length)
    {
        StartCoroutine(FreezeRoutine(length));
    }

    IEnumerator FreezeRoutine(float length)
    {
        frozen = true;
        Debug.Log("Frozen for " + length);
        yield return new WaitForSeconds(length);
        frozen = false;
    }

    public void KnockbackNei(Vector3 force)
    {
        knockBackTimer = Time.time + 0.3f;
        knockbackForce = force;
    }

    // ---------------------------------------------------------
    // DEBUGGING VISUALS
    // ---------------------------------------------------------
    private void OnDrawGizmosSelected()
    {
        // Draw the collision sphere in the Editor so you can adjust the Radius
        Gizmos.color = Color.yellow;
        Vector3 origin = transform.position + Vector3.up * CollisionRadius;
        Gizmos.DrawWireSphere(origin, CollisionRadius);

        // Draw Knockback line if active
        if (isBeingKnockedBack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + knockbackForce.normalized * 2f);
        }
    }
}