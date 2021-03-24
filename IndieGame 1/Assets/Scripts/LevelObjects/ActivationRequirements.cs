using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivationRequirements : MonoBehaviour
{
    [SerializeField] private int _numberOfRequirements = 1;
    [SerializeField] private int _requirementsFulfilled = 0;
    private IActivatable _activatable;

    private void Start()
    {
        _activatable = GetComponent<IActivatable>();
    }

    public void FulfillOne()
    {
        _requirementsFulfilled++;
        if(_requirementsFulfilled >= _numberOfRequirements)
        {
            if(_activatable != null) _activatable.Activate();
        }
    }
}
