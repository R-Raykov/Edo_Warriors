using System.Collections.Generic;
using UnityEngine;

public class FMOD_EnemySoundController : MonoBehaviour
{
    private AbstractEnemyAgent _player;
    private FMOD_SoundLibrary _soundLibrary;

    [SerializeField] private int _deathSoundIndex;
    [SerializeField] private int _hurtSoundIndex;

    private void Awake()
    {
        _player = GetComponentInParent<AbstractEnemyAgent>();
        _soundLibrary = GetComponent<FMOD_SoundLibrary>();
    }

    public void PlayOneShot(int soundID)
    {
        FMOD_SoundLibrary.PlayOneShot(_soundLibrary.sounds[soundID], transform.position);
    }

    private void playDeath(AbstractEnemyAgent p)
    {
        // Hardcoded death sound index
        FMOD_SoundLibrary.PlayOneShot(_soundLibrary.sounds[_deathSoundIndex], transform.position);
    }

    private void playHurt()
    {
        // Hardcoded hurt sound index
        FMOD_SoundLibrary.PlayOneShot(_soundLibrary.sounds[_hurtSoundIndex], transform.position);
    }

    private void OnDestroy()
    {
        _player.OnDeath -= playDeath;
        _player.OnDamageTaken -= playHurt;
    }

    private void OnEnable()
    {
        _player.OnDeath += playDeath;
        _player.OnDamageTaken += playHurt;
    }

    private void OnDisable()
    {
        _player.OnDeath -= playDeath;
        _player.OnDamageTaken -= playHurt;
    }

}