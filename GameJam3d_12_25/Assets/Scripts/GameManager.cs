using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    
    

    [Header("Enemy Prefabs")]
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject sceletonPrefab;
    [SerializeField] GameObject seekerPrefab;

    [Header("Spawning Settings")]
    [SerializeField] Transform playerTransform;
    [SerializeField] float spawnInterval = 5f;
    [SerializeField] float spawnRadius = 20f;
    [SerializeField] float minPlayerDistance = 10f;


    [Header("Base Speeds")]
    [SerializeField] public float zombieSpeed = 2f;
    [SerializeField] public float sceletonSpeed = 1f;
    [SerializeField] public float seekerSpeed = 2f;

    public int score;
    private int nextLevelScoreThreshold = 5;
    public Transform levelFillRect;

    void UpdateLevelBar()
    {
        float fraction = Mathf.Clamp01((float)score / (float)nextLevelScoreThreshold) *0.68f;
        Vector3 newScale = levelFillRect.localScale;
        newScale.x = fraction;
        levelFillRect.localScale = newScale;
    }

   

    private float spawnTimer;
    

    void Start()
    {
        UpdateLevelBar();

        spawnTimer = spawnInterval;
        FindPlayer();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject[] enemyPrefabs = { zombiePrefab, sceletonPrefab, seekerPrefab };
        GameObject enemyToSpawn = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];

        // Set enemy speed based on type
        if (enemyToSpawn == zombiePrefab)
        {
            enemyToSpawn.GetComponent<Enemy>().speed = zombieSpeed;
        }
        else if (enemyToSpawn == sceletonPrefab)
        {
            enemyToSpawn.GetComponent<Enemy>().speed = sceletonSpeed;
        }
        else if (enemyToSpawn == seekerPrefab)
        {
            enemyToSpawn.GetComponent<Enemy>().speed = seekerSpeed;
        }

        enemyToSpawn.GetComponent<Enemy>().target = playerTransform.gameObject;
        enemyToSpawn.GetComponent<Enemy>().gameManager = this;

        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPos = Vector3.zero;
        bool positionFound = false;
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * spawnRadius;
            randomDirection.y = 0;

            Vector3 candidatePosition = transform.position + randomDirection;

            if (playerTransform != null)
            {
                if (Vector3.Distance(candidatePosition, playerTransform.position) >= minPlayerDistance)
                {
                    randomPos = candidatePosition;
                    positionFound = true;
                    break;
                }
            }
            else
            {
                randomPos = candidatePosition;
                positionFound = true;
                break;
            }
        }

        if (!positionFound)
        {
            Debug.LogWarning("Could not find a valid spawn position away from the player after " + maxAttempts + " attempts.");
        }
        
        randomPos.y = 0;

        return randomPos;
    }

    public void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);

        UpdateLevelBar();

        if(score >= nextLevelScoreThreshold)
        {
            TriggerLevelUp();
        }


    }

    void TriggerLevelUp()
    {
        Debug.Log("Level Up Triggered!");
        score = 0;
        nextLevelScoreThreshold += 10; // Increase threshold for next level
        Debug.Log("Level Up! Next level score threshold: " + nextLevelScoreThreshold);



        // heal player
        playerTransform.GetComponent<PlayerHealth>().heal();

        UpdateLevelBar();
    }

    
}