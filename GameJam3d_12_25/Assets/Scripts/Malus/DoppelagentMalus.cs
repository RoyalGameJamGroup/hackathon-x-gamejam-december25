using UnityEngine;

public class DoppelagentMalus : MonoBehaviour
{
    
    public void turnOver(){
        GameObject[] spawnables = GameObject.FindGameObjectsWithTag("Spawnable");
        foreach(GameObject spawn in spawnables){
            Monke monkeScript = spawn.GetComponent<Monke>();
            if(monkeScript != null){
                monkeScript.doppelAgent = true;
            }
        }
    }

}
