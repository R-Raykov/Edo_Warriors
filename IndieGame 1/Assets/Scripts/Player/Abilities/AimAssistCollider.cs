using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistCollider : MonoBehaviour
{
    private bool enemyInRange;
    private List<GameObject> enemyInCone;

    public bool Slow
    {
        get { return enemyInRange; }
    }

    private void Start()
    {
        enemyInCone = new List<GameObject>();
    }

    private void Update()
    {
        if (enemyInCone.Count != 0) enemyInRange = true;
        else enemyInRange = false;

        EnemyDeath();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy") && !enemyInCone.Contains(col.gameObject)) enemyInCone.Add(col.gameObject);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy") && enemyInCone.Contains(col.gameObject)) enemyInCone.Remove(col.gameObject);
    }

    private void EnemyDeath()
    {
        for (int i = enemyInCone.Count; i > 0; i--)
        {
            if (!enemyInCone[i - 1].activeInHierarchy) enemyInCone.RemoveAt(i - 1);
        }
    }
}
