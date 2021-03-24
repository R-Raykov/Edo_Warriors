using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUISwapper : MonoBehaviour
{
    [SerializeField] private PlayerClass _playerClass;

    private Dictionary<PlayerClass, PlayerUISet> _uiSets = new Dictionary<PlayerClass, PlayerUISet>();
    private PlayerUISet _currentSet;

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            PlayerUISet set = transform.GetChild(i).GetComponent<PlayerUISet>();
            if (set != null) _uiSets.Add(set.GetClassID(), set);
        }

        _currentSet = _uiSets[PlayerClass.Defaut];
    }

    public void SwapUISet(PlayerClass pClass)
    {
        _currentSet.gameObject.SetActive(false);
        _currentSet = _uiSets[pClass];
        _currentSet.gameObject.SetActive(true);
    }

    public PlayerUISet GetCurrentUISet()
    {
        return _currentSet;
    }
}
