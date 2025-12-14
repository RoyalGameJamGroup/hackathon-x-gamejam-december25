using System.Collections;
using UnityEngine;

public class Monke : Spell
{
    private enum AttackState
    {
        Chase,
        SteppingBack,
        Pausing
    }
    private AttackState currentState = AttackState.Chase;
    
    [SerializeField] float speed = 4f;
    [SerializeField] float lifeTime = 20f;

    [Header("Attack Settings")]
    [SerializeField] float stepBackDistance = 1.0f;
    [SerializeField] float stepBackSpeed = 4.0f;
    [SerializeField] float stepBackRadius = 2.0f;
    [SerializeField] int damage = 5;
    

    private Transform nearestEnemy;
    public bool doppelAgent = false;
    private Vector3 retreatTarget = Vector3.zero;
    private float spawnTime;

    void Start()
    {
        PlaySpawnsSound();
        StartCoroutine("timerKill");
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Find the nearest enemy
        FindNearestEnemy();
        float step;
        Vector3 moveDir;
        // 2. Move towards the nearest enemy if one is found
        if (nearestEnemy != null)
        {
            Vector3 lookAtTarget = nearestEnemy.transform.position;
            lookAtTarget.y = transform.position.y; // keep only horizontal rotation
            transform.LookAt(nearestEnemy.transform);
            
            float distanceToTarget = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            switch (currentState)   
            {
                case AttackState.Chase:
                    step = speed * Time.deltaTime;
                    
                    if(distanceToTarget < stepBackRadius)
                    {
                        moveDir = (transform.position - nearestEnemy.transform.position).normalized;
                        retreatTarget = transform.position + moveDir * stepBackDistance;

                        currentState = AttackState.SteppingBack;
                    }else{
                        moveDir = (nearestEnemy.transform.position - transform.position).normalized;
                        transform.position = Vector3.MoveTowards(
                            transform.position,
                            nearestEnemy.transform.position,
                            step
                        );
                    }
                    break;

                case AttackState.SteppingBack:
                    step = stepBackSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        retreatTarget,
                        step
                    );
                    
                    // Check if the monke has reached the retreat target
                    if (transform.position == retreatTarget)
                    {
                        currentState = AttackState.Chase;
                    }
                    break;
            }
        }
    }

    void FindNearestEnemy()
    {
        if (doppelAgent){
            nearestEnemy = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }
        // Find all GameObjects tagged "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        float closestDistance = Mathf.Infinity;
        Transform currentNearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentNearestEnemy = enemy.transform;
            }
        }

        // Update the private field
        nearestEnemy = currentNearestEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if we are chasing and not already in an attack sequence
        if (other.gameObject.CompareTag("Enemy") && currentState == AttackState.Chase)
        {
            PlayImpactSound();
            other.gameObject.GetComponent<Enemy>()?.PoopNei(damage, Element.Physical);
        }
        if(other.gameObject.CompareTag("Player") && doppelAgent)
        {
            PlayImpactSound();
            other.gameObject.GetComponent<PlayerHealth>()?.PoopNei(damage);
        }
    }

    private IEnumerator timerKill()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}