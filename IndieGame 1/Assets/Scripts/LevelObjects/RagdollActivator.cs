using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    // References
    private Rigidbody _parent;
    private CharacterStats _player;
    private Rigidbody[] _ragdoll;
    private Collider[] _colliders;
    private Animator _animator;

    // Active variables
    private Vector3 _originalPos;
    private bool _isActive = false;

	// Use this for initialization
	void Start () {
        _player = GetComponentInParent<CharacterStats>();
        _parent = GetComponentInParent<Rigidbody>();
        _ragdoll = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        _animator = GetComponent<Animator>();
        _originalPos = transform.localPosition;

        // To deacticvate it actually, assuming rigidbodies were added in default state
        ResetBody(_player);
        _player.OnRespawn += ResetBody;
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.RightControl))
        {
            Activate(null);
        }
	}

    public void Activate(CharacterStats p)
    {
        _animator.StopPlayback();
        _animator.enabled = false;
        transform.parent = null;

        for(int i = 0; i < _ragdoll.Length; i++)
        {
            _ragdoll[i].isKinematic = false;
            _ragdoll[i].useGravity = true;
            _colliders[i].enabled = true;

            _ragdoll[i].AddForce(_parent.velocity * 1.5f, ForceMode.Impulse);
        }
    }

    public void ResetBody(CharacterStats p)
    {
        for (int i = 0; i < _ragdoll.Length; i++)
        {
            _ragdoll[i].isKinematic = true;
            _ragdoll[i].useGravity = false;
            _colliders[i].enabled = false;

        }

        transform.parent = _parent.transform;
        transform.localPosition = _originalPos;
        _animator.enabled = true;
    }

    private void OnDestroy()
    {
        _player.OnRespawn -= ResetBody;
    }
}
