using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMOD_SoundLibrary : MonoBehaviour
{
    [EventRef] public string[] sounds;

    public static void PlayOneShot(string soundName, Vector3 soundPosition)
    {
        RuntimeManager.PlayOneShot(soundName, soundPosition);
    }
}
