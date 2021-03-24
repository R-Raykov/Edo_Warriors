using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDarkenElements : MonoBehaviour, IActivatable
{
    private Image _img;
    private Color _ogColor;
    private bool _isDark = false;

	// Use this for initialization
	void Start () {
        _img = GetComponent<Image>();
        _ogColor = _img.color;
	}

    public void Activate()
    {
        if (_isDark)    Brighten();
        else            Darken();

        _isDark = !_isDark;
    }

    public void Darken()
    {
        _img.CrossFadeColor(Color.black, 0.5f, false, false);
    }

    public void Brighten()
    {
        _img.CrossFadeColor(_ogColor, 0.5f, false, false);
    }
}

