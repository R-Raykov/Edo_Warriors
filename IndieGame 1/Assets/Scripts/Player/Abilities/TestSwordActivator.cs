using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stupid fix that enables the sword after 1 frame
/// </summary>
public class TestSwordActivator : MonoBehaviour
{
    public GameObject sword;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(testc());
	}
	
    private IEnumerator testc()
    {
        yield return null;
        sword.SetActive(true);
    }
}
