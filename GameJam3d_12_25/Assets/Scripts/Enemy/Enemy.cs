using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Enemy : MonoBehaviour
{

    [SerializeField] public GameObject target;
    [SerializeField] protected float speed = 2.0f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int health = 50;
    Element status;

    public GameManager gameManager;
    [SerializeField] protected int scoreValue = 10;

    protected Vector3 moveDir;
    protected float step;
    [SerializeField] public float CollisionRadius = 0.5f;
    [SerializeField] public LayerMask obstacleMask;

    protected bool movementBlocked = false;


    // Update is called once per frame
    protected void Update()
    {
       // Check if movement is blocked
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

    public void KnockbackNei (Vector3 force)
    {
        transform.position += force;
    }

}
