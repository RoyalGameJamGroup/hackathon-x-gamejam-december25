using UnityEngine;
using System.Linq;
using System.Collections;

public class Dummy : Spell
{
    
    private GameObject[] nearestEnemies;
    [SerializeField] int damage = 20;
    [SerializeField] float distance = 5f;
    [SerializeField] int enemyAttraction = 3;
    [SerializeField] float lifetime = 5f;

    void Start()
    {
        Vector3 targetPosition = transform.position + new Vector3(direction.x, 0, direction.y).normalized * distance;
        transform.position = targetPosition;

        PlaySpawnsSound();
        nearestEnemies = FindNearestEnemies(enemyAttraction);
        for(int i = 0; i < nearestEnemies.Length; i++)
        {
            if(nearestEnemies[i] != null)
            {
                nearestEnemies[i].GetComponent<Enemy>().target = this.gameObject;
            }
        }

        StartCoroutine(kill());
    }

    IEnumerator kill()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        for(int i = 0; i < nearestEnemies.Length; i++)
        {
            if(nearestEnemies[i] != null)
            {
                nearestEnemies[i].GetComponent<Enemy>().target = GameObject.FindGameObjectWithTag("Player");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject[] FindNearestEnemies(int count)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        var nearest = enemies
            .Select(enemy => new {
                GameObject = enemy,
                Distance = Vector3.Distance(transform.position, enemy.transform.position)
            })
            .OrderBy(e => e.Distance)
            .Take(count)
            .Select(e => e.GameObject)
            .ToArray();

        return nearest;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if we are chasing and not already in an attack sequence
        
        SpawnEffect();
        PlayImpactSound();
        other.gameObject.GetComponent<Enemy>()?.PoopNei(damage, Element.Physical);
        Destroy(gameObject);
    }
}