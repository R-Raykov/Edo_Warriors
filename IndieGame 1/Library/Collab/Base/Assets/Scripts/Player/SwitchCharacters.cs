using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacters : MonoBehaviour {

    [SerializeField] private GameObject def;
    [SerializeField] private GameObject samurai;
    [SerializeField] private GameObject geisha;

    [SerializeField] private PlayerUISwapper UISwapper;

    [SerializeField] private ParticleSystem playerCircle;
    [SerializeField] private ParticleSystem playerText;

    private CharacterStats charStats;
    private bool startOfTheGame = true;


    private void Start ()
    {
        charStats = GetComponent<CharacterStats>();
        geisha.SetActive(false);
        samurai.SetActive(false);
        def.SetActive(true);
        charStats.CharacterClass = PlayerClass.Defaut;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            if (charStats.CharacterClass == PlayerClass.Samurai) SwitchToGeisha(); 
            else if (charStats.CharacterClass == PlayerClass.Geisha) SwitchToSamurai();
        }

        //SHOULD BE MOVED TO A METHOD THATS CALLED WHEN YOU PICK UP SHIT
        if ((charStats.CharacterClass == PlayerClass.Samurai
            || charStats.CharacterClass == PlayerClass.Defaut)
            && geisha.activeSelf == true && !startOfTheGame)
        {
            SwitchToSamurai();
        }
        else if ((charStats.CharacterClass == PlayerClass.Geisha
                || charStats.CharacterClass == PlayerClass.Defaut) 
                && samurai.activeSelf == true && !startOfTheGame)
        {
            SwitchToGeisha();
        }
    }

    private void SwitchToSamurai()
    {
        samurai.SetActive(true);
        geisha.SetActive(false);
        def.SetActive(false);

        swapUI(PlayerClass.Samurai);

        charStats.CharacterClass = PlayerClass.Samurai;

        playerCircle.startColor = new Color(0.070f, 0.811f, 0.925f);        //used the internet to conver it t
        playerText.startColor = new Color(0.070f, 0.811f, 0.925f);

        samurai.GetComponent<FMOD_PlayerSoundController>().enabled = true;
        geisha.GetComponent<FMOD_PlayerSoundController>().enabled = false;
    }

    private void SwitchToGeisha()
    {
        geisha.SetActive(true);
        def.SetActive(false);
        samurai.SetActive(false);

        swapUI(PlayerClass.Geisha);

        charStats.CharacterClass = PlayerClass.Geisha;
        var main = playerCircle.main;                                   //new way of doing it
        main.startColor = new Color(0.886f, 0.498f, 0.819f);            //i dont like the new way
        playerText.startColor = new Color(0.886f, 0.498f, 0.819f);      //deprecated way of doing it

        print("a");

        geisha.GetComponent<FMOD_PlayerSoundController>().enabled = true;
        samurai.GetComponent<FMOD_PlayerSoundController>().enabled = false;
    }

    private void swapUI(PlayerClass pClass)
    {
        UISwapper.SwapUISet(pClass);
        GetComponent<PlayerUI>().SetUISet(UISwapper.GetCurrentUISet());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Sword CLASS") SwitchToSamurai();
        else if (other.gameObject.name == "Shamisen CLASS") SwitchToGeisha();
    }
}
