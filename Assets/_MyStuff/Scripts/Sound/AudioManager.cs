using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

    public AudioMixer mainMixer;

    public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

    IEnumerator FadeIncoroutine;

    IEnumerator FadeOutcoroutine;


    void Awake()
	{
        

        if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = s.mixerGroup;
		}

        
    }

	public void Play(string sound)
	{

		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

    public void SetMixerVolume()
    {
        if (PlayerPrefs.HasKey("sfxVol"))
        {
            mainMixer.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVol"));
            Debug.Log("sfxVol : " + PlayerPrefs.GetFloat("sfxVol"));
        }
        else
        {
            // mainMixer.SetFloat("sfxVol", 0);
        }



        if (PlayerPrefs.HasKey("musicVol"))
        {
            mainMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
            Debug.Log("musicVol : " + PlayerPrefs.GetFloat("musicVol"));
        }
        else
        {
            // mainMixer.SetFloat("musicVol", 0);
        }
    }
    public void FadeInCaller(string sound, float time, float maxVolume)
    {
        SetMixerVolume();
        FadeIncoroutine = FadeIn(sound, time, maxVolume);
        StartCoroutine(FadeIncoroutine);

    }

    public void FadeOutCaller(string sound, float time)
    {
        SetMixerVolume();
        FadeOutcoroutine = FadeOut(sound, time);
        StartCoroutine(FadeOutcoroutine);
    }

    public IEnumerator FadeIn(string sound, float time, float maxVolume)
    {

        
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            //return;
        }

        if(s.fadingOut)
        {
            StopCoroutine(FadeOutcoroutine);
        }
        s.fadingIn = true;
        s.fadingOut = false;

        s.source.Play();
        s.source.volume = 0;

        float audioVolume = s.source.volume;

        while(s.source.volume < maxVolume && s.fadingIn)
        {
            audioVolume += time;
            s.source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }

       /*s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();*/
    }

    public IEnumerator FadeOut(string sound, float time)
    {

        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            //return;
        }

        s.fadingIn = false;
        s.fadingOut = true;

        //s.source.Play();
        //s.source.volume = 0;

        float audioVolume = s.source.volume;

        while (s.source.volume >= time && s.fadingOut)
        {
            audioVolume -= time;
            s.source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
        s.source.Stop();

        /*s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
         s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

         s.source.Play();*/
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Stop();
    }
}
