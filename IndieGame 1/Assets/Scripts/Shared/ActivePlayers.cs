using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivePlayers : MonoBehaviour {

    public GameObject p1;
    public GameObject p2;

    [SerializeField] private GameObject P1Join;
    [SerializeField] private GameObject P2Join;

    private float deactivationTimer = 0;

    private bool canJoin;

	private void Start ()
    {
        //p1 = GameObject.Find("player1");
        //p2 = GameObject.Find("player2");
        //GameManager.Instance.RegisterPlayer(p1.GetComponent<CharacterStats>());
        //GameManager.Instance.RegisterPlayer(p2.GetComponent<CharacterStats>());
        p1.SetActive(false);
        p2.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(UnityEngine.SceneManagement.SceneManager.sceneCount);
        }

        PlayerConnects();
        PressStart();
    }

    private void PlayerConnects()
    {
        //if (!canJoin) return;
        if (SceneManager.GetActiveScene().name != "00_Central_Hub") return;     //sorry Rodrigo, the canJoin isnt working and we are on a clock

        if (Input.GetKeyDown(KeyCode.Joystick1Button7) && p1.activeSelf == false)
        {
            print("hello p1");
            GameManager.Instance.RegisterPlayer(p1.GetComponent<CharacterStats>());
            p1.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button7) && p2.activeSelf == false)
        {
            print("hello p2");
            GameManager.Instance.RegisterPlayer(p2.GetComponent<CharacterStats>());
            p2.SetActive(true);
        }
    }

    private void PressStart()
    {
        if (SceneManager.GetActiveScene().name != "00_Central_Hub")
        {
            P1Join.gameObject.SetActive(false);
            P2Join.gameObject.SetActive(false);
            return;
        }

        if (GameManager.Instance.Player1 == null) P1Join.gameObject.SetActive(true);
        else P1Join.gameObject.SetActive(false);
        if (GameManager.Instance.Player2 == null) P2Join.gameObject.SetActive(true);
        else P2Join.gameObject.SetActive(false);
    }

    public bool CanJoin
    {
        get { return canJoin; }
        set { canJoin = value; }
    }
}
