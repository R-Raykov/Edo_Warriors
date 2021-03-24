using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDmg : MonoBehaviour {

    [Tooltip("Damage of the ablility")]
    [SerializeField ]private float _damage;

    [Tooltip("Damage of the ablility")]
    [SerializeField] private float _physicalAmplifier;

    [Tooltip("Damage of the ablility")]
    [SerializeField] private float _magicalAmplifier;


    public enum Type {StayDmg, BurstDmg};
    public Type DmgType;

    public CharacterStats charStats; 

    public float Damage
    {
        get { return _damage /*+ _magicalAmplifier * charStats.MagicPower + _physicalAmplifier * charStats.AttackPower*/; }
    }

}
