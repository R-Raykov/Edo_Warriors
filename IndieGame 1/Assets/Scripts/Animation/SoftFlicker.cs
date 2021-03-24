using UnityEngine;
using System.Collections;

public class SoftFlicker : MonoBehaviour
{
    [SerializeField] private Vector2 lightIntensityRange = new Vector2(1.0f, 2.5f);
    [SerializeField] private float intensityFlickerSpeed = 20f;
    [SerializeField] private float positionFlickerSpeed = 0.12f;

    private Vector3 randomPos;
    private Light light;
    private float newIntensity;

    private float TimeSinceRandomRefresh = 9999.0f;

    //public float 
    private void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    private void Update()
    {
        setRandomPos(0f);
        RandomLerpPos(positionFlickerSpeed);
        RandomLerpLight(intensityFlickerSpeed);
    }


    private void RandomLerpPos(float speed)
    {
        Vector3 newPos = Vector3.Lerp(transform.position, randomPos, Time.deltaTime * speed);
        transform.position = newPos;
    }

    private void RandomLerpLight(float speed)
    {
        float intensity = Mathf.Lerp(light.intensity, newIntensity, Time.deltaTime * speed);
        light.intensity = intensity;
    }

    private void setRandomPos(float interval)
    {
        if (TimeSinceRandomRefresh > interval)
        {
            randomPos = Random.insideUnitSphere * 2;
            randomPos += transform.position;
            newIntensity = Random.Range(lightIntensityRange.x, lightIntensityRange.y);
            //Debug.Log(randomPos);
            TimeSinceRandomRefresh = 0.0f;
        }
        else
        {
            TimeSinceRandomRefresh += Time.deltaTime;
        }
    }
}
