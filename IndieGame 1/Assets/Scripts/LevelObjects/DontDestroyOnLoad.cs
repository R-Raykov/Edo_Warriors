using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static Dictionary<string, GameObject> PersistentObjects = new Dictionary<string, GameObject>();

	// Use this for initialization
	void OnEnable () {
        if(!PersistentObjects.ContainsKey(this.gameObject.name))
        {
            PersistentObjects.Add(this.gameObject.name, this.gameObject);
            DontDestroyOnLoad(this.gameObject);
        }
	}

    private void Awake()
    {
        if (PersistentObjects.ContainsKey(this.gameObject.name))
        {
            Destroy(this.gameObject);
        }
    }

    public void Clear()
    {
        foreach(KeyValuePair<string, GameObject> entry in PersistentObjects)
        {
            Destroy(entry.Value);
        }
        PersistentObjects.Clear();
    }
}
