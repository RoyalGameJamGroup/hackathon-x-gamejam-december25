using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [SerializeField] public GameObject target;
    [SerializeField] public float speed = 2.0f;
    [SerializeField] protected int damage = 10;
    public int health = 50;
    [HideInInspector]
    public int maxhealth;
    public Element status;

    public GameObject healthBar;
    public float barOffset = 1.0f;

    public GameManager gameManager;
    [SerializeField] protected int scoreValue = 10;

    Vector3 knockbackForce;
    float knockBackTimer = 0f;

    protected Vector3 moveDir;
    protected float step;
    [SerializeField] public float CollisionRadius = 0.5f;
    [SerializeField] public LayerMask obstacleMask;

    protected bool movementBlocked = false;
    protected bool frozen = false;


    protected virtual void Start()
    {
        maxhealth = health;
        // Instantiate health bar
        GameObject healthBars = GameObject.FindWithTag("HealthBars");
        var bar = Instantiate(healthBar, healthBars.transform);

        bar.GetComponent<HealthBar>().SetMonitor(gameObject.GetComponent<Enemy>(),gameObject.transform, barOffset);
        Debug.Log("Set health bar for enemy" + gameObject.GetInstanceID());
    }

    // Update is called once per frame
    protected void Update()
    {

      if(Time.time < knockBackTimer){
        transform.position += knockbackForce * Time.deltaTime;
      }

       // Check if movement is blocked
       if(!frozen){
            if (Physics.SphereCast(
                transform.position,
                CollisionRadius,
                moveDir,
                out RaycastHit hit,
                step,
                obstacleMask
            ))
            {
                Debug.Log("Enemy movement blocked by " + hit.collider.gameObject.name);
                movementBlocked = true;
            }
            else
            {
                movementBlocked = false;
            }
       }
        
    }

    public void PoopNei(int damage, Element el)
    {
        health -= damage;
        status = el;
        if(health <= 0)
        {
            gameManager?.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    public void Freeze(float length)
    {
        Debug.Log("I'm freezing");
        StartCoroutine(FreezeRoutine(length));
    }

    IEnumerator FreezeRoutine(float length)
    {
        frozen = true;
        movementBlocked = true;
        Debug.Log("Frozen for " + length + " seconds");
        
        yield return new WaitForSeconds(length);
        Debug.Log("Thawing out");
        movementBlocked = false;
        frozen = false;
    }


    public void KnockbackNei (Vector3 force)
    {
      knockBackTimer = Time.time + 0.3f;
      Debug.Log("timer: " + knockBackTimer);
      knockbackForce = force;
    }

}
