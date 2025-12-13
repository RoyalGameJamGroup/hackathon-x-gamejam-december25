using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellPage : MonoBehaviour
{
    //some way to clearly identify what spell this is for
    [SerializeField] private string key;

    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI comboName;
    [SerializeField] private TextMeshProUGUI spellDescription;
    [SerializeField] private Image spellIcon;
    private void OnEnable()
    {
        //somehow retrieve values for all of the above from somewhere and set them
    }
}
