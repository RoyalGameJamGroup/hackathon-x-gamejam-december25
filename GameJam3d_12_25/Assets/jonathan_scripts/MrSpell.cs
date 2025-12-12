using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MrSpell : MonoBehaviour
{
    public Dictionary<string, string> SpellLookup = new Dictionary<string, string>()
    {
        {"huso", "fireball"}
    };
    public string typedText = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        typedText = "";   
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;

        foreach (var key in keyboard.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                //Debug.Log("Pressed: " + key.keyCode);
                typedText += (key.keyCode+"").ToLower();
                ProcessInputToSpell(typedText);
            }
        }
    }

    public void ProcessInputToSpell(string uncheckedInput)
    {
        Debug.Log(typedText);
        if (!SpellLookup.Keys.ToList().Any(x => IsPrefixOfSpell(uncheckedInput, x)))
        {
            typedText = "";
            return;   
        }
        if(SpellLookup.ContainsKey(uncheckedInput))
        {
            Debug.Log(SpellLookup[uncheckedInput]);
        }
    }

    public bool IsPrefixOfSpell(string possiblePrefix, string spell)
    {
        if(possiblePrefix.Length > spell.Length) return false;
        return spell.Substring(0,possiblePrefix.Length).Equals(possiblePrefix);
    }
}
