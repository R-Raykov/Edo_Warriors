using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : AttackBehaviour
{
    [SerializeField] private Projectile _projectile;

    private Transform _muzzle;

    private GhostAnimations _ghostAnime;

    private void Awake()
    {
        _muzzle = transform.Find("Muzzle");
        _ghostAnime = GetComponentInChildren<GhostAnimations>();
    }

    public override void UpdateTarget(Transform target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1.75f * Time.deltaTime);
        StartCoroutine(attack());
    }

    public override void Chase(Transform target)
    {
        SetNavValues();
        _navAgent.SetDestination(target.position);
    }

    /// <summary>
    /// Temporary method before animation is implemented
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator attack()
    {
        yield return new WaitForSeconds(_animationLength);
        Fire();
    }

    private void Fire()
    {
        if (_enemy.CanAttack == true)
        {
            _enemy.CanAttack = false;
            Instantiate(_projectile.gameObject, _muzzle.position, _muzzle.rotation);

            if (_ghostAnime != null) _ghostAnime.Attack();

            StartCoroutine(attackCooldown());
        }
    }

    protected override IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        StopCoroutine(attack());
        _enemy.CanAttack = true;
    }

    public Transform Muzzle
    {
        get { return _muzzle; }
    }
}
