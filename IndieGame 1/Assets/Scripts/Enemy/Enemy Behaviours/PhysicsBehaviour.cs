using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PhysicsBehaviour : MonoBehaviour
{
    [SerializeField] private float _ragdollTime = 2f;

    private NavMeshAgent _navAgent;
    private Rigidbody _rb;
    private Enemy _enemy;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _enemy.OnCrowdControlled += enablePhysics;
    }

    private void enablePhysics()
    {
        _rb.isKinematic = false;
        _navAgent.enabled = false;

        StartCoroutine(pushTimer());
    }

    private IEnumerator pushTimer()
    {
        yield return new WaitForSeconds(_ragdollTime);

        if (IsAgentOnNavMesh(this.gameObject))
        {
            _rb.isKinematic = true;
            _navAgent.enabled = true;
        }
        else
        {
            _rb.freezeRotation = false;
        }
    }

    public bool IsAgentOnNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        float onMeshThreshold = 3f;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }

    private void OnDisable()
    {
        _enemy.OnCrowdControlled -= enablePhysics;
    }
}
