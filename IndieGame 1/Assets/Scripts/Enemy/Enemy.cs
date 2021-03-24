using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//TODO SPLIT DIS SHIT IN 2 CLASSES
public class Enemy : AbstractEnemyAgent
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _damage;

    private NavMeshAgent _navAgent;
    private AttackBehaviour _attackBehaviour;
    private PassiveBehaviour _passiveBehaviour;

    private float _cachedSpeed;
    private float _cachedStoppingDistance;
    private float _cachedAngularSpeed;
    private float _cachedAcceleration;

    private bool _shouldWander = true;
    private enum Type { Melee, Shooting };
    private Type _type;

    //private bool _crowdControlled = false;

	// Use this for initialization
	protected override void Awake () {
        base.Awake();

        _navAgent = GetComponent<NavMeshAgent>();
        _attackBehaviour = GetComponent<AttackBehaviour>();
        _passiveBehaviour = GetComponent<PassiveBehaviour>();
        //_passiveBehaviour.enabled = false;

        _cachedSpeed = _navAgent.speed;
        _cachedStoppingDistance = _navAgent.stoppingDistance;
        _cachedAngularSpeed = _navAgent.angularSpeed;
        _cachedAcceleration = _navAgent.acceleration;

        if (GetComponent<ShootingBehaviour>() != null)  _type = Type.Shooting;
        else                                            _type = Type.Melee;

        OnAlertRange += enterAggro;

        CoinExplode coinExplode = GetComponent<CoinExplode>();
        if (coinExplode != null) OnDeath += coinExplode.Generate;
    }

    // Update is called once per frame
    void Update()
    {
        if (_navAgent.enabled == false) return;

        if (_type == Type.Melee)    updateMelee();
        else                        updateShooting();

        if (Input.GetKeyDown(KeyCode.U)) TakeDamage(Mathf.Infinity);
    }

    private void updateMelee()
    {
        if (IsAlerted)
        {
            _passiveBehaviour.enabled = false;
            _attackBehaviour.Chase(_target);
            _attackBehaviour.UpdateTarget(_target);
        }
        else if (_shouldWander) updatePassiveState();
        
        if (InMelee)            _attackBehaviour.AttackTarget();
        else                    _navAgent.updateRotation = true;
    }

    private void updateShooting()
    {
        if (IsAlerted)
        {
            _attackBehaviour.UpdateTarget(_target);
            _attackBehaviour.Chase(_target);
        }
        else if (_shouldWander) updatePassiveState();
        
    }

    private void updatePassiveState()
    {
        _passiveBehaviour.enabled = true;
        _passiveBehaviour.SetNavValues();
        _passiveBehaviour.UpdateAgent();
    }

    /// <summary>
    /// Calls to enter aggro on the specified player, will try to engage
    /// </summary>
    /// <param name="player"></param>
    private void enterAggro(CharacterStats player)
    {
        _passiveBehaviour.enabled = false;
        _target = player.transform;
        //_attackBehaviour.SetNavValues();
        IsAlerted = true;
        player.OnDeath += resetAggro;
    }

    /// <summary>
    /// Stops aggro on the specified player and returns to passive behaviour
    /// </summary>
    /// <param name="player"></param>
    private void resetAggro(CharacterStats player)
    {
        // The first check is when quitting the application
        if (this == null || !gameObject.activeInHierarchy) return;

        _target = null;
        //_passiveBehaviour.SetNavValues();
        IsAlerted = false;
        InMelee = false;

        updatePassiveState();
    }

    /// <summary>
    /// Gets the damage the agent deals
    /// </summary>
    public float Damage
    {
        get { return _damage; }
    }

    /// <summary>
    /// Gets/Sets if the agent can attack
    /// </summary>
    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }

    /// <summary>
    /// Gets/Sets if the agent should wander if it is able to
    /// </summary>
    public bool ShouldWander
    {
        get { return _shouldWander; } 
        set { _shouldWander = value; }
    }

    public Transform Target
    {
        get { return _target; }
    }
}

[System.Serializable]
public class NavAgentValues
{
    public float speed = 0.5f;
    public float stoppingDistance = 0.1f;
    public float angularSpeed = 90f;
    public float acceleration = 2f;
}
