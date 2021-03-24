using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollDecompose : MonoBehaviour
{
    [SerializeField] private float _startDecomposeTime = 3f;
    [SerializeField] private float _decomposeDestroyTime = 5f;

    // References
    private Rigidbody _parent;
    private Rigidbody[] _ragdoll;
    private Collider[] _colliders;
    private Animator _animator;

	// Use this for initialization
	void Start () {
        _parent = GetComponentInParent<Rigidbody>();
        _ragdoll = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        _animator = GetComponent<Animator>();


        // To deacticvate it actually, assuming rigidbodies were added in default state
        for (int i = 0; i < _ragdoll.Length; i++)
        {
            _ragdoll[i].isKinematic = true;
            _ragdoll[i].useGravity = false;
            _colliders[i].enabled = false;
        }

    }

    public void Activate(AbstractEnemyAgent p)
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

        StartCoroutine(decompose());
    }

    private IEnumerator decompose()
    {
        yield return new WaitForSeconds(_startDecomposeTime);

        for (int i = 0; i < _ragdoll.Length; i++)
        {
            _ragdoll[i].isKinematic = true;
            _ragdoll[i].useGravity = false;
            _colliders[i].enabled = false;
        }

        while (_decomposeDestroyTime > 0)
        {
            _decomposeDestroyTime -= Time.deltaTime;
            transform.Translate(Vector3.down * 0.001f);
            yield return null;
        }
        Destroy(gameObject);
    }

    
    //public void ResetBody(CharacterStats p)
    //{
    //    for (int i = 0; i < _ragdoll.Length; i++)
    //    {
    //        _ragdoll[i].isKinematic = true;
    //        _ragdoll[i].useGravity = false;
    //        _colliders[i].enabled = false;
    //    }

    //    transform.parent = _parent.transform;
    //    transform.localPosition = _originalPos;
    //    _animator.enabled = true;
    //}
}
