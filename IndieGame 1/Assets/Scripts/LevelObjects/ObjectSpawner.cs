using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour, IActivatable
{
    [Tooltip("The type of object to spawn")]
    [SerializeField] private GameObject _gameObject;
    [Tooltip("The parent for the spawned objects")]
    [SerializeField] private GameObject _parent;

    [Header("Spawn Parameters")]

    [Tooltip("The type of spawning to do")]
    [SerializeField] private SpawnType _spawnType = SpawnType.Point;
    [Tooltip("Ammount of objects to spawn")]
    [SerializeField] private int _nObjects;
    [Tooltip("The interval in seconds between spawns")]
    [SerializeField] private float _spawnTimer = 5f;

    [Tooltip("Should this be activated at start?")]
    [SerializeField] private bool _activateAtStart = false;
    [Tooltip("Should the objects keep spawning after the interval")]
    [SerializeField] private bool _repeating;

    private Bounds bounds;
    private float maxTimer;
    private enum SpawnType { Point, Area };

    // Use this for initialization
    void Start ()
    {
        bounds = GetComponent<Collider>().bounds;
        if(_activateAtStart) Activate();
	}

    public void Activate()
    {
        StartCoroutine(spawnTimer());
    }
	
    private IEnumerator spawnTimer()
    {
        yield return new WaitForSeconds(_spawnTimer);
        SpawnObjects();

        if (_repeating) StartCoroutine(spawnTimer());
    }

    /// <summary>
    /// Calls the relevant method that spawns objects
    /// </summary>
    public void SpawnObjects()
    {
        if (_spawnType == SpawnType.Point)          StartCoroutine(SpawnObjectPoint(_nObjects, transform.position));
        else if (_spawnType == SpawnType.Area)      StartCoroutine(SpawnObjectArea(_nObjects));
    }

    #region EnemyInstancingCoroutines
    public IEnumerator SpawnObjectPoint(int number, Vector3 position)
    {
        if (_parent == null) {
            for (int i = 0; i < number; i++) {
                Instantiate(_gameObject, position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
        } else {
            for (int i = 0; i < number; i++) {
                Instantiate(_gameObject, position, Quaternion.identity, _parent.transform);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public IEnumerator SpawnObjectArea(int number)
    {
        if (_parent == null) {
            for (int i = 0; i < number; i++) {
                Instantiate(_gameObject, RandomPointInBox(), Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
        } else {
            for (int i = 0; i < number; i++) {
                Instantiate(_gameObject, RandomPointInBox(), Quaternion.identity, _parent.transform);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    #endregion

    public Vector3 RandomPointInBox()
    {
        return bounds.center + new Vector3 (
                                           (Random.value - 0.5f) * bounds.size.x,
                                           (transform.position.y),
                                           (Random.value - 0.5f) * bounds.size.z
                                           ); 
    }

    /// <summary>
    /// Gets and Sets the time between spawning objects
    /// </summary>
    public float SpawnTimer {
        get { return _spawnTimer; }
        set { _spawnTimer = value; }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
