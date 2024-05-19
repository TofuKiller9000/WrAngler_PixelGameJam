using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HowToPlayManager : MonoBehaviour
{

    [SerializeField] private AudioClip punch;
    [SerializeField] private AudioClip transistionSoundEffect;
    [SerializeField] private Animator fishermanAnimator;

    public int punchCount;
    public float cooldownTimerValue;
    public float cooldownTimerStart = 0.45f;
    public bool cooldownComplete;

    public GameObject playButton;
    
    public Sprite playButton1;
    public Sprite playButton2;
    public Sprite playButton3;
    public GameObject waveTransition;
    
    private SpriteRenderer playButtonSprite;
    private void Awake()
    {
        playButtonSprite = playButton.GetComponent<SpriteRenderer>();

        cooldownTimerValue = cooldownTimerStart;

        fishermanAnimator = GetComponent<Animator>();

        RadioManager.instance.PauseAllStations();
    }

    private void Update()
    {
        if (cooldownTimerValue < 0.01f)
        {
            cooldownComplete = true;
        }
        else
        {
            cooldownTimerValue -= Time.deltaTime;
        }
    }

    public void OnStartPunch(InputAction.CallbackContext context)
    {
        if (context.started && cooldownComplete == true)
        {
            cooldownTimerValue = cooldownTimerStart;

            punchCount++;

            Debug.Log(punchCount);

            switch(punchCount)
            {
                case 1:
                    playButtonSprite.sprite = playButton1;
                    RadioManager.instance.PlaySoundEffect(punch);
                    fishermanAnimator.SetTrigger("Punch");
                    break; 
                case 2:
                    playButtonSprite.sprite = playButton2;
                    RadioManager.instance.PlaySoundEffect(punch);
                    fishermanAnimator.SetTrigger("Punch");
                    break; 
                case 3:
                    playButtonSprite.sprite = playButton3;
                    RadioManager.instance.PlaySoundEffect(punch);
                    RadioManager.instance.PlayAmbience(transistionSoundEffect);
                    fishermanAnimator.SetTrigger("Punch");
                    waveTransition.SetActive(true);
                    Debug.Log("NEXT SCENE");
                    break; 
            }

            cooldownTimerValue = cooldownTimerStart;
            cooldownComplete = false;
        }
    }

}
