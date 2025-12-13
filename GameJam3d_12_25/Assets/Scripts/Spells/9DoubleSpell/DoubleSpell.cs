using UnityEngine;

public class DoubleSpell : Spell
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MrSpell>().RedoSpell(lastSpell);
        Destroy(gameObject);
    }

}
