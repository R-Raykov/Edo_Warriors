using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class MeleeBehaviour : AttackBehaviour
{
    [SerializeField] private EnemyAttackHitBox _attackHitBox;
    [SerializeField] private float _targetingSpeed = 1.75f;

    private bool _updateTarget = true;

    private KappaAnimations _kappaAnime;

    private void Awake()
    {
        _kappaAnime = GetComponentInChildren<KappaAnimations>();
    }

    public override void UpdateTarget(Transform target)
    {
        if (!_updateTarget) return;

        // Look at player
        Vector3 dirVec = (target.position + target.GetComponent<Rigidbody>().velocity * 0.5f) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dirVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _targetingSpeed * Time.deltaTime);
    }

    public override void Chase(Transform target)
    {
        SetNavValues();
        _navAgent.SetDestination(target.position);
    }

    public override void AttackTarget()
    {
        if (_enemy.CanAttack == true)
        {
            StartCoroutine(attack());
        }
    }

    /// <summary>
    /// Temporary method while animations are not implemented
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator attack()
    {
        _navAgent.updateRotation = false;
        _navAgent.isStopped = true;
        _updateTarget = false;
        yield return new WaitForSeconds(_animationLength);
        meleeAttack();
    }

    private void meleeAttack()
    {
        //if (_enemy.CanAttack == true)
        {
            _enemy.CanAttack = false;

            // Activate trigger hitbox
            _attackHitBox.gameObject.SetActive(true);
            _attackHitBox.enabled = true;

            if (_kappaAnime != null) _kappaAnime.AttackAnimation();

            // Start cooldown
            StartCoroutine(attackCooldown());
        }
    }

    protected override IEnumerator attackCooldown()
    {
        // Disables the hitbox after 1 frame of the attack
        yield return null;
        _attackHitBox.enabled = false;

        // Resumes the movement and resets the canAttack property after some time
        yield return new WaitForSeconds(_attackCooldown);
        StopCoroutine(attack());

        // Resume movement and rotation
        _navAgent.isStopped = false;
        _updateTarget = true;

        // Reset Attack
        _attackHitBox.gameObject.SetActive(false);
        _enemy.CanAttack = true;
    }
}
