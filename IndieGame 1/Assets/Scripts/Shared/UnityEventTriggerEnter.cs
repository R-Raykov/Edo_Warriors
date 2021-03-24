using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventTriggerEnter : MonoBehaviour {

    public UnityEvent onTriggerEnter;
    public Collider Trigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CharacterStats>() != null)
        {
            if (onTriggerEnter != null) onTriggerEnter.Invoke();
        }
    }
}
