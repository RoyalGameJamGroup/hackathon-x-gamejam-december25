using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Detonate : MonoBehaviour
{

    [Header("Detonation")]
    public int damage = 30;
    public float knockbackForce = 10f;
    public float spellSize = 15f;

    public GameObject explosion;

    [Header("Duplication")]
    public float dupMaxDistance = 10.0f;
    public GameObject dupEffect;

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spellSize);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.gameObject.GetComponent<Enemy>();
                float distance = (enemy.gameObject.transform.position - transform.position).magnitude;
                float falloff_multiplier = Mathf.Min(1.0f / (distance * distance), 1.0f);
                enemy.PoopNei((int)(falloff_multiplier * damage), Element.Fire);

                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                enemy.KnockbackNei(direction * knockbackForce * falloff_multiplier);
            }
        }

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Duplicate()
    {
        Vector2 random2D = Random.insideUnitCircle * dupMaxDistance;
        Vector3 point = new Vector3(transform.position.x + random2D.x, transform.position.y, transform.position.z + random2D.y);
        Quaternion randomRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        var castedSpell = Instantiate(gameObject, point, randomRot);
        castedSpell.GetComponent<Spell>().direction = castedSpell.transform.forward;

        // Spawn dupe effect
        if (dupEffect != null)
            Instantiate(dupEffect, point, Quaternion.identity);

    }
}
