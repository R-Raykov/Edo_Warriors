using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlowBlink : MonoBehaviour, IActivatable
{
    [SerializeField] private float _blinkSpeed = 0.75f;

    private Text _text;
    //private TextMesh _text2;
    private Color _ogColor;
    private bool _isDark = false;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<Text>();
        //if (_text == null) _text2 = GetComponent<TextMesh>();

        _ogColor = _text.color;

        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        yield return new WaitForSeconds(_blinkSpeed);
        Activate();
        StartCoroutine(Loop());
    }

    public void Activate()
    {
        if (_isDark) Brighten();
        else Darken();

        _isDark = !_isDark;
    }

    //public void Darken2()
    //{
    //    _text2.color = new Color(_text2.color.r, _text2.color.g, _text2.color.b, Mathf.Lerp(_text2.color.a, 0, _blinkSpeed));
    //}

    public void Darken()
    {
        _text.CrossFadeAlpha(0, _blinkSpeed, false);
    }

    public void Brighten()
    {
        _text.CrossFadeAlpha(1, _blinkSpeed, false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
