using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [Header("Movement values")]
    [SerializeField] protected NavAgentValues _navAgentValues;

    [SerializeField] protected float _animationLength = 0.5f;
    [SerializeField] protected float _attackCooldown = 0.5f;

    protected NavMeshAgent _navAgent;
    protected Enemy _enemy;

    // Use this for initialization
    void Start ()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();
    }

    public void SetNavValues()
    {
        _navAgent.speed = _navAgentValues.speed;
        _navAgent.stoppingDistance = _navAgentValues.stoppingDistance;
        _navAgent.angularSpeed = _navAgentValues.angularSpeed;
        _navAgent.acceleration = _navAgentValues.acceleration;
    }

    public virtual void Chase(Transform target)
    {
        // Get overriden boi
    }

    public virtual void UpdateTarget(Transform target) 
    {
        // Get overriden boi
    }

    public virtual void AttackTarget()
    {
        // Get overriden boi
    }

    protected virtual IEnumerator attack()
    {
        yield return new WaitForSeconds(_animationLength);
    }

    protected virtual IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
    }
}
