using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerOptions { pulo=0, defesa=1,correr=2}


public class PowerScript : MonoBehaviour
{
    [SerializeField] PowerOptions _power;
    // [SerializeField] Sprite[] powerSpriteOptions;
    // [SerializeField] SpriteRenderer powerSprite;
    [SerializeField] private Animator powerShine;


     // [Jessé]
    private string[] animationsName = new string[3]{"PowerEffectJump",
                                                    "PowerEffectShild",
                                                    "PowerEffectSlowMotion"};

    public PowerOptions GetPower()
    {
        return _power;
    }
    private void Start()
    {
        SetPower(_power);
    }
    public void SetPower(PowerOptions powerValue)
    {
        _power = powerValue;
        // powerSprite.sprite = powerSpriteOptions[(int)_power];
        powerShine.Play(animationsName[(int)_power], -1, 0); // [Jessé]
    }

}
