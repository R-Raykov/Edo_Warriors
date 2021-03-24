using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LootExplode))]
public class LootChest : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip[] _sounds;

    private LootExplode _loot;
    private Animator _anime;
    private AudioSource _source;


    private void Start()
    {
        _loot = GetComponent<LootExplode>();
        _source = GetComponent<AudioSource>();
        _anime = GetComponentInChildren<Animator>();
    }

    public void Interact(CharacterStats player)
    {
        if (player.KarmaPoints > 0)
        {
            if (_anime != null) _anime.Play("Crate_Lit");

            player.AttackPower *= 2;
            player.MagicPower *= 2;

            player.ModifyKarma(int.MinValue);
            _loot.Generate(player.KarmaPoints);

            // point suck
            _source.PlayOneShot(_sounds[0]);
            StartCoroutine(delay());
        }
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(0.25f);
        // Chest open
        _source.PlayOneShot(_sounds[1]);

        yield return new WaitForSeconds(0.5f);
        // Power up sound
        _source.PlayOneShot(_sounds[2]);
    }
}
