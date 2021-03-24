using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : MonoBehaviour {

	private void Start ()
    {
		
	}
	
	private void Update ()
    {
		
	}

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Enemy") && gameObject.GetComponent<AbilityDmg>() != null)
        {
           col.GetComponent<AbstractEnemyAgent>().TakeDamage(gameObject.GetComponent<AbilityDmg>().Damage);
        }
    }
}
