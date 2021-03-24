using System.Collections.Generic;
using UnityEngine;

public class FMOD_PlayerSoundController : MonoBehaviour
{
    private Attack _attack;
    private CharacterStats _player;
    private FMOD_SoundLibrary _soundLibrary;

    [SerializeField] private int _deathSoundIndex;
    [SerializeField] private int _hurtSoundIndex;

    private void Awake()
    {
        _attack = GetComponentInParent<Attack>();
        _player = GetComponentInParent<CharacterStats>();
        _soundLibrary = GetComponent<FMOD_SoundLibrary>();
    }

    public void PlayOneShot(int soundID)
    {
        FMOD_SoundLibrary.PlayOneShot(_soundLibrary.sounds[soundID], transform.position);
    }

    private void playDeath(CharacterStats p)
    {
        // Hardcoded death sound index
        FMOD_SoundLibrary.PlayOneShot(_soundLibrary.sounds[_deathSoundIndex], transform.position);
    }

    private void playHurt(CharacterStats p)
    {
        // Hardcoded hurt sound index
        FMOD_SoundLibrary.PlayOneShot(_soundLibrary.sounds[_hurtSoundIndex], transform.position);
    }

    private void OnDestroy()
    {
        _player.OnDeath -= playDeath;
        _player.OnTakeDamage -= playHurt;
    }

    private void OnEnable()
    {
        _player.OnDeath += playDeath;
        _player.OnTakeDamage += playHurt;
    }

    private void OnDisable()
    {
        _player.OnDeath -= playDeath;
        _player.OnTakeDamage -= playHurt;   
    }

    //private void LoadGeishaSet()
    //{
    //    _attack.OnLBUp += playRB;
    //}

    private void LoadSamuraiSet()
    {

    }

}