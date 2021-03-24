using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour {

    private CharacterStats charStats;
    private BasicMovement playerMovement;
    private Animator anime;
    private Rigidbody rb;

    private float h;
    private float v;
    private float j;

    private float directionOffset;

	private void Start ()
    {
        rb = GetComponentInParent<Rigidbody>();
        charStats = GetComponentInParent<CharacterStats>();
        playerMovement = GetComponentInParent<BasicMovement>();
        anime = GetComponent<Animator>();
	}

    private void Update()
    {
        //print(rb.velocity);
        //if (rb.velocity.x > 0) directionOffset = rb.velocity.x * transform.right.x * -1;
        //else directionOffset = rb.velocity.x * transform.right.x;
        //print(rb.velocity.z * transform.forward.z + rb.velocity.x);
        // print(Input.GetAxis("HorizontalP" + charStats.PlayerNumber));
        //h = playerMovement.GetXMovement;
        // v = playerMovement.GetZMovement;
        h = rb.velocity.x * transform.right.x * -1 + rb.velocity.z * transform.right.z;
        v = rb.velocity.z * transform.forward.z + rb.velocity.x * transform.forward.x;
        j = rb.velocity.y;
        //print("h " + h + " v " + v);
        anime.SetFloat("Speed", v);
        anime.SetFloat("Direction", h);
        anime.SetFloat("Jump", j);
    }
}
