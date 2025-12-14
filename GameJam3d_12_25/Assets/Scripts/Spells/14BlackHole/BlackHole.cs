using UnityEngine;

public class BlackHole: Spell
{
    float spawnTime;
    float lifeTime = 5f;
    float speed = 5f;
    int damage = 10;

    private Material fireballMaterial;
    private readonly string directionPropertyName = "_direction";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

     
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().PoopNei(damage, Element.Fire);
            PlayImpactSound();
            Destroy(gameObject);
        }
    }
}
