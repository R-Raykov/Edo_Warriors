using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSlidingDoor : MonoBehaviour, IActivatable {

    [SerializeField] private GameObject[] Doors;

    private bool _activateDoor;
    private bool _doorIsOpen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        _doorIsOpen = true;

        foreach (GameObject door in Doors)
        {
            door.GetComponent<Animator>().SetBool("DoorState", _doorIsOpen);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _doorIsOpen = false;

        foreach (GameObject door in Doors)
        {
            door.GetComponent<Animator>().SetBool("DoorState", _doorIsOpen);

            print("close doooor???? ");
        }
    }

    public void Activate()
    {
        _activateDoor = true;
    }
}
