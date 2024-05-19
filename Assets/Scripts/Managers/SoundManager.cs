using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //fairly simplistic SoundManager, plays sounds and music along with a fade in and out
    public static SoundManager instance;

    [SerializeField] private AudioSource channel1, channel2, channel3, effectSource;
    [SerializeField] private float defaultMasterVolume;
    //[SerializeField] private Animator musicAnimator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ChangeMasterVolume(defaultMasterVolume);
    }

    public void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void PlayChannel(string channelName)
    {
        switch (channelName)
        {
            case "Channel 1":
                channel1.loop = true;
                channel2.Pause();
                channel3.Pause();
                channel1.Play();
                break;
            case "Channel 2":
                channel2.loop = true;
                channel1.Pause();
                channel3.Pause();
                channel2.Play();
                break;
            case "Channel 3":
                channel3.loop = true;
                channel2.Pause();
                channel1.Pause();
                channel3.Play();
                break;
            default: 
                channel1.Play();
                break; 
        }
    }

    public void StopChannel(string channelName)
    {
        switch (channelName)
        {
            case "Channel 1":
                channel1.Stop();
                break;
            case "Channel 2":
                channel2.Stop();
                break;
            case "Channel 3":
                channel3.Stop();
                break;
            default:
                channel1.Stop();
                channel2.Stop();
                channel3.Stop();
                break;
        }
    }

    //public void PlayMusic(AudioClip clip)
    //{
    //    musicSource.loop = true;
    //    musicSource.clip = clip;
    //    musicSource.Play();
    //}

    //public void StopMusic()
    //{
    //    musicSource.Stop();
    //}

    //public void TriggerFadeOut()
    //{
    //    musicAnimator.SetTrigger("fadeOut");
    //}

    //public void TriggerFadeIn()
    //{
    //    musicAnimator.SetTrigger("fadeIn");
    //}

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;  //this is adjusted with a slider
    }
}