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
    [SerializeField] private Transform fishSpawnPoint;
    [SerializeField] private GameObject bobber;
    [Space]
    private bool removeFishWhenCaught = false;

    [Space]
    [Header("Animation & UI")]
    [SerializeField] private Animator FishingAnimator;
    [SerializeField] private GameObject fishingNotif;

    [Space]
    [Header("Fish")]
    public List<GameObject> spawnableFish;



    #endregion

    #region Private
    private bool isFishing;
    private bool fishOnLine;
    private bool caughtFish;
    private bool winGameState;
    private int skillCheckValue;
    private float timer = 0;
    private float timeTillSkillCheck;
    private float responseTimer;
    private GameObject currentFishOnLine;
    private List<GameObject> shuffledFish;
    #endregion
    
    

    void Start()
    {
        isFishing = false;
        bobber.SetActive(false);
        fishingNotif.SetActive(false);
        timer = 0;
        caughtFish = false;
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
            }
        }
    }


    #region Input
    public void OnClick(InputAction.CallbackContext context)
    {

        if (context.canceled && !winGameState)
        {
            //print("Click!");
            if (!isFishing)
            {
                shuffledFish = Shuffle<GameObject>(spawnableFish); //Shuffle our fish
                FishingAnimator.SetTrigger("Cast");
                FishingAnimator.SetBool("IsFishing", true);
            }
            else if (isFishing && fishOnLine)
            {
                print("You have a fish on the line and are trying to reel it in");
                if (responseTimer > 0.01)
                {
                    CatchFish();

                }
                else
                {
                    ReelIn();
                }
            }
            else if (isFishing)
            {
                ReelIn();
            }
        }

    }

    #endregion

    #region Fishing Functions
    private void SkillCheck()
    {
        print("In Skill Check");
        skillCheckValue = Random.Range(0, 101);
        for (int i = 0; i < shuffledFish.Count; i++)
        {
            if (shuffledFish[i].GetComponent<FishBase>().SpawnChance >= skillCheckValue)
            {
                print("You have a bite on the line!");
                fishOnLine = true;
                fishingNotif.SetActive(true);
                currentFishOnLine = shuffledFish[i];
                //isFishing = false;
                //FishingAnimator.SetBool("IsFishing", false);
                responseTimer = responseTime;
                print("fishOnLine: " + fishOnLine);
                break;
            }
            //else
            //{
            //    //print("i: " + i);
            //}
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
        timer = 0;
    }

    private void CatchFish()
    {
        //successfully caught fish
        print("Fight Time!");
        Instantiate(currentFishOnLine, fishSpawnPoint);
        FishingAnimator.SetBool("IsFishing", false);
        bobber.SetActive(false);
        fishingNotif.SetActive(false);
        caughtFish = true;
        fishOnLine = false;
        isFishing = false;
        timer = 0;
        responseTimer = responseTime;
        if(removeFishWhenCaught)
        {

            RemoveFishFromList(currentFishOnLine.GetComponent<FishBase>().fishName);
        }

        if(spawnableFish.Count == 0)
        {
            print("You've caught all the fish!");
            //here we should subscribe to a win state in a game manager 
            winGameState = true; 
        }
        else
        {
            print("There are " + spawnableFish.Count + " fish remaining to catch!");
        }
    }

    void RemoveFishFromList(string fishName)
    {
        int fishToRemoveIndex = 0;

        for (int i = 0; i < spawnableFish.Count; i++)
        {
            if (spawnableFish[i].GetComponent<FishBase>().FishName.Contains(fishName))
            {
                fishToRemoveIndex = i;
            }
        }
        if (spawnableFish.Count == 1)
        {
            spawnableFish.Clear();
        }
        else
        {
            spawnableFish.RemoveAt(fishToRemoveIndex);
        }

    }

    public void LineCast()
    {
        print("Line has been cast, bobber is in the water");
        bobber.SetActive(true);
        timeTillSkillCheck = Random.Range(1, maxFishingWaitTime);
        isFishing = true;
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

}
