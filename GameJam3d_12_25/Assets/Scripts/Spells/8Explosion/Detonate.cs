using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Detonate : MonoBehaviour
{

    public int damage = 30;
    public float knockbackForce = 10f;
    public float spellSize = 15f;

    public GameObject explosion;
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
}
