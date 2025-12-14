using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Spell
{
    [Header("Settings")]
    public float range = 10.0f;
    public int maxBounces = 10;
    public int baseDamage = 40;
    public float bounceDelay = 0.1f;
    public float bounceDamageMultiplier = 0.85f;

    [Header("Visuals")]
    public GameObject strikeEffect;
    public GameObject lightningLinePrefab;

    private List<GameObject> alreadyHit = new List<GameObject>();

    void Start()
    {
        StartCoroutine(ProcessChain());
    }

    IEnumerator ProcessChain()
    {
        Vector3 currentSearchPos = transform.position;

        for (int i = 0; i < maxBounces; i++)
        {
            Enemy nextTarget = FindBestTarget(currentSearchPos);

            if (nextTarget == null) break;

            alreadyHit.Add(nextTarget.gameObject);

            // 1. Capture the position NOW (before the enemy potentially dies)
            Vector3 hitPosition = nextTarget.transform.position;

            // 2. Spawn Visual Line
            if (lightningLinePrefab != null)
            {
                GameObject lineObj = Instantiate(lightningLinePrefab, Vector3.zero, Quaternion.identity);
                LightningLine lineScript = lineObj.GetComponent<LightningLine>();
                lineScript.DrawLine(currentSearchPos, hitPosition);
            }

            // 3. Update search position for next loop
            currentSearchPos = hitPosition;

            // 4. Calculate & Apply Damage
            int calculatedDamage = Mathf.RoundToInt(baseDamage * Mathf.Pow(bounceDamageMultiplier, i));

            if (nextTarget.status == Element.Water)
            {
                nextTarget.PoopNei(calculatedDamage, Element.Lightning);
            }

            // 5. Spawn Strike Effect using the SAVED POSITION, not the enemy reference
            StartCoroutine(SpawnStrikeEffect(bounceDelay, hitPosition));

            yield return new WaitForSeconds(bounceDelay);
        }

        Destroy(gameObject, 2.0f);
    }

    Enemy FindBestTarget(Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, range);
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            GameObject obj = hitCollider.gameObject;
            if (obj.CompareTag("Enemy") && !alreadyHit.Contains(obj))
            {
                Enemy enemyScript = obj.GetComponent<Enemy>();
                if (enemyScript != null && enemyScript.status == Element.Water)
                {
                    float dist = Vector3.Distance(center, obj.transform.position);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        closestEnemy = enemyScript;
                    }
                }
            }
        }
        return closestEnemy;
    }

    // CHANGED: Pass Vector3 pos instead of Enemy enemy
    IEnumerator SpawnStrikeEffect(float delay, Vector3 pos)
    {
        yield return new WaitForSeconds(delay);

        // We don't check if enemy != null anymore, because we have the position hardcoded
        if (strikeEffect != null)
        {
            Instantiate(strikeEffect, pos, Quaternion.identity);
        }
    }
}