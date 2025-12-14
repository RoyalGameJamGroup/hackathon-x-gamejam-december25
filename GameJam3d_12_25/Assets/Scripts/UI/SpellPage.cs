using Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellPage : MonoBehaviour
{
    //some way to clearly identify what spell this is for    
    [SerializeField] private SpellType spellType;
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI comboName;
    [SerializeField] private TextMeshProUGUI spellDescription;
    [SerializeField] private Image spellIcon;
    private void OnEnable()
    {
        string name;
        string comboNameString;
        string description;
        Sprite icon;

        
        bool ranCorrectly = MrSpell.Instance.GetKnownSpellData(spellType,out name, out description, out comboNameString, out icon);
        if (!ranCorrectly)
            Debug.LogError("Something went wrong when trying to get Spell Data !");
        spellName.text = name;
        comboName.text = comboNameString;
        spellDescription.text = description;
        spellIcon.sprite = icon;

    }
}
