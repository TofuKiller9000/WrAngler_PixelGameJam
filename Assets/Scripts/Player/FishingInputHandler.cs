using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class FishingInputHandler : MonoBehaviour
{

    public Animator fishingPoleAnim;
    [Space]
    public List<GameObject> spawnableFish;
    public List<GameObject> shuffledFish;

    public Transform fishSpawnPoint; 

    [Space]
    public float maxFishingWaitTime;

    public int skillCheckValue;
    private bool isFishing;
    private bool fishOnLine;

    public float timeTillSkillCheck;
    public float timer = 0;
    public GameObject bobber;
    public GameObject fishingNotif;
    public float responseTime;
    private float responseTimer;
    private GameObject currentFishOnLine; 
    // Start is called before the first frame update
    void Start()
    {
        isFishing = false;
        bobber.SetActive(false);
        fishingNotif.SetActive(false);
        timer = 0; 
    }

    // Update is called once per frame
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

        if(fishOnLine)
        {
            if(responseTimer >= responseTime)
            {
                responseTimer -= Time.deltaTime; 
            }
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {

        if(context.canceled) 
        {
            if (!isFishing)
            {
                shuffledFish = Shuffle<GameObject>(spawnableFish); //Shuffle our fish
                fishingPoleAnim.SetTrigger("Cast");
                fishingPoleAnim.SetBool("IsFishing", true);
            }
            else if (isFishing && fishOnLine)
            {
                //activate fighting mode
                if(responseTimer >= responseTime)
                {
                    //successfully caught fish
                    print("Fight Time!");
                    Instantiate(currentFishOnLine, fishSpawnPoint);
                    fishOnLine = false; 
                }
                else
                {
                    //didn't react quickly enough. 
                    print("Reel in");
                    bobber.SetActive(false);
                    //reel back in, didn't catch a fish
                    isFishing = false;
                    timer = 0;
                    fishingPoleAnim.SetBool("IsFishing", false);
                }
            }
            else if (isFishing)
            {
                print("Reel in");
                bobber.SetActive(false);
                //reel back in, didn't catch a fish
                isFishing = false;
                timer = 0; 
                fishingPoleAnim.SetBool("IsFishing", false);
            }
        }

    }

    private void SkillCheck()
    {
        skillCheckValue = Random.Range(0, 101);
        for (int i = 0; i < shuffledFish.Count; i++)
        {
            if(shuffledFish[i].GetComponent<FishBase>().SpawnChance >= skillCheckValue)
            {
                print("You have a bite on the line!");
                fishOnLine = true;
                fishingNotif.SetActive(true);
                currentFishOnLine = shuffledFish[i];
                //isFishing = false;
                fishingPoleAnim.SetBool("IsFishing", false);
                responseTimer = responseTime;
                break; 
            }
            else
            {
                print("No fish on the line, wait a bit longer");
                timeTillSkillCheck = Random.Range(1, maxFishingWaitTime);
                StartCoroutine(FishingWait(timeTillSkillCheck));
            }
        }

        timeTillSkillCheck = Random.Range(1, maxFishingWaitTime);
        timer = 0; 
    }

    IEnumerator FishingWait(float time)
    {
        print("FishingWait");
        yield return new WaitForSeconds(time);
    }

    public void LineCast()
    {
        print("Line has been cast, bobber is in the water");
        bobber.SetActive(true);
        timeTillSkillCheck = Random.Range(1, maxFishingWaitTime);
        isFishing = true;
    }

    public List<T> Shuffle<T>(List<T> fishList)
    {
        List<T> outputList = new List<T>();
        
        foreach(T t in fishList) 
        {
            int position = Random.Range(0, outputList.Count);
            outputList.Add((position == outputList.Count) ? t : outputList[position]);
            outputList[position] = t; 
        }
        return outputList; 
    }
}
