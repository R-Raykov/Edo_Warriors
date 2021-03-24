using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region Stats
    // Region used to store global game stats
    #endregion

    #region Players

    public static int nPlayers = 0;

    /// <summary>
    /// To be called when a player connects, assigns the new player to P1 or P2
    /// </summary>
    /// <param name="player"></param>
    public void RegisterPlayer(CharacterStats player)
    {
        nPlayers = Mathf.Clamp(++nPlayers, 0, 2);
        UnityEngine.Debug.Log("REGISTERED P number: " + nPlayers);

        if (nPlayers == 1) Player1 = player;
        else if (nPlayers == 2) Player2 = player;

        player.OnDeath += PlayerDeathHandler;
    }

    /// <summary>
    /// To be called when the player disconnects, frees the reference to the player
    /// </summary>
    /// <param name="player"></param>
    public void UnregisterPlayer(CharacterStats player)
    {
        player.OnDeath -= PlayerDeathHandler;

        if (player == Player2) Player2 = null;
        else if (player == Player1) Player1 = null;

        nPlayers = Mathf.Clamp(--nPlayers, 0, 2);
        UnityEngine.Debug.Log("UNREGISTERED P number: " + nPlayers);
    }

    private CharacterStats _player1;
    /// <summary>
    /// Reference to player one
    /// </summary>
    public CharacterStats Player1
    {
        get { return _player1; }
        set
        {
            Debug.LogWarning("SETTING PLAYER 1");
            _player1 = value;
        }
    }

    private CharacterStats _player2;
    /// <summary>
    /// Reference to player two
    /// </summary>
    public CharacterStats Player2
    {
        get { return _player2; }
        set
        {
            Debug.LogWarning("SETTING PLAYER 2");
            _player2 = value;
        }
    }

    // Reference that holds the Respawn controller
    private PlayerRespawnController _playerRespawnController;
    public PlayerRespawnController PlayerRespawnController
    {
        get
        {
            if (_playerRespawnController == null && !isQuitting && _gameObject != null)
            {
                _playerRespawnController = _gameObject.GetComponent<PlayerRespawnController>();
            }
            return _playerRespawnController;
        }
    }

    /// <summary>
    /// Handles what happens when the player dies. Starts the player respawn timer
    /// </summary>
    private void PlayerDeathHandler(CharacterStats player)
    {
        if (PlayerRespawnController != null && _instance != null && Instance._gameObject.activeInHierarchy)
        {
            PlayerRespawnController.StartCoroutine(PlayerRespawnController.StartRespawn(player, player.RespawnTime));
        }
    }
    #endregion

    #region References

    private Camera _mainCam;
    public Camera MainCamera
    {
        get { return _mainCam; }
        set { _mainCam = value; }
    }

    private LoadLevel _levelLoader;
    public LoadLevel LevelLoader
    {
        get { return _levelLoader; }
        set { _levelLoader = value; }
    }

    #endregion

    #region Self
    private GameObject _gameObject;
    public GameObject GameObject
    {
        get { return _gameObject; }
    }

    // Singleton Reference to this class
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
                _instance._gameObject = new GameObject("_gameManager");
                _instance._gameObject.AddComponent<PlayerRespawnController>();
                GameObject.DontDestroyOnLoad(_instance._gameObject);
            }
            return _instance;
        }
    }

    public static bool isQuitting = false;
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDisable()
    {
        Object.Destroy(_instance.GameObject);
        _instance = null;
    }

    public void Delete()
    {
        Object.Destroy(_instance.GameObject);
        _instance = null;
    }
    #endregion
}
