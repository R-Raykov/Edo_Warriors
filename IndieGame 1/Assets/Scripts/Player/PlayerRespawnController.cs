using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnController : MonoBehaviour
{
    [Tooltip("The current place in the world where the player will respawn")]
    [SerializeField] private Transform _checkpoint;

    private Vector3 _playerSpawner;

    /// <summary>
    /// Deactivates the player and waits some time before activating it back
    /// </summary>
    /// <param name="time">Time before the player is back alive</param>
    /// <returns></returns>
    public IEnumerator StartRespawn(CharacterStats player, float time)
    {
        Debug.Log("DEATH");
        if (player == null) yield break;

        player.gameObject.SetActive(false);
        GameManager.Instance.MainCamera.GetComponent<CoopCamera>().Constrain = false;
        yield return new WaitForSeconds(time);
        RespawnPlayer(player);
        yield return new WaitForSeconds(1);
        GameManager.Instance.MainCamera.GetComponent<CoopCamera>().Constrain = true;
    }

    /// <summary>
    /// Finds the current respawn point, activates and sets the position of the player to it.
    /// </summary>
    public void RespawnPlayer(CharacterStats player)
    {
        player.gameObject.SetActive(true);

        if (player == GameManager.Instance.Player1 && GameManager.Instance.Player2 != null && GameManager.Instance.Player2.gameObject.activeInHierarchy)
        {
            player.transform.position = GameManager.Instance.Player2.transform.position + Vector3.left;
            if(Physics.Linecast(player.transform.position, -player.transform.up * 2))
            {
                player.transform.position = GameManager.Instance.Player2.transform.position + Vector3.right;
            }
        }
        else if (player == GameManager.Instance.Player2 && GameManager.Instance.Player1 != null && GameManager.Instance.Player1.gameObject.activeInHierarchy)
        {
            player.transform.position = GameManager.Instance.Player1.transform.position + Vector3.right;
            if (Physics.Linecast(player.transform.position, -player.transform.up * 2))
            {
                player.transform.position = GameManager.Instance.Player1.transform.position + Vector3.left;
            }
        }
        else
        {
            Debug.Log("On checkpoint");
            if (_checkpoint == null)
            {
                Debug.LogWarning("Missing respawn point reference");
                player.transform.position = Vector3.zero;
                return;
            }

            player.transform.position = _checkpoint.transform.position;
        }

        if(player.OnRespawn != null) player.OnRespawn.Invoke(player);

        // PLAY RESPAWN SOUND
        //player.sounds.PlayRespawn();

        //Get normalized orientation
        //Vector3 orientation = Vector3.ClampMagnitude(
        //    _respawnPoint.transform.GetComponentInParent<Checkpoint>().orientation,
        //    1);

        //Set position

        ////Set player orientation
        //player.GetComponent<Movement>().SetOrientation(orientation);

        //player.CanJump = true;
    }

    public Transform CheckpointSpawnPoint
    {
        get { return _checkpoint; }
        set { _checkpoint = value; }
    }
}
