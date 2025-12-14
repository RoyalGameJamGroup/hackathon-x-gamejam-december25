using UnityEngine;

public class Teleport : Spell
{

    public float spellRange = 10.0f; 

    void Start()
    {
        SpawnEffect();
        PlaySpawnsSound();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); 

        playerObject.transform.position += new Vector3(direction.x * spellRange, 0.0f, direction.y * spellRange);
    }

    
}