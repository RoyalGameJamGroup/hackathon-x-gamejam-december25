using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MrSpell : MonoBehaviour
{
    
    public GameObject[] spellPrefab;
    public Dictionary<string, GameObject> SpellLookup;
    public string typedText = "";
    private List<char> spellAlphabet;
    private string leftKeyboard = "qwertzasdfghyxcvb";
    [SerializeField]
    public List<string> spellNames;
    public List<int> spellWordLengths = new List<int>()
    {
        4,4,4,4,
        4,4,4,4,
        4,4,4,4,
        4,4,4,4
    };
    
    
    void Awake()
    {
        
        spellAlphabet = new List<char>();
        for (char c = 'a'; c <= 'z'; c++)
        {
            spellAlphabet.Add(c);
        }
        //spellAlphabet = leftKeyboard.ToCharArray().ToList();
        spellNames = GenerateSpellKeysUniqueChar(spellWordLengths);
        SpellLookup = new Dictionary<string, GameObject>();
        for (int i = 0; i < spellPrefab.Length; i++)
        {
            SpellLookup.Add(spellNames[i], spellPrefab[i]);
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
        var keyboard = Keyboard.current;

        foreach (var key in keyboard.allKeys)
        {
            if(!IsInAlphabet(key)) continue;
            if (key.wasPressedThisFrame)
            {
                //Debug.Log("Pressed: " + key.keyCode);
                typedText += (key.keyCode+"").ToLower();
                ProcessInputToSpell(typedText);
            }
        }
    }

    public bool IsInAlphabet(KeyControl input)
    {
        string keyPressed =(input.keyCode + "").ToLower();
        if(keyPressed.Length != 1)return false;
        return spellAlphabet.Contains(keyPressed[0]);
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
            typedText = "";
            SpawnSpell(uncheckedInput);
        }
    }

    public void SpawnSpell(string spell)
    {
        var castedSpell =Instantiate(SpellLookup[spell],new Vector3(0, 0, 0), Quaternion.identity);
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
