using NUnit.Framework;
using System;
using UnityEngine;

using UnityEngine.Rendering;

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

    public GameObject combo;
    public TextMeshProUGUI newSpellTitle;
    public Image newSpellIcon;
    public TextMeshProUGUI newSpellCombo;
    public Button newSpellOkButton;

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
    public int nextLevelScoreThreshold = 50;
    public Transform levelFillRect;


    [Header("GlobalMultipliers")]
    public float speedMult = 1.0f;
    public float damageMult = 1.0f;
    public float healthMult = 1.0f;

    public float playerDamageMult = 1.0f;

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

        newSpellOkButton.onClick.AddListener(() => {
            continueNowDuHurenMalte();
        });

        spawnTimer = spawnInterval;
        FindPlayer();
    }

    /*
    PauseManager.Instance.OnPauseStateChanged += ToggleUI;
    public void ToggleUI(bool isActive)
    {
        
    }*/
    /*void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerLevelUp();
        }
       
    }*/

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

        UpdateLevelBar();

        if(score >= nextLevelScoreThreshold)
        {
            TriggerLevelUp();
        }


    }


    public void IncreaseDifficulty()
    {
        healthMult *= 1.1f;
        damageMult *= 1.05f;
    }



    void TriggerLevelUp()
    {
        Debug.Log("Level Up Triggered!");
        score = 0;
        nextLevelScoreThreshold = (int)(1.5f * nextLevelScoreThreshold); // Increase threshold for next level
        Debug.Log("Level Up! Next level score threshold: " + nextLevelScoreThreshold);

        // Retrieve random unknown spells from MrSpell
        //playerTransform.GetComponent<MrSpell>().RetrieveRandomUnknownSpells(2);
        bool didWork;
        (spell1Type, spell2Type, didWork) = playerTransform.GetComponent<MrSpell>().GetRandomUnknownSpell();

        playerTransform.GetComponent<MrSpell>().GetSpellData(spell1Type, out spell1Name, out spell1Desc, out spell1Combo, out spell1Icon);
        playerTransform.GetComponent<MrSpell>().GetSpellData(spell2Type, out spell2Name, out spell2Desc, out spell2Combo, out spell2Icon);

        if(!didWork)
        {
            Debug.Log("No more unknown spells to learn.");
            return;
        }
        // trigger UI popup
        uiObject.SetActive(true);
        spell1ImageUI.sprite = spell1Icon;
        spell2ImageUI.sprite = spell2Icon;
        Debug.Log("Spell 1: " + spell1Name + " - " + spell1Desc);
        spell1text.text = spell1Name + "\n" + spell1Desc;
        spell2text.text = spell2Name + "\n" + spell2Desc;

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
        //uiObject.SetActive(false);
        string spellName;
        string comboString;
        string description;
        Sprite icon;
        playerTransform.GetComponent<MrSpell>().GetSpellData(spellType, out spellName, out description, out  comboString, out  icon);
        newSpellTitle.text = "New Spell Learned: " + spellName + "\n" + description;
        newSpellIcon.sprite = icon;
        newSpellCombo.text = "Cast spell with: \n" + "->\"" + comboString + "\"<-";


        MrSpell.Instance.SetSpellKnown(spellType);

        combo.SetActive(true);
    }

    public void continueNowDuHurenMalte(){


        combo.SetActive(false);
        uiObject.SetActive(false);
        PauseManager.Instance.SetGameRunningState(true);
    }




}