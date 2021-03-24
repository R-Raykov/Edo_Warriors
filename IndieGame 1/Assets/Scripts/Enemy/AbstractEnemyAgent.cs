using UnityEngine;

public abstract class AbstractEnemyAgent : MonoBehaviour
{
    [Tooltip("Ammount of hitpoints the agent can take in damage")]
    [SerializeField] private float _health = 300;

    [Tooltip("The UI element that represents the health of the player")]
    [SerializeField] private UIFillBar _hpBar;

    [Tooltip("How many points the player gets by killing this enemy")]
    [SerializeField] private int _bounty = 25;

    public System.Action<AbstractEnemyAgent> OnDeath;
    public System.Action OnHealthLow;
    public System.Action OnHealthStabilize;
    public System.Action OnDamageTaken;
    public System.Action OnCrowdControlled;

    public System.Action<CharacterStats> OnAlertRange;
    public System.Action<CharacterStats> OnMeleeRange;

    protected bool _canAttack = true;
    private bool _isHealthLow = false;
    private bool _alerted = false;
    private bool _inMelee = false;

    private float _lowHealthPercentage = 0.25f;
    private float _maxHealth;

    private static int _nextId = 0;
    private int _id;

    // Needs to run before subclass init (Use Start for subclass)
    protected virtual void Awake()
    {
        _maxHealth = _health;
        _id = _nextId++;
    }

    /// <summary>
    /// Takes health away from the agent by the specified ammount, clamping health to minimum
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
        if (_health / _maxHealth <= _lowHealthPercentage && !_isHealthLow)
        {
            _isHealthLow = true;
            if (OnHealthLow != null) OnHealthLow.Invoke();
        }

        if(_health == 0)
        {
            GetComponentInChildren<RagdollDecompose>().Activate(this);
            if (OnDeath != null) OnDeath.Invoke(this);
            gameObject.SetActive(false);
        }

        OnDamageTaken();
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BulletBehaviour>() != null)
        {
            BulletBehaviour bullet = other.GetComponent<BulletBehaviour>();
            TakeDamage(bullet.Damage);
            Destroy(other.gameObject);
        }

        if (other.GetComponent<AbilityDmg>() != null)
        {
            TakeDamage(other.GetComponent<AbilityDmg>().Damage);
        }
    }

    #region Properties

    /// <summary>
    /// Gets and sets if the enemy is a alert of a player closeby
    /// </summary>
    public bool IsAlerted
    {
        get { return _alerted; }
        set { _alerted = value; }
    }

    /// <summary>
    /// Getss and sets if the enemy is in Melee range from a player
    /// </summary>
    public bool InMelee
    {
        get { return _inMelee; }
        set { _inMelee = value; }
    }

    /// <summary>
    /// Gets the Id of the enemy
    /// </summary>
    public int Id
    {
        get { return _id; }
    }

    /// <summary>
    /// Gets the current health of the enemy
    /// </summary>
    public float Health
    {
        get { return _health; }
    }

    /// <summary>
    /// Gets the max health of the enemy
    /// </summary>
    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    /// <summary>
    /// Gets and Sets the bounty for the enemy
    /// </summary>
    public int Bounty
    {
        get { return _bounty; }
        set { _bounty = value; }
    }
    #endregion
}
