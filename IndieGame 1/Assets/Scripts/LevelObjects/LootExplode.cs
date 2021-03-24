using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootExplode : MonoBehaviour
{
    public LootDrop[] loot;

    public void Generate(int value)
    {
        StartCoroutine(releaseLoot(value * 0.01f));
    }

    private IEnumerator releaseLoot(float chanceModifier)
    {
        Vector3 rotVector = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Vector3.right;

        for (int i = 0; i < loot.Length; i++)
        {
            for (int j = 0; j < loot[i].ammount; j += j < loot[i].ammount ? 1 : 0)
            {
                //if (i > 12) dist = 3f;
                float dist = Random.Range(0.5f, 2f);
                float height = Random.Range(2.5f, 4f);


                // Percentage chance
                if (Random.Range(0, 99) < loot[i].dropChance + chanceModifier)
                {
                    GameObject obj1 = Instantiate(loot[i].lootItem, transform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
                    obj1.GetComponent<Rigidbody>().AddForce((Vector3.up * height + rotVector * dist), ForceMode.Impulse);
                }

                // Go to next item and flip the direction it's launched to
                yield return null;
                j += j < loot.Length ? 1 : 0;

                // Percentage chance
                if (Random.Range(0, 99) < loot[i].dropChance + chanceModifier)
                {
                    GameObject obj2 = Instantiate(loot[i].lootItem, transform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
                    obj2.GetComponent<Rigidbody>().AddForce((Vector3.up * height + -rotVector * dist), ForceMode.Impulse);
                }

                // Rotate the rotation for next iterations
                yield return null;
                rotVector = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * rotVector;
            }
        }
    }

}

[System.Serializable]
public class LootDrop
{
    [Tooltip("Name of the item for the inspector only")]
    public string name;

    [Tooltip("Prefab item to drop")]
    public GameObject lootItem;

    [Tooltip("Percentage chance to drop this object")]
    [Range(0, 100)]
    public float dropChance = 100;

    [Tooltip("How many of this item to drop")]
    public int ammount = 1;
}
