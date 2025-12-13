using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MrInputVisualizer : MonoBehaviour
{
    public GameObject KeyPrefab;
    public GameObject QueueElementPrefab;
    public List<GameObject> CurrentSpell = new List<GameObject>();
    public List<GameObject> CurrentQueue = new List<GameObject>();
    private CancellationTokenSource ct;
    private CancellationTokenSource ctqueue;
    [SerializeField]
    private GameObject KeysCollection;
    [SerializeField]
    private GameObject QueueCollection;
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
        await Task.Delay(500, ct.Token);
    }
    private async Task OnQueued()
    {
        ctqueue?.Cancel();
        ctqueue = new CancellationTokenSource();
        await Task.Delay(500, ctqueue.Token);
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

    public async Task ShowQueue(Queue<SpellType> queue, bool succuess, List<SpellPrefabLookup> spelllookups)
    {
        ctqueue?.Cancel();
        ClearQueue();
        CreateQueue(queue, succuess, spelllookups);
        if (succuess)
        {
            QueueCollection.GetComponent<Image>().color = Color.green;    
        }
        else
        {
            QueueCollection.GetComponent<Image>().color = Color.red;
        }

        await OnQueued();
        QueueCollection.GetComponent<Image>().color = Color.clear;
    }

    public void ClearQueue()
    {
        foreach (var keyInstance in CurrentQueue)
        {
            Destroy(keyInstance);
        }
        CurrentQueue.Clear();
    }

    public void CreateQueue(Queue<SpellType> queue, bool succuess, List<SpellPrefabLookup> spelllookups)
    {
        foreach (var e in queue)
        {
            var queueElement = Instantiate(QueueElementPrefab, QueueCollection.transform);
            Sprite s = spelllookups.Find(x => x.key == e).icon;
            queueElement.GetComponentInChildren<Image>().sprite = s;
            CurrentQueue.Add(queueElement);
        }
    }
}
