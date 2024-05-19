using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clickchecker : MonoBehaviour
{

    public int punchCount;
    public float cooldownTimerValue;
    public float cooldownTimerStart = 0.45f;
    public bool cooldownComplete;

    public GameObject playButton;
    public SpriteRenderer playButtonSprite;
    public Sprite playButton1;
    public Sprite playButton2;
    public Sprite playButton3;
    public GameObject waveTransition;
    public AudioSource waveTransitionAudio;
    public Rigidbody2D waveTransitionRB;
    public GameObject fisherman;
    public Animator fishermanAnimator;

    public AudioSource punch;

    // Start is called before the first frame update
    void Start()
    {
        playButtonSprite = playButton.GetComponent<SpriteRenderer>();

        cooldownTimerValue = cooldownTimerStart;

        fishermanAnimator = fisherman.GetComponent<Animator>();

        waveTransitionRB = waveTransition.GetComponent<Rigidbody2D>();

        waveTransitionAudio = waveTransition.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (cooldownTimerValue < 0.01f)
        {
            cooldownComplete = true;
            fishermanAnimator.SetBool("Punched", false);
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

            if(punchCount == 1)
            {
                playButtonSprite.sprite = playButton1;

                punch.Play();

                fishermanAnimator.SetBool("Punched", true);
            }
            else if(punchCount == 2)
            {
                playButtonSprite.sprite = playButton2;

                punch.Play();

                fishermanAnimator.SetBool("Punched", true);
            }
            else if (punchCount == 3)
            {
                playButtonSprite.sprite = playButton3;

                punch.Play();

                fishermanAnimator.SetBool("Punched", true);

                waveTransitionRB.simulated = true;
                waveTransitionAudio.Play();

                Debug.Log("NEXT SCENE");
            }

            cooldownTimerValue = cooldownTimerStart;
            cooldownComplete = false;
        }
    }
}
