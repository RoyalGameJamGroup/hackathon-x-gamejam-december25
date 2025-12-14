using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MalusController : MonoBehaviour
{
    //Millions of references, if this wasn't a gamejam these should all be accessible through some central singleton but alas...
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Playermovement playermovement;
    [SerializeField] private ShortSightedController shortSightedController;
    [SerializeField] private BrainrotController brainrotController;
    [SerializeField] private SpeedUpMalus speedUpMalus;
    [SerializeField] private GameObject detonation;
    [SerializeField] private FilterMalusController filterMalusController;
    [SerializeField] private SchizoMalusController schizoMalusController;
    [SerializeField] private DoppelagentMalus doppelagentMalus;




    public static MalusController Instance { get; private set; }

    //not really used but let's still implement it
    private int goFastCount = 0;
    private int shortSightedCount = 0;
    //not really used but let's still implement it
    private int rebellionCount = 0;
    private int filterCount = 0;
    private int brainrotCount = 0;
    //not really used but let's still implement it
    private int speedUpCount = 0;
    private int schizoCount = 0;
    //not really used but let's still implement it
    private int detonationCount = 0;

    private void Awake()
    {
        if(Instance == null) Instance = this;


    }


    public void AddMalus(MalusType malusType)
    {
        Debug.Log("Adding malus type: " + malusType);
        switch (malusType)
        {
            case MalusType.GoFast:
                //Access player controller, double (?) speed, halve HP
                playermovement.Speed *= 2;
                playerHealth.maxHealth-= playerHealth.maxHealth / 2;
                if (playerHealth.maxHealth < playerHealth.currentHealth)
                    playerHealth.currentHealth = playerHealth.maxHealth;
                goFastCount++;
                break;
            case MalusType.Shortsighted:
                //Add Overlay that get worse, depending on the already aquired shortSightedCount
                shortSightedController.AddShortSightedNess();
                shortSightedCount++;
                break;
            case MalusType.Rebellion:
                //Get all current companions and turn them hostile
                doppelagentMalus.turnOver();
                break;
            case MalusType.Filter:
                filterMalusController.IncreaseFilters();
                filterCount++;
                break;
            case MalusType.Brainrot:
                //Add Brainrot as Overlay and Audio
                brainrotController.ActivateRandomVideoPlayer();
                brainrotCount++;
                break;
            case MalusType.SpeedUp:
                //Access enemies, double their speed
                speedUpMalus.ApplySpeedUp();
                speedUpCount++;
                break;
            case MalusType.Schizo:
                schizoMalusController.InCreaseSchizoLevel();
                schizoCount++;
                break;
            case MalusType.Detonation:
                //Kill the player but for now do nothing (we don't use that one without conditions being met)
                GameObject player = GameObject.FindWithTag("Player");
                Instantiate(detonation, player.transform.position, Quaternion.identity);
                detonationCount++;
                break;

        }
    }

    public enum MalusType
    {
        GoFast,
        Shortsighted,
        Rebellion,
        Filter,
        Brainrot,
        SpeedUp,
        Schizo,
        Detonation
    }
}
