using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinExplode : MonoBehaviour
{
    [Tooltip("Ammount of karmaPoints to drop")]
    [SerializeField] private Vector2 _rangeAmmount;

    [Header("References")]
    [SerializeField] private GameObject _explosionObject;
    [SerializeField] private GameObject _p1Coins;
    [SerializeField] private GameObject _p2Coins;

    private bool _safe = true;

    public void Generate(AbstractEnemyAgent enemy)
    {
        if (_safe) {
            GameObject instance = Instantiate(_explosionObject, transform.position, Quaternion.identity);
            instance.GetComponent<EnemyDeathObject>().Generate(_rangeAmmount, _p1Coins, _p2Coins);
        }
    }

    private void OnApplicationQuit()
    {
        _safe = false;
    }
}
