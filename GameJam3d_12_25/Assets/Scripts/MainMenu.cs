using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    public GameObject optionsMenu;
    public GameObject creditsScreen;
    public GameObject controlsScreen;
    
    public AudioMixer audioMixer;
    public Slider mastSlider, musicSlider, sfxSlider;



    // Start is called before the first frame update
    void Start()
    {
        optionsMenu.SetActive(false);
        creditsScreen.SetActive(false);
        controlsScreen.SetActive(false);

        audioMixer.SetFloat("MasterVol", mastSlider.value);
        audioMixer.SetFloat("MusicVol", musicSlider.value);
        audioMixer.SetFloat("SFXVol", sfxSlider.value);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);                     
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        creditsScreen.SetActive(true);
    }

    public void OpenControls()
    {
        controlsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsScreen.SetActive(false);
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
