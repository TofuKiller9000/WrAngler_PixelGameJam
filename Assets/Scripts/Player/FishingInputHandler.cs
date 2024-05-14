using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class FishingInputHandler : MonoBehaviour
{

    [SerializeField] private Animator fishingPoleAnim;
    [Space]
    [SerializeField] private List<GameObject> spawnableFish;
    [SerializeField] private List<GameObject> shuffledFish;

    [SerializeField] private Transform fishSpawnPoint; 

    [Space]
    [SerializeField] private float maxFishingWaitTime;

    [SerializeField] private int skillCheckValue;
    private bool isFishing;
    private bool fishOnLine;

    private float timeTillSkillCheck;
    [SerializeField] private float timer = 0; 
    // Start is called before the first frame update
    void Start()
    {
        isFishing = false;
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
    }

    public void OnClick(InputAction.CallbackContext context)
    {

        if(context.canceled) 
        {
            if (!isFishing)
            {
                print("Cast Line");
                shuffledFish = Shuffle<GameObject>(spawnableFish);
                fishingPoleAnim.SetTrigger("Cast");
                fishingPoleAnim.SetBool("IsFishing", true);
                isFishing = true;
                timeTillSkillCheck = Random.Range(1, maxFishingWaitTime);
            }
            else if (isFishing && fishOnLine)
            {
                //activate fighting mode
            }
            else if (isFishing)
            {
                print("Reel in");
                //reel back in, didn't catch a fish
                isFishing = false;
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
                //fishOnLine = true;
                Instantiate(shuffledFish[i], fishSpawnPoint);
                isFishing = false;
                fishingPoleAnim.SetBool("IsFishing", false);
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
        yield return new WaitForSeconds(time);
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
