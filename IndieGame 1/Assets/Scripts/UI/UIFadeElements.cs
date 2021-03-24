using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeElements : MonoBehaviour {

    private Image _img;

    private void Start()
    {
        _img = GetComponent<Image>();
    }

    public static void CrossFadeIn(Image image, float time, float min = 0f, float max = 1f)
    {
        //image.CrossFadeAlpha(max, time, )
    }

    public void FadeIn()
    {
        _img.CrossFadeAlpha(1, 0.5f, false);
    }

    public void FadeOut()
    {
        _img.CrossFadeAlpha(0.01f, 0.5f, false);
    }
}
