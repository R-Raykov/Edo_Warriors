using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathTimer : MonoBehaviour
{
    [Tooltip("Offset of the health bar from the target object")]
    [SerializeField] private Vector3 _offset = Vector3.up;
    [SerializeField] private Image _back;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _front;
    [SerializeField] private Text _text;

    private enum PlayerID { P1, P2 };
    [SerializeField] private PlayerID _playerID;

    private CharacterStats _player;
    private Transform _otherPlayer;
    private bool _enabled = false;

    private void Awake()
    {
        // Doing a lookup to avoid having too much variable pollution in the inspector
        _fill.canvasRenderer.SetAlpha(0.0f);
        _back.canvasRenderer.SetAlpha(0.0f);
        _front.canvasRenderer.SetAlpha(0.0f);
        _text.canvasRenderer.SetAlpha(0.0f);
    }

    private IEnumerator delayStart()
    {
        yield return new WaitForSeconds(5f);

        if (_playerID == PlayerID.P1)
        {
            Debug.Log(_player);
            _player = GameManager.Instance.Player1;
            Debug.Log(_player);
            Debug.Log(_otherPlayer);
            _otherPlayer = GameManager.Instance.Player2.transform;
            Debug.Log(_otherPlayer);
        }
        else if (_playerID == PlayerID.P2)
        {
            _player = GameManager.Instance.Player2;
            //_otherPlayer = GameManager.Instance.Player1.transform;
        }

        // Store the reference to the agent
        Debug.Log("is player null? " + _player == null);
        _player.OnDeath += Enable;


        //_maxHealth = _agent.MaxHealth;
    }

    private void OnEnable()
    {
        StartCoroutine(delayStart());
    }

    /// <summary>
    /// Sets and gets the fill ammount the bar is currently at
    /// </summary>
    public float Value
    {
        get { return _fill.fillAmount; }
        set { _fill.fillAmount = Mathf.Max(0, Mathf.Min(1, value)); }
    }

    private void Enable(CharacterStats p)
    {
        Debug.Log("UI ENABLED");
        _enabled = true;
        _fill.CrossFadeAlpha(1, 0.15f, false);
        _back.CrossFadeAlpha(1, 0.15f, false);
        _front.CrossFadeAlpha(1, 0.15f, false);
        _text.CrossFadeAlpha(1, 0.15f, false);

        Value = 1;
        StartCoroutine(lerpText(0));
        //StartCoroutine(lerpBar(0));
        StartCoroutine(MoveOverSeconds());
    }

    private void FixedUpdate()
    {
        // Update the position of the UI to the position of the target
        if (_enabled)
        {
            if (_otherPlayer.gameObject.activeInHierarchy)
                transform.position = GameManager.Instance.MainCamera.WorldToScreenPoint((_offset) + _otherPlayer.position);
            else
                transform.position = GameManager.Instance.MainCamera.WorldToScreenPoint((_offset) + _player.transform.position);
        }
    }

    public IEnumerator lerpText(int newValue)
    {
        float currentVal = _player.RespawnTime;

        // Lerp the bar fill ammount
        while (currentVal > newValue)
        {
            currentVal--;
            //StartCoroutine(lerpBar(currentVal));
            _text.text = "" + currentVal;
            //Value = currentVal;
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(lerpBar(0));
        // Disable the bar after the lerping is finished
        if (newValue == 0) disableBar();
    }

    public IEnumerator lerpBar(float newValue)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / _player.RespawnTime;
            Value = Mathf.Lerp(Value, newValue, t);
            yield return null;
        }
    }

    public IEnumerator MoveOverSeconds()
    {
        float duration = _player.RespawnTime;
        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            Value = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }

        disableBar();
    }

    private void disableBar()
    {
        _fill.CrossFadeAlpha(0, 0.15f, false);
        _back.CrossFadeAlpha(0, 0.15f, false);
        _front.CrossFadeAlpha(0, 0.15f, false);
        _text.CrossFadeAlpha(0, 0.15f, false);
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
        _player.OnDeath -= Enable;
    }
}
