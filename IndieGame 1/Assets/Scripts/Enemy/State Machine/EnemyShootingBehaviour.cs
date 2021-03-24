using UnityEngine;

public class EnemyShootingBehaviour : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _rateOfFire; 

    private Transform _muzzle;

    private float nextFireAllowed;

    // Use this for initialization
    void Awake()
    {
        _muzzle = transform.Find("Muzzle");
    }

    public void Fire()
    {
        if (Time.time < nextFireAllowed)
            return;

        nextFireAllowed = Time.time + _rateOfFire;
        Instantiate(_projectile.gameObject, _muzzle.position, _muzzle.rotation);
    }

    public float RateOfFire
    {
        get { return _rateOfFire; }
        set { _rateOfFire = value; }
    }

    public Transform Muzzle
    {
        get { return _muzzle; }
    }
}
