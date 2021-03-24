using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    private float timer;
    public float destroyAfter;

	private void Start ()
    {
		if(destroyAfter == 0) destroyAfter = 0.5f;
    }

    private void Update ()
    {
        timer += Time.deltaTime;
        if (timer > destroyAfter) Destroy(gameObject);
	}
}
