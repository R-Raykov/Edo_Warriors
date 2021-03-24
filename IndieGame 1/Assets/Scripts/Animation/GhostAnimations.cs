using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimations : MonoBehaviour {

    private Animator anime;

    private AbstractEnemyAgent agent;

    private void Start ()
    {
        anime = GetComponent<Animator>();
        agent = GetComponentInParent<Enemy>();
        agent.OnDamageTaken += TakeDamage;
    }

    public void Attack()
    {
        anime.SetTrigger("Attack");
    }

    public void TakeDamage()
    {
        anime.SetTrigger("TakeDMG");
    }

    private void OnDisable()
    {
        agent.OnDamageTaken -= TakeDamage;
    }
}
