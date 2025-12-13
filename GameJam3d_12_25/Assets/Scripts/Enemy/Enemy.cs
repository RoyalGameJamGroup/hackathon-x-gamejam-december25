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



    // Update is called once per frame
    protected void Update()
    {
       
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
