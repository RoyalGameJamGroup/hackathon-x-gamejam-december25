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

    public int score;

    private float spawnTimer;

    void Start()
    {
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
        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        enemyToSpawn.GetComponent<Enemy>().target = playerTransform.gameObject;

        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPos = Vector3.zero;
        bool positionFound = false;
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
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
}