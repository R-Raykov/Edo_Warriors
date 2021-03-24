using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DetectionTrigger : MonoBehaviour {

    private AbstractEnemyAgent _agent;
    private float _triggerRadius;

    private void Start()
    {
        _agent = GetComponentInParent<AbstractEnemyAgent>();
        _triggerRadius = GetComponent<SphereCollider>().radius;

        Debug.Assert(_agent != null, "There's no enemy agent as parent of this object", this);
        _agent.OnAlertRange += test;
    }

    private void test(CharacterStats p)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats player = other.GetComponent<CharacterStats>();

        if(player != null)
        {
            //Debug.Log("Enemy Alerted! : ", this.gameObject);
            if(_agent.OnAlertRange != null) _agent.OnAlertRange.Invoke(player);
        }
    }

    public float TriggerRadius
    {
        get { return _triggerRadius; }
    }
}
