using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KappaAnimations : MonoBehaviour {

    private Animator anime;

    private bool attacking;
    private float attackTimer;

    private AbstractEnemyAgent agent;

    private void Start ()
    {
        anime = GetComponent<Animator>();
        agent = GetComponentInParent<Enemy>();
        agent.OnDamageTaken += TakeDamage;
	}

    private void Update ()
    {
        if (attacking) attackTimer += Time.deltaTime;
        if (attackTimer >= 0.6f)
        {
            attackTimer = 0;
            attacking = false;
            anime.SetFloat("AttackN", 0);
        }
        anime.GetFloat("AttackN");
	}

    public void AttackAnimation()
    {
        print("Attack");
        anime.SetFloat("AttackN", Random.Range(1,3));
        attacking = true;
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
