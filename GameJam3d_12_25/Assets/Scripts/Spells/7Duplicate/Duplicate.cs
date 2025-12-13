using UnityEngine;

public class Duplicate : Spell
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
                detonate.Duplicate();
            }
        }
        Destroy(gameObject);
    }
}
