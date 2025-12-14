using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Random = UnityEngine.Random;

[Serializable] 
public class SpellPrefabLookup
{
    public SpellType key;
    public GameObject value;
    public int wordLength;
    public Sprite icon;
    public string spellName;
    public string combo;
    public string description;
}
[Serializable] 
public class CursePrefabLookup
{
    public MalusController.MalusType key;
    public float weight;
    public Sprite icon;
}

public class MrSpell : MonoBehaviour
{
    public static MrSpell Instance {get; private set;}
    public List<SpellPrefabLookup> spellPrefabLookup = new List<SpellPrefabLookup>();
    public List<CursePrefabLookup> cursePrefabLookup = new List<CursePrefabLookup>();
    public Dictionary<string, SpellType> SpellLookup;
    public string typedText = "";
    private List<char> spellAlphabet;
    [SerializeField] public int queueSize =1;
    [SerializeField]
    public List<string> spellNames;
    [SerializeField]
    private MrInputVisualizer inputVisualizer;

    public GameObject lastSpell;
    
    public Dictionary<SpellType, HashSet<int>> knownSpellCharIdxs = new Dictionary<SpellType, HashSet<int>>(); 
    
    public Queue<SpellType> spellQueue = new Queue<SpellType>();
    void Awake()
    {
        if(Instance == null) Instance = this;
        spellAlphabet = GetAlphabet();
        //spellAlphabet = leftKeyboard.ToCharArray().ToList();
        spellNames = GenerateSpellKeysUniqueChar(spellPrefabLookup.Select(x=> x.wordLength).ToList());
        SpellLookup = new Dictionary<string, SpellType>();
        for (int i = 0; i < spellPrefabLookup.Count; i++)
        {
            SpellLookup.Add(spellNames[i], spellPrefabLookup[i].key);
            spellPrefabLookup[i].combo = spellNames[i];
        }
        foreach (var spellType in spellPrefabLookup.Select(x => x.key))
        {
            knownSpellCharIdxs.TryAdd(spellType, new HashSet<int>());
        }
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        typedText = ""; 
        SetSpellKnown(SpellType.Fireball);
        SetSpellKnown(SpellType.Freeze);
        SetSpellKnown(SpellType.Shockwave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI()
    {
        if(!PauseManager.Instance.isRunning) return;
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
        
        //Debug.Log(uncheckedInput);
        if (!SpellLookup.Keys.ToList().Any(x => IsPrefixOfSpell(uncheckedInput, x)))
        {
            string closest = GetClosestSpell(typedText);
            List<int> correct = new List<int>();
            List<int> contained = new List<int>();
            for (int i=0;i< typedText.Length;i++)
            {
                if (i < closest.Length)
                {
                    if (closest[i] == typedText[i])
                    {
                        correct.Add(i);
                    }
                    
                }
                if(closest.Contains(typedText[i]))
                {
                    contained.Add(i);
                }
            }
            SpellType spellType = SpellLookup[closest];
            knownSpellCharIdxs.TryAdd(spellType, new HashSet<int>());
            knownSpellCharIdxs[spellType].UnionWith(correct);
            inputVisualizer.OnSpellFailure(correct, contained, typedText);
            typedText = "";
            TriggerCurse();
            inputVisualizer.updateCurses();
            return;   
        }
        if(SpellLookup.ContainsKey(uncheckedInput))
        {
            SpellType spellType = SpellLookup[uncheckedInput];
            knownSpellCharIdxs.TryAdd(spellType, new HashSet<int>());
            knownSpellCharIdxs[spellType].UnionWith(Enumerable
                .Range(0, uncheckedInput.Length)
                .ToList());
            inputVisualizer.OnSpellSuccess(typedText);
            typedText = "";
            SpawnSpell(uncheckedInput);
        }
    }

    public async Task SpawnSpell(string spell)
    {
        
        SpellType spellType = SpellLookup[spell];
        Sprite spellSprite = spellPrefabLookup.Find(x => x.key == spellType).icon;
        if (spellQueue.Contains(spellType))
        {
            //Debug.Log("Spell is in Queue");
            await inputVisualizer.AnimateSpellToQueue(spellSprite);
            inputVisualizer.ShowQueue(spellQueue, false, spellPrefabLookup);
            return;
        }


        if(spellQueue.Count >=queueSize) spellQueue.Dequeue();
        spellQueue.Enqueue(spellType);
        await inputVisualizer.AnimateSpellToQueue(spellSprite);
        inputVisualizer.ShowQueue(spellQueue, true, spellPrefabLookup);
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 mousePos = (Vector2)Input.mousePosition;
        Vector2 direction = (mousePos - screenCenter).normalized;

        GameObject prefab = spellPrefabLookup.Find((x=>x.key == SpellLookup[spell])).value;
        Debug.Log(spell+" is casted "+ spellPrefabLookup.Find((x=>x.key == SpellLookup[spell])).spellName);
        var castedSpell = Instantiate(prefab, transform.position + new Vector3(direction.x, 0, direction.y), prefab.transform.rotation);
        castedSpell.GetComponent<Spell>().direction = direction;
        castedSpell.GetComponent<Spell>().lastSpell = lastSpell;
        lastSpell = prefab;
    }

    public void RedoSpell(GameObject lastSpell){
        Debug.Log("Redoing last spell");
        Debug.Log(lastSpell);
        if(lastSpell == null) return;
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 mousePos = (Vector2)Input.mousePosition;
        Vector2 direction = (mousePos - screenCenter).normalized;

        GameObject prefab = lastSpell;
        var castedSpell = Instantiate(prefab, transform.position + new Vector3(direction.x, 0, direction.y), prefab.transform.rotation);
        castedSpell.GetComponent<Spell>().direction = direction;
    }

    public MalusController.MalusType GetRandomCurse()
    {
        float probSum= cursePrefabLookup.Select(x => x.weight).Sum(); 
        float random = Random.value * probSum;
        foreach (var i in cursePrefabLookup)
        {
            random -= i.weight;
            if (random <= 0f)
                return i.key;
        }

        // Fallback (in case of floating-point precision issues)
        return cursePrefabLookup[^1].key;
    }
    public void TriggerCurse()
    {
        
        MalusController.MalusType malus = GetRandomCurse();
        MalusController.Instance.AddMalus(malus);
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

    public string GetClosestSpell(string wrongSpell)
    {
        List<string> spellKeys = new List<string>();
        int matchingCharNum = 0;
        foreach (var spell in SpellLookup)
        {
            int cn= GetMatchingCharNumber(wrongSpell, spell.Key);
            if (cn > matchingCharNum)
            {
                matchingCharNum = cn;
                spellKeys.Clear();
                spellKeys.Add(spell.Key);
            }else if (cn == matchingCharNum)
            {
                spellKeys.Add(spell.Key);
            }
        }
        return spellKeys.OrderBy((x=>GetCommonLetters(wrongSpell, x).Count)).Last();
    }

    public List<char> GetCommonLetters(string spell1, string spell2)
    {
        return spell1.ToCharArray().ToList()
            .Intersect(spell2.ToCharArray().ToList())
            .ToList();
    }
    

    public int GetMatchingCharNumber(string spell1, string spell2)
    {
        int shorterLength = spell1.Length>spell2.Length ? spell2.Length : spell1.Length;
        int matchingCharNumber = 0;
        for (int i = 0; i < shorterLength; i++)
        {
            matchingCharNumber += spell1[i] == spell2[i] ? 1 : 0;
        }
        return matchingCharNumber;
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

    public bool GetSpellData(SpellType spell, out string spellname, out string description, out string combo, out Sprite icon)
    {
        int idx = spellPrefabLookup.FindIndex(x=>x.key == spell);
        spellname =  spellPrefabLookup[idx].spellName;
        Debug.Log("Getting data for spell: " + spellname);
        combo = spellPrefabLookup[idx].combo;
        description = spellPrefabLookup[idx].description;
        icon = spellPrefabLookup[idx].icon;
        return true;
    }

    public (SpellType spell1, SpellType spell2, bool unknownSpellsExist) GetRandomUnknownSpell()
    {
        List<SpellType> stillUnknownSpells = new List<SpellType>();
        foreach (SpellType spellType in SpellLookup.Values)
        {
            
            string spellName;
            string description;
            string combo;
            Sprite icon;
            GetKnownSpellData(spellType, out spellName, out description, out combo, out icon);
            if (combo.Contains("_"))
            {
                stillUnknownSpells.Add(spellType);
            }
        }
        if(stillUnknownSpells.Count == 0) return (SpellType.Fireball,SpellType.Fireball,false);
        int randmIdx1 = (int)Math.Floor( Random.value * stillUnknownSpells.Count());
        SpellType spell1 = stillUnknownSpells[randmIdx1];
        stillUnknownSpells.RemoveAt(randmIdx1);
        
        if(stillUnknownSpells.Count == 0) return (spell1, spell1, true);
        int randmIdx2 = (int)Math.Floor( Random.value * stillUnknownSpells.Count());
        SpellType spell2 = stillUnknownSpells[randmIdx2];
        stillUnknownSpells.RemoveAt(randmIdx2);
        
        
        return (spell1, spell2, true);
    }

    public void SetSpellKnown(SpellType spell)
    {
        string spellName;
        string description;
        string combo;
        Sprite icon;
        GetSpellData(spell, out spellName, out description, out combo, out icon);
        knownSpellCharIdxs[spell].UnionWith(Enumerable
            .Range(0, combo.Length)
            .ToList());
    }

    public bool GetKnownSpellData(SpellType spell, out string spellname, out string description, out string combo,
        out Sprite icon)
    {
        string spellCombo;
        GetSpellData(spell, out spellname, out description, out spellCombo, out icon);
        string knownCombo = "";
        for(int i=0;i<spellCombo.Length;i++)
        {
            if (knownSpellCharIdxs[spell].Contains(i))
            {
                knownCombo += spellCombo[i];
            }
            else
            {
                knownCombo += "_";
            }
        }
        combo = knownCombo;
        return true;
    }
    
}
