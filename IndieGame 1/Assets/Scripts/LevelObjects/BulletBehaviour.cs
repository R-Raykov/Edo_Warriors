using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    public ParticleSystem PC;

    private Rigidbody rb;

    private float speed = 15;
    private float dmg = 15;

    private bool shoot;

    private float destroyT;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        destroyT += Time.deltaTime;
        if (destroyT > 10) Destroy(gameObject);

        if (!shoot)
        {
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
            shoot = true;
        }
    }

    public float Damage
    {
        get { return dmg; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            ParticleSystem p = Instantiate(PC, null);
            p.transform.position = transform.position;
            Destroy(gameObject);
        }

        Rigidbody otherRb = other.GetComponent<Rigidbody>();
        if(otherRb != null && other.GetComponent<Enemy>() == null)
        {
            otherRb.AddForce(transform.forward * 5f, ForceMode.Impulse);
        }
    }
}
