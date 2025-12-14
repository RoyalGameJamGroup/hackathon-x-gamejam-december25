using System;
using UnityEngine;
using UnityEngine.UI; // Required for handling UI elements
using Spells;
using TMPro;

public class GameManager : MonoBehaviour
{
    private string spell1Name = "";
    private string spell2Name = "";

    private string spell1Desc = "";
    private string spell2Desc = "";
    private string spell1Combo = "";
    private string spell2Combo = "";

    private Sprite spell1Icon = null;
    private Sprite spell2Icon = null;

    SpellType spell1Type;
    SpellType spell2Type;


    [Header("Ui Prefabs")]
[SerializeField] public Image spell1ImageUI;
 [SerializeField]   public Image spell2ImageUI;
 [SerializeField]   public TextMeshProUGUI spell1text;
 [SerializeField]   public TextMeshProUGUI spell2text;
 [SerializeField]   public Button spell1button;
 [SerializeField]   public Button spell2button;
 [SerializeField]   public GameObject uiObject;
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

        spell1button.onClick.AddListener(() => {
           pressedSpellButton(spell1Type);
        });
        spell2button.onClick.AddListener(() => {
           pressedSpellButton(spell2Type);
        });

        spawnTimer = spawnInterval;
        FindPlayer();
    }
    /*
    PauseManager.Instance.OnPauseStateChanged += ToggleUI;
    public void ToggleUI(bool isActive)
    {
        
    }*/
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerLevelUp();
        }
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

        // Retrieve random unknown spells from MrSpell
        //playerTransform.GetComponent<MrSpell>().RetrieveRandomUnknownSpells(2);
        bool didWork;
        (spell1Type, spell2Type, didWork) = playerTransform.GetComponent<MrSpell>().GetRandomUnknownSpell();

        playerTransform.GetComponent<MrSpell>().GetKnownSpellData(spell1Type, out spell1Name, out spell1Desc, out spell1Combo, out spell1Icon);
        playerTransform.GetComponent<MrSpell>().GetKnownSpellData(spell2Type, out spell2Name, out spell2Desc, out spell2Combo, out spell2Icon);

        // trigger UI popup
        uiObject.SetActive(true);
        spell1ImageUI.sprite = spell1Icon;
        spell2ImageUI.sprite = spell2Icon;
//        spell1text.text = spell1Name + "\n" + spell1Desc;
//        spell2text.text = spell2Name + "\n" + spell2Desc;

        PauseManager.Instance.SetGameRunningState(false);

        // add listeners to buttons to select spells
        
        // heal player
        playerTransform.GetComponent<PlayerHealth>().heal();

        UpdateLevelBar();
    }

    public void pressedSpellButton(SpellType spellType)
    {
        Debug.Log("Player selected spell: " + spellType.ToString());

        // hide Level Up UI
        uiObject.SetActive(false);
        PauseManager.Instance.SetGameRunningState(true);
    }



    
}