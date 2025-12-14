using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLogic : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Setup")]
    public List<BoxCollider> spawnAreas;
    public List<GameObject> enemyPrefabs;
    public GameObject bossPrefab;

    [Header("Room Dependencies")]
    [Tooltip("List of rooms that must be finished before this room starts.")]
    public List<RoomLogic> prerequisiteRooms;

    // Internal reference: Small rooms will store which Boss Room controls them here
    private RoomLogic linkedBossRoom;

    public bool isFinished = false;

    [Header("Visuals")]
    public GameObject finishedVisualObject;

    [Header("Door Settings")]
    public List<DoorPair> roomDoors;
    public AudioSource asource;

    [Header("Wave Settings")]
    public uint difficulty = 1;
    public uint baseWaves = 3;
    public float waveTime = 20.0f;
    public float timeBetweenSpawns = 1.0f;

    private bool roomActive = false;

    [System.Serializable]
    public struct DoorPair
    {
        public GameObject openStateObject;
        public GameObject closedStateObject;
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>(); // Unity 2023+
        UpdateFinishedVisual();
        OpenDoorsInternal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!roomActive && other.CompareTag("Player"))
        {
            // --- NEW LOGIC: SEAL THE BOSS ROOM ---
            // If this is a small room, and we have a Boss Room that is currently finished/open,
            // we must close the Boss Room now because the player has re-started the cycle.
            if (linkedBossRoom != null && linkedBossRoom.isFinished)
            {
                Debug.Log($"Player entered {gameObject.name}. Sealing the Boss Room.");
                linkedBossRoom.SealRoom();
            }
            // -------------------------------------

            if (CheckPrerequisitesMet())
            {
                roomActive = true;
                ShutDoor();
                StartCoroutine(PlayLoop());
            }
            else
            {
                if (!isFinished) Debug.Log("Room Locked! Clear sub-rooms first.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Logic for the BOSS ROOM when Player leaves it
        if (other.CompareTag("Player") && isFinished && prerequisiteRooms.Count > 0)
        {
            Debug.Log("Player left Boss Room. Resetting Sub-Rooms (but keeping Boss Door open).");

            // 1. Reset all Sub-Rooms so they can be played again
            foreach (RoomLogic subRoom in prerequisiteRooms)
            {
                subRoom.ResetRoom();

                // 2. Tell the sub-room that *THIS* is the boss room that needs to be sealed later
                subRoom.linkedBossRoom = this;
            }

            // Note: We do NOT close the door here. We wait until the player enters a sub-room.
        }
    }

    // Called on Small Rooms to make them playable again
    public void ResetRoom()
    {
        StopAllCoroutines();
        roomActive = false;
        isFinished = false;
        UpdateFinishedVisual();
        OpenDoorsInternal(); // Ensure small room doors are open
    }

    // Called on the Boss Room when the player re-enters a Small Room
    public void SealRoom()
    {
        StopAllCoroutines(); // Just in case
        roomActive = false;
        isFinished = false;  // Mark as not done, so the lock is active again
        UpdateFinishedVisual();
        ShutDoor(); // CLOSE the boss door now
    }

    bool CheckPrerequisitesMet()
    {
        foreach (RoomLogic room in prerequisiteRooms)
        {
            if (!room.isFinished) return false;
        }
        return true;
    }

    void UpdateFinishedVisual()
    {
        if (finishedVisualObject != null)
            finishedVisualObject.SetActive(isFinished);
    }

    void ShutDoor()
    {
        if (asource) asource.Play();
        foreach (var door in roomDoors)
        {
            if (door.openStateObject) door.openStateObject.SetActive(false);
            if (door.closedStateObject) door.closedStateObject.SetActive(true);
        }
    }

    void OpenDoor()
    {
        if (asource) asource.Play();
        OpenDoorsInternal();
    }

    void OpenDoorsInternal()
    {
        foreach (var door in roomDoors)
        {
            if (door.openStateObject) door.openStateObject.SetActive(true);
            if (door.closedStateObject) door.closedStateObject.SetActive(false);
        }
    }

    Vector3 GetRandomPointInArea()
    {
        if (spawnAreas.Count == 0) return transform.position;

        BoxCollider area = spawnAreas[Random.Range(0, spawnAreas.Count)];
        Bounds b = area.bounds;

        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            b.center.y,
            Random.Range(b.min.z, b.max.z)
        );
    }

    IEnumerator PlayLoop()
    {
        // 1. Calculate Waves
        int totalWaves = (int)baseWaves + (int)difficulty;

        for (int wave = 0; wave < totalWaves; wave++)
        {
            for (int j = 0; j < enemyPrefabs.Count; j++)
            {
                int countToSpawn = 2 + (int)difficulty;
                for (int k = 0; k < countToSpawn; k++)
                {
                    gameManager.SpawnEnemyPrefab(GetRandomPointInArea(), enemyPrefabs[j]);
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
            }
            yield return new WaitForSeconds(waveTime);
        }

        // 2. Boss Phase
        Debug.Log("Spawning Boss...");
        GameObject activeBoss = gameManager.SpawnEnemyPrefab(GetRandomPointInArea(), bossPrefab);

        while (activeBoss != null && activeBoss.activeInHierarchy)
        {
            yield return null;
        }

        // 3. Victory & Difficulty Increase
        Debug.Log("Room Cleared! Difficulty Increasing.");

        gameManager.IncreaseDifficulty(); 
        difficulty++;

        isFinished = true;
        UpdateFinishedVisual();
        OpenDoor();
    }
}