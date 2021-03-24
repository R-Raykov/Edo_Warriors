using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class MeleeSmartBehaviour : AttackBehaviour
{
    [SerializeField] private EnemyAttackHitBox _attackHitBox;
    [SerializeField] private float _targetingSpeed = 1.75f;

    [SerializeField] private Vector3 _sideStepDistance = Vector3.right;
    private bool _sidestep = false;
    private bool _updateTarget = true;
    private int _side;

    private Vector3 _steppingPositionR;
    private Vector3 _steppingPositionL;

    private float _timer = 120f;

    private UmbrellaAnimations _umbrellaAnimations;

    private void Awake()
    {
        _umbrellaAnimations = GetComponentInChildren<UmbrellaAnimations>();
    }

    public override void UpdateTarget(Transform target)
    {
        if (!_updateTarget) return;

        // Look at player
        Vector3 dirVec = (target.position + target.GetComponent<Rigidbody>().velocity * 0.5f) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dirVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _targetingSpeed * Time.deltaTime);
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

            if(_umbrellaAnimations != null) _umbrellaAnimations.AttackAnimation();

            // Start cooldown
            StartCoroutine(attackCooldown());
        }
    }

    public override void Chase(Transform target)
    {
        SetNavValues();
        if (!_navAgent.pathPending)
        {
            if (!_sidestep) _navAgent.SetDestination(target.position);
            else
            {
                if (_side % 2 == 0)
                {
                    _navAgent.SetDestination(_steppingPositionR * 3);
                } else
                {
                    _navAgent.SetDestination(_steppingPositionL * 3);
                }

                _timer--;
                if(_timer < 0)
                {
                    _side++;
                    _timer = 120;
                    //Debug.Log(_side);
                }

            }
        }
        //UpdateTarget(target);
    }

    private IEnumerator delay(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

    protected override IEnumerator attackCooldown()
    {
        // Disables the hitbox after 1 frame of the attack
        yield return null;
        _attackHitBox.enabled = false;

        _steppingPositionR = transform.position + transform.right;
        _steppingPositionL = transform.position - transform.right;
        _sidestep = true;

        // Resumes the movement and resets the canAttack property after some time
        yield return new WaitForSeconds(_attackCooldown);
        StopCoroutine(attack());

        // Resume the movement and rotation
        _navAgent.isStopped = false;
        _updateTarget = true;
        _sidestep = false;

        // Reset attack
        _attackHitBox.gameObject.SetActive(false);
        _enemy.CanAttack = true;
    }
}
