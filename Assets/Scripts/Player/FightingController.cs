using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FightingController : MonoBehaviour
{

    [Header("Fighting Properties")]
    [SerializeField] private float punchDelay;
    [SerializeField] private float roundTime;
    [Space]
    [SerializeField] private int punchDamage = 1;
    [SerializeField] private int activeFishHealth;
    [Space]
    [SerializeField] private bool winState;
    [SerializeField] private Transform fightingPlayerFishHolder;
    [SerializeField] private Transform fishingPlayerFishHolder; 

    [Space]
    [SerializeField] private int countDownValue;


    [Space]
    [Header("Audio Properties")]
    [SerializeField] private AudioClip punchSoundEffect;
    [SerializeField] private AudioClip finalPunchSoundEffect;
    [SerializeField] private AudioClip underwaterAmbiance; 
    

    [Header("Animations & UI")]
    [SerializeField] private Image timerForeground;
    [SerializeField] TextMeshProUGUI countDownDisplay; 

    [Space]
    [Header("Scripts & Components")]
    [SerializeField] private SceneManager sceneManager;

    private PlayerInput _playerInput; 
    private GameObject _activeFish; 
    private float _punchTimer;
    private float _roundTimer; 
    private Animator _animator;
    private bool isRoundActive;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _playerInput = GetComponent<PlayerInput>();

    }


    private void OnEnable()
    {
        Debug.Log("Fighting Controller Enabled");
        _activeFish = fightingPlayerFishHolder.transform.GetChild(0).gameObject;
        if (_activeFish == null)
        {
            Debug.LogError("FISH NOT FOUND");
        }
        else
        {
            activeFishHealth = _activeFish.GetComponent<FishBase>().health;
        }
        _playerInput.enabled = false;
        _roundTimer = roundTime;
        timerForeground.fillAmount = 1;
        isRoundActive = false;
        var timerParent = timerForeground.transform.parent.gameObject;
        timerParent.SetActive(true);
        _punchTimer = punchDelay;
        winState = false;
        sceneManager.ActivateTransistion(0, 1);
        StartRound();
    }

    public void OnPunch(InputAction.CallbackContext context)
    {
        if(context.started && _punchTimer > 0 && isRoundActive) //
        {
            //print("Punch!");
            _punchTimer = punchDelay;
            _animator.SetTrigger("Punch"); //Triggers the DamageFish() function as an animation event
        }
        else if(_punchTimer <= 0)
        {
            Debug.Log("punchTimer: " +  _punchTimer + " isRoundActive?: " + isRoundActive + " Context: " + context.phase);
        }
    }

    public void DamageFish()
    {
        print("Damage fish");
        if(activeFishHealth > 0)
        {
            RadioManager.instance.PlaySoundEffect(punchSoundEffect);
            _activeFish.GetComponent<FishBase>().ActivateTakeDamage();
            activeFishHealth -= punchDamage; 
        }
        else
        {
            RadioManager.instance.PlaySoundEffect(finalPunchSoundEffect);
            winState = true; 
            EndRound();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(isRoundActive)
        {
            if (_punchTimer > 0.01)
            {
                _punchTimer -= Time.deltaTime;
            }

            if(_roundTimer <= 0.01)
            {
                EndRound();
            }
            else
            {
                _roundTimer -= Time.deltaTime;
                timerForeground.fillAmount = _roundTimer / roundTime; 
            }
        }
    }

    private void EndRound()
    {
        countDownDisplay.gameObject.SetActive(false);
        isRoundActive = false;
        _roundTimer = roundTime; 

        if(_playerInput != null)
        {
            _playerInput.enabled = false;
        }

        if(winState == true)
        {
            print("You defeated the fish!!");
        }
        else
        {
            _activeFish.GetComponent<FishBase>().FadeAway();
            print("You didn't defeat the fish");
        }
        Debug.Log("Activating Leave Animation in Fighting Controller");
        _animator.SetTrigger("Leave");
        //additional animations & special effects can go here to signify the end of the round
    }

    public void PlayerLeaves()
    {
        if(winState)
        {
            Debug.Log("Leaving with Fish");
            _activeFish.transform.SetParent(fishingPlayerFishHolder.gameObject.transform); //set the new parent of the active fish to 
            _activeFish.transform.localPosition = Vector3.zero;
            _activeFish.transform.SetSiblingIndex(0);
        }
        else
        {
            Debug.Log("Leaving without Fish");
        }

        var timerParent = timerForeground.transform.parent.gameObject;
        timerParent.SetActive(false);
        //sceneManager.ActivateTransistion(1, 0, "Fishing");
        sceneManager.ActivateSwapModes("Fishing");
    }

    public void StartRound()
    {
        RadioManager.instance.PlayFightMusic();
        RadioManager.instance.PlayAmbience(underwaterAmbiance);
        StartCoroutine(CountDownCoroutine());
    }

    private IEnumerator CountDownCoroutine()
    {
        Debug.Log("Wait until SceneManager Coroutines are done : " + Time.time);
        yield return new WaitUntil(() => sceneManager.inTransistion == false);
        Debug.Log("Scene Manager finished: " + Time.time + sceneManager.inTransistion);
        countDownDisplay.gameObject.SetActive(true);
        int currentTime = countDownValue; 

        while( currentTime > 0 )
        {
            countDownDisplay.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);

            currentTime--; 
        }

        countDownDisplay.text = "WRANGLE TIME!!";
         yield return new WaitForSeconds(1f);

        countDownDisplay.text = "";
        isRoundActive = true;
        _playerInput.enabled = true; 
    }
}
