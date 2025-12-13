using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Enemy : MonoBehaviour
{

    [SerializeField] public GameObject target;
    [SerializeField] protected int speed = 2;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int health = 50;
    Element status;

    public GameManager gameManager;
    [SerializeField] protected int scoreValue = 10;

    Vector3 knockbackForce;
    float knockBackTimer = 0f;

    // Update is called once per frame
    protected void Update()
    {
      if(Time.time < knockBackTimer){
        transform.position += knockbackForce * Time.deltaTime;
      }
      
    }

    public void PoopNei(int damage, Element el)
    {
        health -= damage;
        status = el;
        if(health <= 0)
        {
            gameManager.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    public void KnockbackNei (Vector3 force)
    {
      knockBackTimer = Time.time + 0.3f;
      Debug.Log("timer: " + knockBackTimer);
      knockbackForce = force;
    }

}
