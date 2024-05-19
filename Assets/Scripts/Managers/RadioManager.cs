
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    public static RadioManager instance;


    public AudioSource stationOne;
    public AudioSource stationTwo;
    public AudioSource stationThree;
    public AudioSource fightMusic; 
    public AudioSource radioClick;
    public AudioSource soundEffects;
    public AudioSource ambience; 

    public bool onLand = true;
    public float channelDelay;
    public float delayTime = 0.5f;

    public float radioVolume = 0.25f;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        stationOne.volume = radioVolume;
        stationTwo.volume = 0;
        stationThree.volume = 0;
    }

    public void ChangeStation(int station)
    {
        print("In Change Station");
        radioClick.Play();

        switch (station)
        {
            case 1:
                stationOne.volume = 0;
                stationTwo.volume = radioVolume;
                Debug.Log("Case1");
                break;
            case 2:
                stationTwo.volume = 0;
                stationThree.volume = radioVolume;
                Debug.Log("Case2");
                break;
            case 3:
                stationThree.volume = 0;
                Debug.Log("Case3");
                break;
            case 4:
                stationOne.volume = radioVolume;
                Debug.Log("Case4");
                break;
            default:
                stationOne.volume = 0;
                stationTwo.volume = 0;
                stationThree.volume = 0;
                break;
        }
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffects.clip = clip;
        soundEffects.Play();
    }

    public void PlayAmbience(AudioClip clip)
    {
        ambience.clip = clip;
        ambience.Play();
    }

    public void StopAmbience()
    {
        ambience.Stop();
    }

    public void PlayFightMusic()
    {
        fightMusic.Play();
    }

    public void StopFightMusic()
    {
        fightMusic.Stop();
    }

    public void PauseAllStations()
    {
        stationOne.mute = true; 
        stationTwo.mute = true;
        stationThree.mute = true;
    }

    public void ResumeAllStations()
    {
        print("in ResumeAllStations");
        stationOne.mute = false;
        stationTwo.mute = false;
        stationThree.mute = false;
    }

    public void MuteAllStations()
    {
        stationOne.volume = 0;
        stationTwo.volume = 0;
        stationThree.volume = 0;
    }
}
