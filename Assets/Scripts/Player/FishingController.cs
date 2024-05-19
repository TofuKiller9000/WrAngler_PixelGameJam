using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingController : MonoBehaviour
{

    #region Properties

    [Header("Fishing Properties")]
    [SerializeField] private float maxFishingWaitTime;
    [SerializeField] private float responseTime;
    [Space]
    [SerializeField] private Transform fightingPlayerFishHolder;
    [SerializeField] private Transform fishingPlayerFishHolder; 
    [SerializeField] private GameObject bobber;

    [Space]
    [Header("Animation & UI")]
    [SerializeField] private Animator FishingAnimator;
    [SerializeField] private GameObject fishingNotif;

    [Space]
    [Header("Audio Properties")]
    [SerializeField] private AudioClip diveSoundEffect;
    [SerializeField] private AudioClip castSoundEffect;
    [SerializeField] private AudioClip bobberSoundEffect;
    [SerializeField] private AudioClip biteOnLineSoundEffect;
    [SerializeField] private AudioClip beachAmbiance;
    [SerializeField] private AudioClip sitDownSoundEffect; 

    [Header("Scripts")]
    [SerializeField] private SceneManager sceneManager;

    [Space]
    [Header("Fish")]
    public List<GameObject> spawnableFish;
    public GameObject[] hooks = new GameObject[6]; 


    #endregion

    #region Private
    private bool isFishing;
    private bool fishOnLine;
    private bool winGameState;
    private bool firstLoadIn = true;
    private int skillCheckValue;
    private float timer = 0;
    private float timeTillSkillCheck;
    private float responseTimer;
    public Vector3 _defaultPosition; 
    private GameObject currentFishOnLine;
    private List<GameObject> shuffledFish;
    private PlayerInput _playerInput;
    private FishBase defeatedFish;
     
    #endregion

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        if( _playerInput == null )
        {
            Debug.LogError("UNABLE TO FIND PLAYERINPUT ON FISHING CONTROLLER");
        }
        isFishing = false;
        bobber.SetActive(false);
        fishingNotif.SetActive(false);
        timer = 0;
        fishOnLine = false;
        _defaultPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        Debug.Log("Player local position: " + gameObject.transform.localPosition);
        gameObject.transform.localPosition = _defaultPosition;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        Debug.Log("Player local position: " +  gameObject.transform.localPosition);
        Debug.Log("Fishing Controller Enabled");
        int fishCount = fishingPlayerFishHolder.transform.childCount;

        if(firstLoadIn)
        {
            Debug.Log("First Time Load In");
            RadioManager.instance.PlayAmbience(beachAmbiance);
            _playerInput.enabled = true;
            //here we would put the tutorial screen
        }
        else
        {
            if(fishCount > 0 )
            {
                Debug.Log("Not First Time, and we have a fish");
                var tempFish = fishingPlayerFishHolder.transform.GetChild(0);
                _playerInput.enabled = false;
                defeatedFish = null;
                defeatedFish = tempFish.GetComponent<FishBase>();
                if (defeatedFish == null)
                {
                    Debug.LogError("UNABLE TO FIND FISHBASE ON DEFEATED FISH");
                }
                StartCoroutine(FishingRoundStart());
            }
            else
            {
                StartCoroutine(FishingRoundStart());
                //_playerInput.enabled = false;
                //isFishing = false;
                //bobber.SetActive(false);
                //fishingNotif.SetActive(false);
                //timer = 0;
                //fishOnLine = false;
                //FishingAnimator.SetTrigger("Return");
                //RadioManager.instance.StopAmbience();
                //RadioManager.instance.PlayAmbience(beachAmbiance);
                //_playerInput.enabled = true;
            }
        }
    }

    private void OnDisable()
    {
        Debug.Log("Player local position: " + gameObject.transform.localPosition);
        gameObject.transform.localPosition = _defaultPosition;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Debug.Log("Player local position: " + gameObject.transform.localPosition);
    }

    void Update()
    {
        if (isFishing && timer < timeTillSkillCheck && !fishOnLine)
        {
            timer += Time.deltaTime;
        }
        if (timer >= timeTillSkillCheck && isFishing && !fishOnLine)
        {
            //Perform skillcheck
            SkillCheck();
        }

        if (fishOnLine)
        {
            if (responseTimer > 0.01)
            {
                responseTimer -= Time.deltaTime;
            }
            else
            {
                fishingNotif.SetActive(false);
                ReelIn();
            }
        }
    }


    #region Input
    public void OnClick(InputAction.CallbackContext context)
    {

        if (context.canceled && !winGameState)
        {
            //print("Click!");
            if (!isFishing) //we haven't cast our line yet
            {
                shuffledFish = Shuffle<GameObject>(spawnableFish); //Shuffle our fish
                FishingAnimator.SetTrigger("Cast"); //An animation event triggers the LineCast() function
                FishingAnimator.SetBool("IsFishing", true);
            }
            else if (isFishing && fishOnLine) //we have already casted our line, and we have a fish on the line, so we want to reel it in
            {
                if (responseTimer > 0.01) //if our responseTimer is not at or less than 0, we have successfully caught the fish
                {
                    CatchFish();

                }
                else
                {
                    ReelIn();
                }
            }
            else if (isFishing) //we have already cast our line, and there is no fish on the line, so we just want to reel it in. 
            {
                ReelIn();
            }
        }

    }

    #endregion

    #region Fishing Functions
    private void SkillCheck()
    {
        //print("In Skill Check");
        skillCheckValue = Random.Range(0, 101);
        for (int i = 0; i < shuffledFish.Count; i++)
        {
            if (shuffledFish[i].GetComponent<FishBase>().SpawnChance >= skillCheckValue)
            {
                //print("You have a bite on the line!");
                fishOnLine = true;
                fishingNotif.SetActive(true);
                FishingAnimator.SetBool("FishOnLine", true);
                RadioManager.instance.PlaySoundEffect(biteOnLineSoundEffect);
                currentFishOnLine = shuffledFish[i];
                responseTimer = responseTime;
                break;
            }
        }

        timeTillSkillCheck = Random.Range(1, maxFishingWaitTime);
        timer = 0;
    }

    private void ReelIn()
    {
        //reel back in, didn't catch a fish
        print("Reel in, no fish");
        bobber.SetActive(false);
        fishingNotif.SetActive(false);
        FishingAnimator.SetBool("IsFishing", false);
        isFishing = false;
        fishOnLine = false;
        FishingAnimator.SetBool("FishOnLine", false);
        timer = 0;
        RadioManager.instance.PlaySoundEffect(sitDownSoundEffect);
    }

    private void CatchFish()
    {
        //successfully caught fish
        //print("Fight Time!");
        GameObject spawnedFish = Instantiate(currentFishOnLine, fightingPlayerFishHolder); //we spawn a fish on our fishSpawnPoint in the other area
        spawnedFish.GetComponent<FishBase>().SetPosition();
        FishingAnimator.SetBool("IsFishing", false);
        bobber.SetActive(false);
        fishingNotif.SetActive(false);
        fishOnLine = false;
        isFishing = false;
        timer = 0;
        responseTimer = responseTime;
        _playerInput.enabled = false; //disable player input
        FishingAnimator.SetTrigger("Diving"); //Animation event triggers DivingIn() function
    }

    public void DivingIn()
    {
        //play DiveIn Sound effect
        RadioManager.instance.PauseAllStations();
        RadioManager.instance.StopAmbience();
        RadioManager.instance.PlaySoundEffect(diveSoundEffect);
        firstLoadIn = false; 
        sceneManager.ActivateTransistion(1, 0, "Fighting"); //activate our Scene Transistion coroutine in our scene manager
    }

    public void LineCast()
    {
        //Our line has been cast, and we set our bobber to be active
        bobber.SetActive(true); //this is temporary, and is only here for testing purposes. Will be replaced by animations when they are done
        timeTillSkillCheck = Random.Range(1, maxFishingWaitTime+1); //we set the time till a skill check to a random value from 1 to our maxFishing wait time
        isFishing = true;
        RadioManager.instance.PlaySoundEffect(bobberSoundEffect);
    }

    public void ShowCaughtFish()
    {
        
        if(defeatedFish != null)
        {
            print("Defeated Fish == " + defeatedFish.fishName);
            for (int i = 0; i < hooks.Length; i++)
            {
                if (defeatedFish.fishName.Contains(hooks[i].GetComponent<FishBase>().fishName))
                {
                    if (!hooks[i].gameObject.activeSelf)
                    {
                        hooks[i].SetActive(true);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            defeatedFish.DestroyFish();
            defeatedFish = null;
            

        }
        RadioManager.instance.PlaySoundEffect(sitDownSoundEffect);
        _playerInput.enabled = true;
    }


    public void PlayCastSFX()
    {
        print("Play Sound effect ");
        RadioManager.instance.PlaySoundEffect(castSoundEffect);
    }

    #endregion

    #region Generic Functions
    public List<T> Shuffle<T>(List<T> fishList)
    {
        List<T> outputList = new List<T>();

        foreach (T t in fishList)
        {
            int position = Random.Range(0, outputList.Count);
            outputList.Add((position == outputList.Count) ? t : outputList[position]);
            outputList[position] = t;
        }
        return outputList;
    }

    #endregion

    private IEnumerator FishingRoundStart()
    {
        isFishing = false;
        bobber.SetActive(false);
        fishingNotif.SetActive(false);
        timer = 0;
        fishOnLine = false;
        Debug.Log("Wait until SceneManager Coroutines are done : " + Time.time);
        yield return new WaitUntil(() => sceneManager.inTransistion == false);
        Debug.Log("Scene Manager finished: " + Time.time + sceneManager.inTransistion);
        RadioManager.instance.StopAmbience();
        RadioManager.instance.PlayAmbience(beachAmbiance);
        FishingAnimator.SetTrigger("Return");
    }
}
