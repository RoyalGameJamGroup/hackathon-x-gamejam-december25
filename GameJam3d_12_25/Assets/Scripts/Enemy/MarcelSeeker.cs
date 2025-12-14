using UnityEngine;

public class MarcelSeeker : Enemy
{
    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update(); // Runs Knockback logic

        // 1. If we are frozen or flying backward, don't run AI
        if (target == null || IsStatusImpaired()) return;

        // 2. Rotation
        Vector3 lookAtTarget = target.transform.position;
        lookAtTarget.y = transform.position.y;
        transform.LookAt(lookAtTarget);

        // 3. Movement using Base Class Helper (Handles Collisions)
        float currentSpeed = speed * gameManager.speedMult;
        MoveWithCollisionCheck(target.transform.position, currentSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>()?.PoopNei((int)(damage * gameManager.damageMult));
            Destroy(gameObject);
        }
    }
}