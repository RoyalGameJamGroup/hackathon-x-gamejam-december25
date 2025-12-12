using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Enemy : MonoBehaviour
{

    [SerializeField] GameObject target;
    [SerializeField] int speed;
    [SerializeField] int health;
    Element status;



    // Update is called once per frame
    void Update()
    {
        if (target == null) return;


        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );
    }

    public void PoopNei(int damage, Element el)
    {
        health -= damage;
        status = el;
    }

}
