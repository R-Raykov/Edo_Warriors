using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitBox : MonoBehaviour
{
    private float _damage;

    private void Start()
    {
        Enemy parent = GetComponentInParent<Enemy>();
        if (parent != null)
            _damage = GetComponentInParent<Enemy>().Damage;
        else
        {
            _damage = 0;
            Debug.LogError("Component needs to be a child of an object with an Enemy component. Damage set to default 0. ", this);
        }
    }

    // Checks if theres a hit with a player and damages them
    private void OnTriggerEnter(Collider other)
    {
        CharacterStats player = other.GetComponent<CharacterStats>();
        
        if(player != null)
        {
            player.TakeDamage(_damage);
        }
    }

    /// <summary>
    /// Gets and sets the damage, set by the enemy class at instantiation
    /// </summary>
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
}
