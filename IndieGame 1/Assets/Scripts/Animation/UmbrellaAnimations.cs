using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaAnimations : MonoBehaviour {

    [SerializeField] private Animator anime;

    private bool attacking;
    private float attackTimer;

    private AbstractEnemyAgent agent;

    private void Start ()
    {
        agent = GetComponentInParent<Enemy>();
        agent.OnDamageTaken += TakeDamage;
    }
	
	private void Update ()
    {
        if (attacking) attackTimer += Time.deltaTime;
        if (attackTimer >= 1.4f)
        {
            attackTimer = 0;
            attacking = false;
            anime.SetFloat("Attack", 0);
        }
    }

    public void AttackAnimation()
    {
        print("Attack");
        anime.SetFloat("Attack", Random.Range(1, 3));
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
