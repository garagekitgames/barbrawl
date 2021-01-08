using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using garagekitgames;
using SO;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;

    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Toggle tutorialOnStart;
    public BoolVariable showTutorialOnStart;
    public bool started = false;
    public void SetMusicVolume(float volume)
    {
        if (started)
        {
            mainMixer.SetFloat("musicVol", volume);
            PlayerPrefs.SetFloat("musicVol", volume);
            PlayerPrefs.Save();
        }
        
    }
    public void SetSFXVolume(float volume)
    {
        if (started)
        {
            mainMixer.SetFloat("sfxVol", volume);
            PlayerPrefs.SetFloat("sfxVol", volume);
            PlayerPrefs.Save();
        }
        
    }

    public void ShowPrivacy()
    {
        SceneManager.LoadScene("PrivacyDemo");
    }

    public void ShowTerms()
    {
        SceneManager.LoadScene("PrivacyDemo");
    }

    public void CloseSettings()
    {
        //SceneManager.LoadScene("PrivacyDemo");
    }

    public void SetTutorialOnStart(bool value)
    {
        showTutorialOnStart.value = value;
        
        PersistableSO.Instance.Save();
    }
    private void Awake()
    {
        if(PlayerPrefs.HasKey("sfxVol"))
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVol");
            Debug.Log("sfxVol : " + PlayerPrefs.GetFloat("sfxVol"));
        }
        else
        {
            //sfxVolumeSlider.value = 0;
        }

       

        if (PlayerPrefs.HasKey("musicVol"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVol");
            Debug.Log("musicVol : " + PlayerPrefs.GetFloat("musicVol"));
        }
        else
        {
            //musicVolumeSlider.value = 0;
        }

        started = true;
        tutorialOnStart.isOn = showTutorialOnStart.value;
    }

}
