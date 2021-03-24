using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private bool _drawCapsule = true;

    [SerializeField] private float _interactRadius = 0;
    [SerializeField] private float _interactDistanceDown = 0;
    [SerializeField] private float _interactDistanceFromPlayer = 0;

    private CharacterStats _player;
    private int _playerNumber;

    private void Start()
    {
        _player = GetComponent<CharacterStats>();
        _playerNumber = _player.PlayerNumber;
    }

    public void OnDrawGizmos()
    {
        if (_drawCapsule)
        {
            Vector3 relativeLocation = transform.position + transform.forward * _interactDistanceFromPlayer;

            Gizmos.DrawWireSphere(relativeLocation, _interactRadius);
            Gizmos.DrawWireSphere(relativeLocation + Vector3.down * _interactDistanceDown, _interactRadius);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("XJ" + _playerNumber))
        {
            Activate();
        }
    }

    /// <summary>
    /// Activates the closest available object (within the Interact radius) that can be activated and returns true if at least one has been found. Otherwise false.
    /// </summary>
    /// <returns>True if at least one ActivateComponent within the Interact radius has been found. Otherwise false.</returns>
    public bool Activate()
    {
        IInteractable interactComponent = GetClosestInteractable();
        if (interactComponent != null)
        {
            interactComponent.Interact(_player);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the closest Activatable component, provided the object has a collider to check against.
    /// </summary>
    /// <typeparam name="T">The type of Component being searched for.</typeparam>
    /// <returns>The Component if one has been found. Otherwise null.</returns>
    public IInteractable GetClosestInteractable()
    {
        //Get search location and a capsule around it pointing down
        Vector3 relativeLocation = transform.position + transform.forward * _interactDistanceFromPlayer;
        Collider[] colliders = Physics.OverlapCapsule(
            relativeLocation, relativeLocation + Vector3.down * _interactDistanceDown, _interactRadius);

        //Iterate over colliders found (if any) and find the closest one.
        if (colliders.Length != 0)
        {
            float minDistance = 1000;
            IInteractable closestT = null;

            for(int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];

                IInteractable objectT = collider.GetComponent<IInteractable>();
                if (objectT != null)
                {
                    //Horizontal Positions
                    Vector3 colliderHorzPos = collider.transform.position;
                    colliderHorzPos.y = 0;
                    Vector3 transformHorzPos = transform.position;
                    transformHorzPos.y = 0;

                    //Least Distance checks
                    float distance = (colliderHorzPos - transformHorzPos).magnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestT = objectT;
                    }
                }
            }

            return closestT;
        }

        return null;
    }
}
