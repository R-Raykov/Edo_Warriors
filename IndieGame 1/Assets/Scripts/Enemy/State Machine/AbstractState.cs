using UnityEngine;

public class AbstractState<T> : MonoBehaviour
    where T : AbstractEnemyAgent
{
    public System.Action OnStateEnter;
    public System.Action OnStateExit;

    protected T _enemyAgent;

    // When the state is entered, enables the component
    public virtual void Enter()
    {
        this.enabled = true;
        OnStateEnter += Exit;

        if (OnStateEnter != null) OnStateEnter.Invoke();
    }

    public virtual void Step()
    {
        //if (GameManager.Instance.isPause)
        //    return;
    }

    // When the state is exited, disables the component
    public virtual void Exit()
    {
        this.enabled = false;
        if (OnStateExit != null) OnStateExit.Invoke();
    }
}
