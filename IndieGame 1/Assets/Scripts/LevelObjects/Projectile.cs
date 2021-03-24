using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2.5f;
    [SerializeField] private float _speed = 75f;

    private float _damage = 2f;
    public ProjectileID ID;

    //private AudioSource source;
    //[SerializeField] private AudioClip[] shots;

    void Start()
    {


        //Enemy parent = GetComponentInParent<Enemy>();
        //if (parent != null)
        //    _damage = GetComponentInParent<Enemy>().Damage;
        //else
        //{
        //    _damage = 0;
        //    Debug.LogError("Component needs to be a child of an object with an Enemy component. Damage set to default 0. ", this);
        //}

        Destroy(gameObject, _lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged"))
        {
            Debug.Log(other.gameObject, other.gameObject);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Gets and sets the damage, set by the enemy class at instantiation
    /// </summary>
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

}

public enum ProjectileID { None, Player, Enemy, Friendly };
