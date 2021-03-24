using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM<T> where T : AbstractEnemyAgent
{
    // Maps the class name of a state to a specific instance of that state
    private Dictionary<Type, AbstractState<T>> _stateCache = new Dictionary<Type, AbstractState<T>>();

    // The current state we are in
    private AbstractState<T> _currentState;

    // Reference to our target so we can pass into our new states
    private T _target;

    public EnemyFSM(T pTarget)
    {
        _target = pTarget;
        getAgentStates<AbstractState<T>>();
    }

    // Changes the state if the cache has it
    public void ChangeState<U>() where U : AbstractState<T>
    {
        // Check is a state like this is already in the cache
        if (!_stateCache.ContainsKey(typeof(U)))
        {
            Debug.LogWarning("Trying to access a state that is not a component of this object!");
            return;
        }
        AbstractState<T> newState = _stateCache[typeof(U)];
        changeState(newState);
    }

    // Changes the state
    private void changeState(AbstractState<T> pNewState)
    {
        if (_currentState == pNewState) return;

        if (_currentState != null) _currentState.Exit();
        _currentState = pNewState;
        if (_currentState != null) _currentState.Enter();
    }



    // Gets all the components of type U in the agent, and adds them to the cache of states
    private void getAgentStates<U>() where U : AbstractState<T>
    {
        foreach (U state in _target.GetComponents<U>())
        {
            if (!_stateCache.ContainsKey(typeof(U)))
            {
                _stateCache.Add(state.GetType(), state);
                state.enabled = false;
            }
        }
    }

    // Updates the state
    public void Update()
    {
        if (_currentState != null)
            _currentState.Step();
    }

    public AbstractState<T> GetCurrentState()
    {
        return _currentState;
    }

}
