using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public string mainMenuScene;

    public GameObject optionsMenu;
    public GameObject controlsScreen;
           
    void OnEnable()
    {
        optionsMenu.SetActive(false);
        controlsScreen.SetActive(false);

    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }
   
    public void OpenControls()
    {
        controlsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void CloseControls()
    {
        controlsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
