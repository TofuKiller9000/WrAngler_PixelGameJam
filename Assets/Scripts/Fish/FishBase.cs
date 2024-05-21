using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBase : MonoBehaviour, IFish
{


    #region Variables
    [Header("Fish Values")]
    public string fishName;
    public string fishDescription;
    public int spawnChance;
    public int health;
    [Space]
    [Header("Fish Assets")]
    public Sprite healthySprite;
    public Sprite hurtSprite;
    public Sprite defeatedSprite;
    public Color _flashColor = Color.white;
    public float _flashTime = 0.25f;
    [Space]
    [Header("Fish Positioning")]
    public float scale = 1; 
    public Vector2 customPosition = Vector2.zero;

    private Animator _animator; 
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    #endregion



    #region Interface

    public string FishName => fishName;

    public string FishDescription => fishDescription;

    public int SpawnChance => spawnChance;

    public int Health => health;

    public Vector2 CustomPosition => customPosition;

    public float Scale => scale; 

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
        _material = GetComponent<SpriteRenderer>().material;
        if( _material != null )
        {
            _material.SetColor("_FlashColor", _flashColor);
            _material.SetFloat("_FlashAmount", 0);
        }
    }

    public void DestroyFish()
    {
        Destroy(gameObject);
    }

    public void FadeAway()
    {
        _animator.SetTrigger("Fade");
        //StartCoroutine(AnimationWait("FishFadeAway_anim"));
    }

    public void ActivateTakeDamage(int currentHealth)
    {
        StartCoroutine(TakeDamage(currentHealth));
        //put things here like special effects and particle effects for whenever we damage a fish
    }

    public void SetPosition()
    {
        transform.position = Vector2.zero; transform.rotation = Quaternion.identity;
        transform.localPosition = customPosition;
        if(scale != 0)
        {
            transform.localScale = new Vector2(scale, scale);
        }
    }

    IEnumerator TakeDamage(int currentHealth)
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

        if(currentHealth == Mathf.RoundToInt(health/2))
        {
            print(currentHealth + " " + Mathf.RoundToInt(health / 2));
            SetSpriteState("Hurt");
        }
        if(currentHealth <= 2)
        {
            SetSpriteState("Defeated");
            //_animator.SetBool("FishDead", true);
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
                Debug.Log("Sprite should now be showing hurt image");
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
