using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats player = other.GetComponent<CharacterStats>() ?? null;
        AbstractEnemyAgent enemy = other.GetComponent<AbstractEnemyAgent>() ?? null;
        if (player != null)
        {
            player.TakeDamage(Mathf.Infinity);
        }

        if(enemy != null)
        {
            enemy.TakeDamage(Mathf.Infinity);
        }
    }
}
