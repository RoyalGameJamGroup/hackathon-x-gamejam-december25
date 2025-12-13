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



    // Update is called once per frame
    void Update()
    {
       
    }

    public void PoopNei(int damage, Element el)
    {
        health -= damage;
        status = el;
    }

    public void KnockbackNei (Vector3 force)
    {
        transform.position += force;
    }

}
