using Unity.VisualScripting;
using UnityEngine;

public class Explosion : Spell
{
    void Start()
    {
        PlaySpawnsSound();
        // Get objects
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Spawnable");
        foreach (GameObject obj in foundObjects)
        {
            Detonate detonate = obj.GetComponent<Detonate>();
            if (detonate != null)
            {
                detonate.Explode();
            }
        }
        Destroy(gameObject);
    }
}
