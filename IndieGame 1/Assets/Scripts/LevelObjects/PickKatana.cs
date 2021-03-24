using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickKatana : MonoBehaviour, IInteractable {

    // Use this for initialization
    void Start () {
		
	}
	

    public void Interact(CharacterStats player)
    {
        if(gameObject.name == "Sword CLASS") player.CharacterClass = PlayerClass.Samurai;
        else player.CharacterClass = PlayerClass.Geisha;
    }
}
