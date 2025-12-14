using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private GameObject CurseCollection;

    [SerializeField] private Color wrong;
    [SerializeField] private Color right;
    [SerializeField] private Color semicorrect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PauseManager.Instance.OnPauseStateChanged += ToggleUI;
        for (int i = 0; i < MrSpell.Instance.queueSize; i++)
        {
            var queueElement = Instantiate(QueueElementPrefab, QueueCollection.transform);
            CurrentQueue.Add(queueElement);
            queueElement.GetComponentsInChildren<Image>().First(x=>x.name=="SpellSprite").color = Color.clear;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUI(bool isRunning)
    {
        KeysCollection.SetActive(isRunning);
        QueueCollection.SetActive(isRunning);
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
        KeysCollection.GetComponent<Image>().color = right;
        
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
            c.GetComponent<Image>().color = wrong;    
        }
        foreach (int c in containedChars)
        {
            CurrentSpell[c].GetComponent<Image>().color = semicorrect;
        }
        foreach (int c in correctChars)
        {
            CurrentSpell[c].GetComponent<Image>().color = right;
        }
        
        KeysCollection.GetComponent<Image>().color = wrong;
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
        await Task.Delay(100, ctqueue.Token);
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

    public async Task AnimateSpellToQueue(Sprite spell)
    {
        var spellAnimation = Instantiate(QueueElementPrefab, KeysCollection.transform);
        spellAnimation.GetComponentsInChildren<Image>().First(x=>x.name=="SpellSprite").sprite = spell;
        spellAnimation.GetComponentsInChildren<Image>().First(x=>x.name=="SpellSprite").color = Color.white;
        CurrentSpell.Add(spellAnimation);
        
    }
    public async Task ShowQueue(Queue<SpellType> queue, bool succuess, List<SpellPrefabLookup> spelllookups)
    {
        ctqueue?.Cancel();
        ClearQueue();
        CreateQueue(queue, succuess, spelllookups);
        if (succuess)
        {
            QueueCollection.GetComponent<Image>().color = right;    
        }
        else
        {
            QueueCollection.GetComponent<Image>().color = wrong;
        }

        await OnQueued();
        QueueCollection.GetComponent<Image>().color = Color.clear;
    }

    public void ClearQueue()
    {
        foreach (var keyInstance in CurrentQueue)
        {
            //Destroy(keyInstance);
        }
        //CurrentQueue.Clear();
    }

    public void CreateQueue(Queue<SpellType> queue, bool succuess, List<SpellPrefabLookup> spelllookups)
    {
        for (int i = 0; i < MrSpell.Instance.queueSize; i++)
        {
            if (i < queue.Count)
            {
                SpellType e = queue.Reverse().ToArray()[i];
                Sprite s = spelllookups.Find(x => x.key == e).icon;
                CurrentQueue[i].GetComponentsInChildren<Image>().First(x=>x.name=="SpellSprite").sprite = s;
                CurrentQueue[i].GetComponentsInChildren<Image>().First(x=>x.name=="SpellSprite").color = Color.white;
            }
            else
            {
                CurrentQueue[i].GetComponentsInChildren<Image>().First(x=>x.name=="SpellSprite").color = Color.clear;
            }
            
        }
    }
}
