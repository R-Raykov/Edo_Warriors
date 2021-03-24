using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [Tooltip("Player to follow")]
    public GameObject Player1;

    [Tooltip("Second player")]
    public GameObject Player2;

    private Vector3 offset;

    private Vector3 twoPlayerCam;

    private void Start ()
    {
        offset = transform.position - Player1.transform.position;
	}

    private void LateUpdate()
    {
        if (Player1.activeSelf && Player2.activeSelf)
        {
            twoPlayerCam = ((Player2.transform.position - Player1.transform.position) / 2.0f) + Player1.transform.position;
            //transform.position = Vector3.Lerp(transform.position, new Vector3(twoPlayerCam.x, transform.position.y, twoPlayerCam.z), Time.deltaTime);
            transform.position = new Vector3(twoPlayerCam.x, transform.position.y, transform.position.z);
        }

        else if (Player1.activeSelf && !Player2.activeSelf) transform.position = Player1.transform.position + offset;
        else if (!Player1.activeSelf && Player2.activeSelf) transform.position = Player2.transform.position + offset;
    }
}
