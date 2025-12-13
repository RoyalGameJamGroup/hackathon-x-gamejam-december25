using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider mastSlider, musicSlider, sfxSlider;

    

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("MasterVol", mastSlider.value);
        audioMixer.SetFloat("MusicVol", musicSlider.value);
        audioMixer.SetFloat("SFXVol", sfxSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVol()
    {
        audioMixer.SetFloat("MasterVol", mastSlider.value);
    }
    public void SetMusicVol()
    {
        audioMixer.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVol()
    {
        audioMixer.SetFloat("SFXVol", sfxSlider.value);
    }


}
