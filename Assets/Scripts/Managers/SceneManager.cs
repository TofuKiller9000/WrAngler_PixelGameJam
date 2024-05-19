using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{

    [Header("Scene Transitions")]
    //[SerializeField] private Material sceneTransistion;
    [SerializeField] private GameObject transistionObject;
    //[SerializeField] private float transistionTime = 1f;

    //[Header("Camera")]
    //[SerializeField] private Transform fishingCameraPosition;
    //[SerializeField] private Transform fightingCameraPosition; 

    [Space]
    [Header("Game Modes")]
    [SerializeField] private GameObject fishing;
    [SerializeField] private GameObject fighting;

    private Camera _camera;

    public bool inTransistion = false;
    private Animator _transistionAnimator;
    private string activeGameMode = "Fishing";

    private void Awake()
    {
        _camera = Camera.main;
        _transistionAnimator = transistionObject.GetComponent<Animator>();
        if(_transistionAnimator == null)
        {
            Debug.LogError("ERROR: transistionObject does not have an animator component on it");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transistionObject.SetActive(false);
        //_camera.transform.position = fishingCameraPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTransistion(float startValue, float endValue, string gameMode)
    {
        //StartCoroutine(ShaderTransistionCoroutine(startValue, endValue, gameMode));
        inTransistion = true;
        transistionObject.SetActive(true);
        activeGameMode = gameMode;
        //StartCoroutine(WaitUntilEndOfAnimation());
    }

    public void SetGameMode()
    {
        if (activeGameMode.Contains("Fishing"))
        {
            
            //activate Fishing stuff
            fighting.SetActive(false);
            fishing.SetActive(true);
            //yield return new WaitForSeconds(2);
            //_camera.gameObject.transform.position = fishingCameraPosition.position;
            RadioManager.instance.StopFightMusic();
            RadioManager.instance.ResumeAllStations();

        }
        else if (activeGameMode.Contains("Fighting"))
        {
            //activate fighting stuff
            fishing.SetActive(false);
            fighting.SetActive(true);
            //yield return new WaitForSeconds(0.25f);
            //_camera.gameObject.transform.position = fightingCameraPosition.position;
        }
    }

    IEnumerator WaitUntilEndOfAnimation()
    {
        Debug.Log("Wait until Transistion is complete " + Time.time);
        while (_transistionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        Debug.Log("transistion is Complete: " + Time.time);
        inTransistion = false; 
    }

    public void ActivateSwapModes(string gameMode)
    {

        transistionObject.SetActive(false);
        if (gameMode.Contains("Fishing"))
        {
            //activate Fishing stuff
            fighting.SetActive(false);
            fishing.SetActive(true);
            //yield return new WaitForSeconds(2);
           // _camera.gameObject.transform.position = fishingCameraPosition.position;
            RadioManager.instance.StopFightMusic();
            RadioManager.instance.ResumeAllStations();

        }
        else if (gameMode.Contains("Fighting"))
        {
            //activate fighting stuff
            fishing.SetActive(false);
            fighting.SetActive(true);
            //yield return new WaitForSeconds(0.25f);
            //_camera.gameObject.transform.position = fightingCameraPosition.position;
        }
        //StartCoroutine(SwapModes(gameMode));
    }

    private IEnumerator SwapModes(string gameMode)
    {
        transistionObject.SetActive(false);
        if (gameMode.Contains("Fishing"))
        {
            //activate Fishing stuff
            fighting.SetActive(false);
            fishing.SetActive(true);
            yield return new WaitForSeconds(2);
            //_camera.gameObject.transform.position = fishingCameraPosition.position;
            
        }
        else if (gameMode.Contains("Fighting"))
        {
            //activate fighting stuff
            fishing.SetActive(false);
            fighting.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            //_camera.gameObject.transform.position = fightingCameraPosition.position;
        }
    }

    //private IEnumerator ShaderTransistionCoroutine(float startValue, float endValue, string gameMode)
    //{
    //    Debug.Log("In Transistion Coroutine w/ gameMode");
    //    sceneTransistion.SetFloat("_Progress", startValue);
    //    transistionObject.SetActive(true);
    //    float elapsedTime = 0f; 

    //    while(elapsedTime < transistionTime)
    //    {
    //        //print("I'm in the while loop pal");
    //        float t = elapsedTime / transistionTime; 
    //        float newVal = Mathf.Lerp(startValue, endValue, t);

    //        sceneTransistion.SetFloat("_Progress", newVal);

    //        elapsedTime += Time.deltaTime; 
    //        yield return null;
    //    }

    //    sceneTransistion.SetFloat("_Progress", endValue);
    //    //Debug.Log("Wait 2 seconds : " + Time.time);
    //    yield return new WaitForSeconds(0.25f);
    //    //Debug.Log("End of Transistion: " + Time.time);

    //    if (endValue == 1)
    //    {
    //        transistionObject.SetActive(false);
    //    }
    //    //Debug.Log("Wait 2 seconds : " + Time.time);
    //    //yield return new WaitForSeconds(2);
    //    //Debug.Log("End of Transistion: " + Time.time);
    //    if (gameMode.Contains("Fishing"))
    //    {
    //        //activate Fishing stuff
    //        fighting.SetActive(false);
    //        fishing.SetActive(true);
    //        _camera.gameObject.transform.position = fishingCameraPosition.position;
    //    }
    //    else if(gameMode.Contains("Fighting"))
    //    {
    //        //activate fighting stuff
    //        fishing.SetActive(false);
    //        fighting.SetActive(true);
    //        _camera.gameObject.transform.position = fightingCameraPosition.position;
    //    }
    //}

    //private IEnumerator ShaderTransistionCoroutine(float startValue, float endValue)
    //{
    //    inTransistion = true; 
    //    Debug.Log("In Transistion Coroutine  " + inTransistion);
    //    sceneTransistion.SetFloat("_Progress", startValue);
    //    transistionObject.SetActive(true);
    //    float elapsedTime = 0f;

    //    while (elapsedTime < transistionTime)
    //    {
    //        float t = elapsedTime / transistionTime;
    //        float newVal = Mathf.Lerp(startValue, endValue, t);

    //        sceneTransistion.SetFloat("_Progress", newVal);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    sceneTransistion.SetFloat("_Progress", endValue);
    //    transistionObject.SetActive(false);
    //    Debug.Log("Wait 2 seconds : " + Time.time);
    //    yield return new WaitForSeconds(2);
    //    Debug.Log("End of Transistion: " + Time.time);
    //    inTransistion = false; 
    //}
}
