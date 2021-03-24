using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBlast : MonoBehaviour {

    public GameObject AttackParticle;

    private List<GameObject> enemiesInRange;

    private int enemiesToHit;
    private int maxEnemie = 5;

    public bool detect;

    private void Awake ()
    {
        enemiesInRange = new List<GameObject>();
	}

    private void FixedUpdate()
    {
        if (enemiesInRange.Count > 0 && !detect)
        {
            foreach (GameObject go in enemiesInRange)
            {
                print(enemiesInRange.Count);
                GameObject p = Instantiate(AttackParticle, go.transform.position, Quaternion.Euler(Vector3.zero), go.transform);
                p.transform.localPosition = Vector3.zero;

                p.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.2f, 0);
            }

            detect = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {

        //print("Tag of the collider " + col.gameObject.tag);
        //print("Enemies in Range " + enemiesInRange.Count);

        if (col.transform.GetComponent<Enemy>() != null)
        {
           if (enemiesInRange.Count < 5 && !enemiesInRange.Contains(col.gameObject)) enemiesInRange.Add(col.gameObject);
        }   

    }
}
