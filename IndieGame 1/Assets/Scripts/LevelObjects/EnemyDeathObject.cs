using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathObject : MonoBehaviour
{
    private Vector2 _rangeAmmount;
    private GameObject _p1Coins;
    private GameObject _p2Coins;

    public void Generate(Vector2 a, GameObject one, GameObject two)
    {
        _rangeAmmount = a;
        _p1Coins = one;
        _p2Coins = two;
        StartCoroutine(releaseLoot((int)Random.Range(_rangeAmmount.x, _rangeAmmount.y)));
    }

    private IEnumerator releaseLoot(int ammount)
    {
        Vector3 rotVector = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Vector3.right;

        for (int i = 0; i < ammount; i += i < ammount ? 1 : 0)
        {
            //if (i > 12) dist = 3f;
            float dist = Random.Range(0.5f, 2f);
            float height = Random.Range(2.5f, 4f);

            GameObject obj1 = Instantiate(_p1Coins, transform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            obj1.GetComponent<Rigidbody>().AddForce((Vector3.up * height + rotVector * dist), ForceMode.Impulse);

            // Duplicate item and flip the direction it's launched to
            yield return null;

            GameObject obj2 = Instantiate(_p2Coins, transform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            obj2.GetComponent<Rigidbody>().AddForce((Vector3.up * height + -rotVector * dist), ForceMode.Impulse);


            // Rotate the rotation for next iterations
            yield return null;
            rotVector = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * rotVector;
        }

        Destroy(gameObject);
    }

}
