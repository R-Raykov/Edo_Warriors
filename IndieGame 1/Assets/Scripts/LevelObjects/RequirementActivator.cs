using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RequirementActivator : MonoBehaviour
{
    public UnityEvent OnFulfill;

	// Use this for initialization
	void Start ()
    {
        GetComponentInParent<AbstractEnemyAgent>().OnDeath += Fulfill;
	}
	

    private void Fulfill(AbstractEnemyAgent p)
    {
        if (OnFulfill != null) OnFulfill.Invoke();
    }
 
    private void OnDestroy()
    {
        GetComponentInParent<AbstractEnemyAgent>().OnDeath -= Fulfill;
    }
}
