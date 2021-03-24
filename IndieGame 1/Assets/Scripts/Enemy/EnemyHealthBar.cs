using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : UIFillBar
{
    [Tooltip("Offset of the health bar from the target object")]
    [SerializeField] private Vector3 _offset = Vector3.up;

    private static Canvas _canvas;
    private AbstractEnemyAgent _agent;
    private float _maxHealth;

    private void Awake()
    {
        // Doing a lookup to avoid having too much variable pollution in the inspector
        if (_canvas == null) _canvas = FindObjectOfType<Canvas>();
        _fill.canvasRenderer.SetAlpha(0.0f);
        _back.canvasRenderer.SetAlpha(0.0f);
    }

    private void Start()
    {
        // Store the reference to the agent
        _agent = transform.GetComponentInParent<AbstractEnemyAgent>();
        _agent.OnDamageTaken += updateBar;
        _maxHealth = _agent.MaxHealth;

        // Move the parent to the canvas
        transform.SetParent(_canvas.transform, false);
    }

    private void LateUpdate()
    {
        // Update the position of the UI to the position of the target
        transform.position = GameManager.Instance.MainCamera.WorldToScreenPoint((_offset) + _agent.transform.position);
    }

    /// <summary>
    /// Updates the fill value of the bar
    /// </summary>
    private void updateBar()
    {
        _fill.CrossFadeAlpha(1, 0.15f, false);
        _back.CrossFadeAlpha(1, 0.15f, false);
        StartCoroutine(lerpBar(_agent.Health / _maxHealth));
    }

    /// To use with poling maybe
    //public void SetAgent(AbstractEnemyAgent agent)
    //{
    //    _agent = agent;
    //}

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
        _agent.OnDamageTaken -= updateBar;
    }
}
