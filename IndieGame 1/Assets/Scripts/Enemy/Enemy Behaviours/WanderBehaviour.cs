using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class WanderBehaviour : PassiveBehaviour
{
    [SerializeField] private float _wanderStepDistance = 0.5f;
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    public override void UpdateAgent()
    {
        Debug.Assert(_navAgent != null);
        if (!_navAgent.pathPending && _navAgent.remainingDistance < 0.1f)
        {
            _navAgent.SetDestination(recalcWander());
            StartCoroutine(wanderInterval());
        }
    }

    private Vector3 recalcWander()
    {
        Vector3 targetDir = Quaternion.AngleAxis(Random.Range(-60f, 60f), transform.up) * transform.forward;
        targetDir = targetDir.normalized * _wanderStepDistance + transform.position;
        NavMeshHit hit;

        if (NavMesh.Raycast(transform.position, targetDir, out hit, NavMesh.AllAreas))
        {
            targetDir = Quaternion.AngleAxis(Random.Range(120f, 240f), transform.up) * transform.forward;
            targetDir = targetDir.normalized * _wanderStepDistance + transform.position;
        }
        return targetDir;
    }

    private IEnumerator wanderInterval()
    {
        _enemy.ShouldWander = false;
        yield return new WaitForSeconds(Random.Range(1.5f, 5f));
        _enemy.ShouldWander = true;
    }
}
