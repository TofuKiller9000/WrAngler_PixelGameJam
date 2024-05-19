using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class FishBase : MonoBehaviour, IFish
{


    #region Variables
    public string fishName;
    public string fishDescription;
    public int spawnChance;
    public int health;
    public Sprite healthySprite, hurtSprite, defeatedSprite; 
    public Color _flashColor = Color.white;
    public float _flashTime = 0.25f; 
    private Animator _animator; 
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    #endregion



    #region Interface

    public string FishName => fishName;

    public string FishDescription => fishDescription;

    public int SpawnChance => spawnChance;

    public int Health => health;

    public Color FlashColor => _flashColor;

    public float FlashTime => _flashTime;   

    public Sprite HealthySprite => healthySprite;

    public Sprite HurtSprite => hurtSprite;

    public Sprite DefeatedSprite => defeatedSprite;

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = GetComponent<Material>();

        _material.SetColor("_FlashColor", _flashColor);
        _material.SetFloat("_FlashAmount", 0);
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

    public void ActivateTakeDamage()
    {
        StartCoroutine(TakeDamage());
        //put things here like special effects and particle effects for whenever we damage a fish
    }

    IEnumerator TakeDamage()
    {
        float currentFlashAmount = 0f;
        float elapsedTime = 0f; 

        while(elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0, elapsedTime / _flashTime);
            _material.SetFloat("_FlashAmount", currentFlashAmount);
            yield return null;
        }
    }

    public void SetSpriteState(string state)
    {
        switch (state)
        {
            case ("Healthy"):
                _spriteRenderer.sprite = healthySprite;
                break;
            case ("Hurt"):
                _spriteRenderer.sprite = hurtSprite; 
                break;
            case ("Defeated"):
                _spriteRenderer.sprite = defeatedSprite;
                break;
            default:
                _spriteRenderer.sprite = healthySprite;
                break; 
        }
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
