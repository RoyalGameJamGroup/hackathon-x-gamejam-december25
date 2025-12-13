using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuAndSpellBook : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject spellBook;

    

    private void Awake()
    {
        pauseMenu.SetActive(false);
        spellBook.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TryTogglePauseMenu();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            TryToggleSpellbook();
        }
    }

    private void TryTogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            return;
        }
           
        if(!spellBook.activeSelf)
        {
            pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void TryToggleSpellbook()
    {
        if (spellBook.activeSelf)
        {
            spellBook.SetActive(false);
            return;
        }

        if (!pauseMenu.activeSelf)
        {
            spellBook.SetActive(true);
        }
    }
}
