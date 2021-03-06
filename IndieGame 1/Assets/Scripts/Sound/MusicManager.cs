using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _mainLoop;
    private AudioSource _source;
 
	// Use this for initialization
	void Start ()
    {
        _source = GetComponent<AudioSource>();
        ChangeClip(_menuMusic);
	}
	
    public void ChangeClip(AudioClip newClip)
    {
        _source.clip = newClip;
        _source.Play();
    }

    public void Stop()
    {
        _source.Stop();
    }

    public void PlayMenu()
    {
        ChangeClip(_menuMusic);
    }

    public void PlayMain()
    {
        ChangeClip(_mainLoop);
    }
}
