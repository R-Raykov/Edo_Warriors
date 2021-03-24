using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IActivatable
{
    [Tooltip("Set which player can pick this item up")]
    [SerializeField] private ItemID _playerID = ItemID.Any; 
    [SerializeField] private float _targetScale = 0.5f;

    private Vector3 _targetScaleVector = Vector3.zero;
    private float _distanceSpeed = 0;
    private float _scaleSpeed = 0;
    private bool _activated = false;
    private CharacterStats _player1 = null;
    private CharacterStats _player2 = null;

    private bool _p1Taken = false;
    private bool _p2Taken = false;

    private bool _spawnDelayOver = false;
    private bool _collectAfterTimeStarted = false;

    private void Start()
    {
        _player1 = GameManager.Instance.Player1;
        _player2 = GameManager.Instance.Player2;
        _activated = false;

        StartCoroutine(spawnDelay());
    }

    private void Update()   // FIX THIS MAKE IT LESS HACKY
    {
        float distance;
        switch(_playerID)
        {
            case ItemID.P1:
                if (_player1 == null) return;
                distance = Vector3.SqrMagnitude(transform.position - _player1.transform.position);
                if (distance < 2f) _activated = true;
                if (_activated) absorb(_player1, distance);
                break;
            case ItemID.P2:
                if (_player2 == null) return;
                distance = Vector3.SqrMagnitude(transform.position - _player2.transform.position);
                if (distance < 2f) _activated = true;
                if (_activated) absorb(_player2, distance);
                break;
            case ItemID.Any:

                float distance1 = Vector3.SqrMagnitude(transform.position - _player1.transform.position);
                float distance2 = Vector3.SqrMagnitude(transform.position - _player2.transform.position);

                if (distance1 < 2f)
                {
                    _activated = true;
                    _p1Taken = true;
                }
                else if (distance2 < 2f)
                {
                    _activated = true;
                    _p2Taken = true;
                }

                if (_activated && _p1Taken) absorb(_player1, distance1);
                else if (_activated && _p2Taken) absorb(_player2, distance2);
                break;
        }


    }

    //private void FixedUpdate()
    private void absorb(CharacterStats target, float distance)
    {
        if (_spawnDelayOver == false) return;
        if(_collectAfterTimeStarted == false) StartCoroutine(collectAfterTime(target));

        _distanceSpeed = distance * 5;
        _scaleSpeed = (transform.localScale.magnitude - _targetScale) / (distance * 0.5f);

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position + Vector3.up, _distanceSpeed * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScaleVector, _scaleSpeed * Time.deltaTime);
        //transform.localScale = Vector3.MoveTowards(transform.localScale, _targetScaleVector, _scaleSpeed * Time.deltaTime);

        if ((distance < 0.1f + float.Epsilon)) collect(target);
    }

    private IEnumerator spawnDelay()
    {
        yield return new WaitForSeconds(1);
        _spawnDelayOver = true;
    }

    private IEnumerator collectAfterTime(CharacterStats target)
    {
        _collectAfterTimeStarted = true;
        yield return new WaitForSeconds(2f);
        collect(target);
    }

    private void collect(CharacterStats player)
    {
        player.ModifyKarma(25);
        Destroy(gameObject);
    }

    public void Activate()
    {
        _activated = true;
        _targetScaleVector = Vector3.one * _targetScale;
    }

}

public enum ItemID { Any, P1, P2 };