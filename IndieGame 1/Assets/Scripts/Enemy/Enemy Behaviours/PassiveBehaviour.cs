using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveBehaviour : MonoBehaviour
{
    [Header("Movement values")]
    [SerializeField] protected NavAgentValues _navAgentValues;

    protected NavMeshAgent _navAgent;

    protected virtual void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }

    public void SetNavValues()
    {
        _navAgent.speed = _navAgentValues.speed;
        _navAgent.stoppingDistance = _navAgentValues.stoppingDistance;
        _navAgent.angularSpeed = _navAgentValues.angularSpeed;
        _navAgent.acceleration = _navAgentValues.acceleration;
    }

    public virtual void UpdateAgent()
    {

    }
}
