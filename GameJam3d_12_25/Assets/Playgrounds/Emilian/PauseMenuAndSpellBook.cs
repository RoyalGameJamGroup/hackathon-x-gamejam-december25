using UnityEngine;

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
