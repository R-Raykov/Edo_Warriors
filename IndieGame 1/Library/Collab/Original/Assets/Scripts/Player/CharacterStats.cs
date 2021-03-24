using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Tooltip("Ammount of hitpoints the player can take in damage")]
    [SerializeField] private float _health = 20;

    [Tooltip("Ammount of mana available to the player")]
    [SerializeField] private float _mana = 20;

    [Tooltip("Ammount of attack power")]
    [SerializeField]  private float _attackPower;

    [Tooltip("Ammount of attack power")]
    [SerializeField] private float _magicPower;

    public enum CharType { Defaut, Samurai, Geisha };
    public CharType CharacterClass;

    [SerializeField] private float _respawnTime = 5f;

    //TODO: Add mana, dmg, etc.

    public event System.Action<CharacterStats> OnDeath;
    public System.Action<CharacterStats> OnRespawn;
    public System.Action<CharacterStats> OnHealthLow;
    public System.Action<CharacterStats> OnHealthStabilize;
    public System.Action<CharacterStats> OnTakeDamage;
    public System.Action<float> OnHealthChange;
    public System.Action<float> OnManaChange;
    public System.Action<int> OnKarmaChange;

    private float _maxHealth;
    private bool _isHealthLow = false;
    private float _lowHealthPercentage = 0.25f;

    private float _maxMana;
    private int _karmaPoints;

    // TEMPORARY 
    private float _oldMana;


    // Player number and a timer that deactivates the player after 30 seconds
    [Tooltip("Number of the player")]
    [Range(1, 2)]
    public int PlayerNumber;
    private float _inactivityTimer;

    // Use this for initialization
    void Awake()
    {
        _maxHealth = _health;
        _maxMana = _mana;

        _oldMana = _mana;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (CharacterClass == CharType.Samurai)     CharacterClass = CharType.Geisha;
            else if (CharacterClass == CharType.Geisha) CharacterClass = CharType.Samurai;
        }

        if(_oldMana != _mana)
        {
            _oldMana = _mana;
            OnManaChange.Invoke(_mana);
        }

        Mana += 0.1f;

        if (Input.GetKeyDown(KeyCode.Y)) ModifyKarma(370);
        if (Input.GetKeyDown(KeyCode.U)) ModifyKarma(-1000);


        HandlePlayers();
    }

    /// <summary>
    /// Takes health away from the player by the specified ammount, clamping health to minimum
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        _health = Mathf.Clamp(_health - damage, 0, _maxHealth);

        // Check and Invoke LowHealth alert. Feedback event
        if (_health / _maxHealth <= _lowHealthPercentage && !_isHealthLow)
        {
            _isHealthLow = true;
            if (OnHealthLow != null)
                OnHealthLow.Invoke(this);
        }

        OnHealthChange.Invoke(_health);

        // "Kills" the object
        if (_health == 0)
        {
            GetComponentInChildren<RagdollActivator>().Activate(this);
            if (OnDeath != null) OnDeath.Invoke(this);
            gameObject.SetActive(false);
        }

        // If still alive invoke TakeDamage
        if (OnTakeDamage != null) OnTakeDamage.Invoke(this);
    }

    /// <summary>
    /// Heals the player by the specified ammount, clamping health to maximum
    /// </summary>
    /// <param name="points"></param>
    public virtual void Heal(float points)
    {
        _health = Mathf.Clamp((_health + points), 0, _maxHealth);

        // Check and Invoke if above low health. Feedback event
        if (_health / _maxHealth > _lowHealthPercentage && _isHealthLow)
        {
            _isHealthLow = false;
            if (OnHealthStabilize != null)
                OnHealthStabilize.Invoke(this);
        }

        if(OnHealthChange != null)
            OnHealthChange.Invoke(_health);
    }

    /// <summary>
    /// Add or Consume mana by the specified ammount
    /// </summary>
    /// <param name="points"></param>
    public void ModifyMana(float points)
    {
        _mana = Mathf.Clamp((_mana + points), 0, _maxMana);

        if (OnManaChange != null)
            OnManaChange.Invoke(_health);
    }

    public void ModifyKarma(int points)
    {
        _karmaPoints = Mathf.Clamp((_karmaPoints + points), 0, int.MaxValue);

        if (OnKarmaChange != null)
            OnKarmaChange.Invoke(_karmaPoints);
    }



    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            Projectile bullet = other.GetComponent<Projectile>();
            TakeDamage(bullet.Damage);
            Destroy(other.gameObject);
        }
    }

    private void OnEnable()
    {
        Heal(MaxHealth);
        Mana = MaxMana;
        GetComponent<PlayerUI>().SetActive(true); 
    }

    public float MaxHealth
    {
        get { return _maxHealth; }
    }
    public float Health
    {
        get { return _health; }
    }

    public float MaxMana
    {
        get { return _maxMana; }
    }
    public float Mana
    {
        get { return _mana; }
        set { _mana = value; }  // TEMPORARY, SHOULD NOT NEED TO SET
    }
    public float RespawnTime
    {
        get { return _respawnTime; }
    }
    public float AttackPower
    {
        get { return _attackPower; }
        set { _attackPower = value; }
    }
    public float MagicPower
    {
        get { return _magicPower; }
        set { _magicPower = value; }
    }
    public int KarmaPoints
    {
        get { return _karmaPoints; }
    }

    private void HandlePlayers()
    {
        _inactivityTimer += Time.deltaTime;
        //print(_inactivityTimer);
        if (_inactivityTimer > 30 && GameManager.nPlayers > 1)
        {
            _inactivityTimer = 0;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameManager.Instance.UnregisterPlayer(this);
            gameObject.SetActive(false);
        }

        // Gay way of detecting if the player is inactive

        if (Input.GetAxis("TriggersJ" + PlayerNumber) != 0
            || Input.GetAxis("RBumperJ" + PlayerNumber) != 0
            || Input.GetAxis("LBumperJ" + PlayerNumber) != 0
            || Input.GetAxis("HorizontalJ" + PlayerNumber) != 0
            || Input.GetAxis("VerticalJ" + PlayerNumber) != 0
            || Input.GetAxis("HorizontalCameraJ" + PlayerNumber) != 0
            || Input.GetAxis("VerticalCameraJ" + PlayerNumber) != 0
            || Input.GetAxis("HorizontalP" + PlayerNumber) != 0
            || Input.GetAxis("VerticalP" + PlayerNumber) != 0) _inactivityTimer = 0;
    }

    private void OnLevelWasLoaded(int level)
    {
        Vector3 spawn = GameObject.FindObjectOfType<Checkpoint>().transform.position;

        if (this == GameManager.Instance.Player1) transform.position = spawn + Vector3.right;
        else if (this == GameManager.Instance.Player2) transform.position = spawn + Vector3.left;
    }
}
