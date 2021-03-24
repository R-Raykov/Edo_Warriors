using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Tooltip("The UI element to use as a health bar")]
    [SerializeField] private UIFillBar _healthBar;
    [Tooltip("The UI element to use as a mana bar")]
    [SerializeField] private UIFillBar _manaBar;
    [Tooltip("The UI element for Karma points")]
    [SerializeField] private Text _karmaPoints;

    private GameObject _skillHolder;

    private float _maxHealth;
    private float _maxMana;
    private CharacterStats _player;

    // Use this for initialization
    void Start () {

        _player = GetComponent<CharacterStats>();

        _player.OnHealthChange += updateHealthBar;
        _maxHealth = _player.MaxHealth;

        _player.OnManaChange += updateManaBar;
        _maxMana = _player.MaxMana;

        _player.OnKarmaChange += updateKarma;
    }

    /// <summary>
    /// Updates the fill value of the bar
    /// </summary>
    private void updateHealthBar(float health)
    {
        //Value = _player.Health / _maxHealth;
        StartCoroutine(_healthBar.lerpBar(health / _maxHealth));

        // TODO FIX THIS, THE LERP ISN'T ACTUALLY CALLED ON THE LAST BIT OF HEALTH
        if(health == 0)
        {
            SetActive(false);
        }
    }

    private void updateManaBar(float mana)
    {
        StartCoroutine(_manaBar.lerpBar(mana / _maxMana));
    }

    private void updateKarma(int karma)
    {
        if(_player.gameObject.activeInHierarchy) StartCoroutine(IncrementCoroutine(karma));
    }

    private IEnumerator IncrementCoroutine(int targetValue)
    {
        float time = 0;
        int startingValue = int.Parse(_karmaPoints.text);
        _karmaPoints.text = startingValue.ToString();

        while (time < 0.5f)
        {
            yield return null;

            time += Time.deltaTime;
            float factor = time / 0.5f;

            _karmaPoints.text = ((int)Mathf.Lerp(startingValue, targetValue, factor)).ToString();
        }

        //if (enableWhenDone != null)
        //    enableWhenDone.SetActive(true);

        yield break;
    }

    public void SetActive(bool value)
    {
        if (_healthBar != null) _healthBar.gameObject.SetActive(value);
        if (_manaBar != null) _manaBar.gameObject.SetActive(value);
        if (_karmaPoints != null) _karmaPoints.gameObject.SetActive(value);
        if (_skillHolder != null) _skillHolder.SetActive(value);
    }

    public void SetUISet(PlayerUISet set)
    {
        _healthBar = set.healthBar;
        _manaBar = set.manaBar;
        _karmaPoints = set.points;
        _skillHolder = set.skillSetHolder;
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
        _player.OnHealthChange -= updateHealthBar;
    }
}
