using System;
using System.Collections.Generic;
using System.Linq;
using Spells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Random = UnityEngine.Random;

[Serializable] 
public class EffectPrefabLookup
{
    public SpellType key;
    public GameObject value;
}
public class MrSpell : MonoBehaviour
{
    public List<EffectPrefabLookup> spellPrefabLookup = new List<EffectPrefabLookup>();
    public List<EffectPrefabLookup> cursePrefabLookup = new List<EffectPrefabLookup>();
    public List<int> spellWordLengths = new List<int>()
    {
        4,4,4,4,
        4,4,4,4,
        4,4,4,4,
        4,4,4,4
    };
    public Dictionary<string, SpellType> SpellLookup;
    public string typedText = "";
    private List<char> spellAlphabet;
    [SerializeField]
    public List<string> spellNames;
    [SerializeField]
    private MrInputVisualizer inputVisualizer;
    
    
    void Awake()
    {
        spellAlphabet = GetAlphabet();
        //spellAlphabet = leftKeyboard.ToCharArray().ToList();
        spellNames = GenerateSpellKeysUniqueChar(spellWordLengths);
        SpellLookup = new Dictionary<string, SpellType>();
        for (int i = 0; i < spellPrefabLookup.Count; i++)
        {
            SpellLookup.Add(spellNames[i], spellPrefabLookup[0].key);
        }
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        typedText = ""; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.character != '\0' && IsInAlphabet(e.character))
        {
            typedText += e.character;
            inputVisualizer.OnKeyPressed(typedText);
            ProcessInputToSpell(typedText);
            //Debug.Log("ONGUI Typed: " + typedText);
        }
    }
    private List<char> GetAlphabet()
    {
        var alphabet = new List<char>();
        for (char c = 'a'; c <= 'z'; c++)
        {
            alphabet.Add(c);
        }
        return alphabet;
    }
    public bool IsInAlphabet(char input)
    {
        //string keyPressed =(input.keyCode + "").ToLower();
        //if(keyPressed.Length != 1)return false;
        return spellAlphabet.Contains(input);
    }
    public void ProcessInputToSpell(string uncheckedInput)
    {
        
        Debug.Log(uncheckedInput);
        if (!SpellLookup.Keys.ToList().Any(x => IsPrefixOfSpell(uncheckedInput, x)))
        {
            inputVisualizer.OnSpellFailure(new List<int>(), new List<int>(), typedText);
            typedText = "";
            return;   
        }
        if(SpellLookup.ContainsKey(uncheckedInput))
        {
            
            inputVisualizer.OnSpellSuccess(typedText);
            typedText = "";
            SpawnSpell(uncheckedInput);
        }
    }

    public void SpawnSpell(string spell)
    {
        GameObject prefab = spellPrefabLookup.Find((x=>x.key == SpellLookup[spell])).value;
        var castedSpell =Instantiate(prefab,new Vector3(0, 0, 0), Quaternion.identity);
        castedSpell.GetComponent<Spell>().direction=new Vector2(1, 0);
    }

    public void TriggerCurse()
    {
        GameObject prefab = cursePrefabLookup[0].value;
        var castedSpell =Instantiate(prefab,new Vector3(0, 0, 0), Quaternion.identity);
        castedSpell.GetComponent<Spell>().direction=new Vector2(1, 0);
    }
    public bool IsPrefixOfSpell(string possiblePrefix, string spell)
    {
        if(possiblePrefix.Length > spell.Length) return false;
        return spell.Substring(0,possiblePrefix.Length).Equals(possiblePrefix);
    }

    public List<string> GenerateSpellKeysUniqueChar( List<int> spellLengths)
    {
        int spellCount = spellLengths.Count;
        List<string> spellKeys = new List<string>();
        
        for (int i = 0; i < spellCount; i++)
        {
            string spellKey = "";
            for (int charPos = 0; charPos < spellLengths[i]; charPos++)
            {
                spellKey+=GetUniqueCharacterAtPos(spellKeys, charPos);
            }
            spellKeys.Add(spellKey);
        }
        return spellKeys;
    }

    public char GetUniqueCharacterAtPos(List<string> spellKeys, int charPos)
    {
        List<char> alreadyUsedChars = spellKeys
            .Where( s => s.Length > charPos)
            .Select(s=> s[charPos])
            .ToList();
        List<char> usable = spellAlphabet.Except(alreadyUsedChars).ToList();

        return usable[Random.Range(0, usable.Count)];
    }

    public List<string> GenerateSpellKeys(List<int> spellLengths)
    {
        List<int> sortedDesc = spellLengths
            .OrderByDescending(i => i)
            .ToList();
        return GenerateSpellKeys(sortedDesc);
    }
    public List<string> GetSpellsWithSamePrefix(string prefix, List<string> spells)
    {
        return spells.Where(s => s.StartsWith(prefix)).ToList();
    }
}
