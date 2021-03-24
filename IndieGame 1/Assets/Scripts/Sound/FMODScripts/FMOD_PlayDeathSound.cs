using FMODUnity;
using UnityEngine;

public class FMOD_PlayDeathSound : MonoBehaviour
{
    private CharacterStats _player;
    private FMOD_SoundLibrary _soundLibrary;
    
	// Use this for initialization
	void Start () {
        _player = GetComponent<CharacterStats>();
        //_player += playSound;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void playSound()
    {
        FMOD_SoundLibrary.PlayOneShot(_soundLibrary.sounds[5], transform.position);
    }
}
