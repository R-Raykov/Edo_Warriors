using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        Rigidbody rb = col.GetComponent<Rigidbody>();
        if (rb != null && col.GetComponent<CharacterStats>() == null)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if(enemy != null) enemy.OnCrowdControlled.Invoke();

            rb.AddForce (transform.forward * 7.5f * rb.mass, ForceMode.Impulse);
        }
    }
}
