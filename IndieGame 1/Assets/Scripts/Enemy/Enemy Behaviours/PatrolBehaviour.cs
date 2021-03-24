using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : PassiveBehaviour
{
    [Tooltip("The points of the path the agent will move through")]
    [SerializeField] private Transform[] _points;

    [Tooltip("Should the object traverse the waypoints in a random order?")]
    [SerializeField] private bool _randomSequence;

    [Tooltip("Should the agent stop at each waypoint or move continuously?")]
    [SerializeField] private bool _ShouldStopAtWaypoint;

    [Tooltip("Time range that the agent will stop at a waypoint")]
    [SerializeField] private Vector2 _waypointStopTime;

    [Tooltip("Turn on or off path visualization in scene")]
    [SerializeField] private bool _drawGizmos;

    private int _destPoint = 0;
    private bool _recalculating = false;

    private void Start()
    {
        // Disabling auto-braking allows for continuous movement
        _navAgent.autoBraking = false;

        Debug.Assert(_points.Length > 0, "The path for this agent is empty", this);

        GotoNextPoint();
    }

    private void OnDrawGizmos()
    {
        if (_drawGizmos)
        {

            for (int i = 0; i < _points.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(_points[i].position, _points[(i + 1) % _points.Length].position);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_points[i].position, 0.1f);
            }
        }
    }


    private void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (_points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        _navAgent.destination = _points[_destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        if(_randomSequence) _destPoint = Random.Range(0, _points.Length);
        else                _destPoint = (_destPoint + 1) % _points.Length;
    }

    private IEnumerator delay(float time)
    {
        _recalculating = true;
        yield return new WaitForSeconds(time);
        _recalculating = false;
        GotoNextPoint();
    }

    public override void UpdateAgent()
    {
        if (_recalculating) return;

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!_navAgent.pathPending && _navAgent.remainingDistance < 0.5f)
        {
            if (!_ShouldStopAtWaypoint)
            {
                GotoNextPoint();
            }
            else
            {
                // Waits a random ammount, then calls GotoNextPoint
                StartCoroutine(delay(Random.Range(_waypointStopTime.x, _waypointStopTime.y)));
            }
        }
    }
}
