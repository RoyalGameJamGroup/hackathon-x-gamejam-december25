using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MrInputVisualizer : MonoBehaviour
{
    public GameObject KeyPrefab;
    public List<GameObject> CurrentSpell = new List<GameObject>();
    private CancellationTokenSource ct;
    [SerializeField]
    private GameObject KeysCollection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnKeyPressed(string word)
    {
        ct?.Cancel();
        ClearKeys();
        CreateWord(word);
    }

    public async Task OnSpellSuccess(string word)
    {
        ct?.Cancel();
        ClearKeys();
        CreateWord(word);
        KeysCollection.GetComponent<Image>().color = Color.green;
        await WordEnded();
        KeysCollection.GetComponent<Image>().color = Color.clear;
        ClearKeys();
    }

    public async Task OnSpellFailure(List<int>correctChars, List<int> containedChars, string word)
    {
        ct?.Cancel();
        ClearKeys();
        CreateWord(word);
        foreach (var c in CurrentSpell)
        {
            c.GetComponent<Image>().color = Color.red;    
        }
        foreach (int c in containedChars)
        {
            CurrentSpell[c].GetComponent<Image>().color = Color.yellow;
        }
        foreach (int c in correctChars)
        {
            CurrentSpell[c].GetComponent<Image>().color = Color.green;
        }
        
        KeysCollection.GetComponent<Image>().color = Color.red;
        await WordEnded();
        KeysCollection.GetComponent<Image>().color = Color.clear;
        ClearKeys();
    }

    private async Task WordEnded()
    {
        ct?.Cancel();
        ct = new CancellationTokenSource();
        await Task.Delay(1000, ct.Token);
    }

    private void CreateWord(string word)
    {
        foreach (var c in word)
        {
            var nextKey = Instantiate(KeyPrefab, KeysCollection.transform);
            nextKey.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString().ToUpper();
            CurrentSpell.Add(nextKey);
        }
    }
    
    private void ClearKeys()
    {
        foreach (var keyInstance in CurrentSpell)
        {
            Destroy(keyInstance);
        }
        CurrentSpell.Clear();
    }
}
