using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.Rendering;

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

    [Header("GlobalMultipliers")]
    public float speedMult = 1.0f;
    public float damageMult = 1.0f;
    public float healthMult = 1.0f;

    public float playerDamageMult = 1.0f;

    private float spawnTimer;
    

    void Start()
    {
        spawnTimer = spawnInterval;
        FindPlayer();
    }


    public GameObject SpawnEnemyPrefab(Vector3 position, GameObject enemy)
    {
        GameObject enemyToSpawn = enemy;

        // Instantiate first
        GameObject newEnemy = Instantiate(enemyToSpawn, position, Quaternion.identity);

        // Setup dependencies
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.target = playerTransform.gameObject;
            enemyScript.gameManager = this;
        }

        return newEnemy; // Return the object so RoomLogic can track it
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
    }

    public void IncreaseDifficulty()
    {
        healthMult *= 1.1f;
        damageMult *= 1.05f;
    }


}