using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MeleeRangeTrigger : MonoBehaviour {

    private AbstractEnemyAgent _agent;
    private float _triggerRadius;

    private void Start()
    {
        _agent = GetComponentInParent<AbstractEnemyAgent>();
        _triggerRadius = GetComponent<SphereCollider>().radius;

        Debug.Assert(_agent != null, "There's no enemy agent as parent of this object", this);
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats player = other.GetComponent<CharacterStats>();
        if (player != null)
        {
            //Debug.Log("Player in melee range! : ", this.gameObject);
            if (_agent.OnMeleeRange != null) _agent.OnMeleeRange.Invoke(player);
            _agent.InMelee = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterStats player = other.GetComponent<CharacterStats>();
        if (player != null)
        {
            //Debug.Log("Player out of melee range! : ", this.gameObject);
            _agent.InMelee = false;
        }
    }

    public float TriggerRadius
    {
        get { return _triggerRadius; }
    }

}
