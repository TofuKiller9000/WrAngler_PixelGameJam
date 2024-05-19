using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBase : MonoBehaviour, IFish
{


    #region Variables
    public string fishName;
    public string fishDescription;
    public int spawnChance;
    public int health;
    private Animator _animator; 
    #endregion



    #region Interface

    public string FishName => fishName;

    public string FishDescription => fishDescription;

    public int SpawnChance => spawnChance;

    public int Health => health;

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void DestroyFish()
    {
        Destroy(gameObject);
    }

    public void FadeAway()
    {
        _animator.SetTrigger("Fade");
        StartCoroutine(AnimationWait("FishFadeAway_anim"));
    }

    IEnumerator AnimationWait(string animName)
    {
        while(_animator.GetCurrentAnimatorStateInfo(0).IsName(animName) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        Debug.Log(animName + " has finished playing!");

    }

}
