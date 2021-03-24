using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuDjordy : MonoBehaviour {

    [SerializeField] private EventSystem _eSystem;

    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private GameObject _selector;

    private GameObject _last;
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _last = _eSystem.firstSelectedGameObject;
        _selector.transform.position = new Vector3(_selector.transform.position.x, _last.transform.position.y);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("00_Central_Hub");
    }

    private void Update()
    {
        if(_eSystem.currentSelectedGameObject != _last)
        {
            _last = _eSystem.currentSelectedGameObject;
            _selector.transform.position = new Vector3(_selector.transform.position.x, _last.transform.position.y);
            _audio.PlayOneShot(_hoverSound);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
