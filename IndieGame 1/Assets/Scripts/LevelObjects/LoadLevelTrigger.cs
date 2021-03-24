using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelTrigger : MonoBehaviour
{
    [SerializeField] private string _levelToLoad;
    private bool _loading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterStats>() != null)
        {
            if (!_loading)
            {
                _loading = true;
                GameManager.Instance.LevelLoader.LoadScene(_levelToLoad);
            }
        }
    }
}
