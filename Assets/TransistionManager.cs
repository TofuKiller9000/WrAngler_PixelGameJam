using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransistionManager : MonoBehaviour
{
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private AudioClip bubbles; 

    private void Start()
    {
        //RadioManager.instance.PlaySoundEffect(bubbles);
    }

    public void ChangeModes()
    {
        sceneManager.SetGameMode();
    }


    public void SetInactive()
    {
        Debug.Log("Transistion set to Inactive");
        gameObject.SetActive(false);
        sceneManager.inTransistion = false;
    }

}
