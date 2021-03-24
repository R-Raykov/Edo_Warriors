using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCamController : MonoBehaviour
{

    public float speedFactor = 0.1f;
    public float zoomFactor = 1.0f;
    public Transform CurrentMount;
    public Transform DefaultMount;
    private Vector3 lastPosition;
    private Camera cameraComp;
    public GameObject MainCamera;
    public EventSystem eSystem;

    private void Start()
    {
        CurrentMount = DefaultMount;
        lastPosition = transform.position;
        cameraComp = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, CurrentMount.position, speedFactor);
        transform.rotation = Quaternion.Slerp(transform.rotation, CurrentMount.rotation, speedFactor);
        float velocity = Vector3.Magnitude(transform.position - lastPosition);
        cameraComp.fieldOfView = 60 + velocity * zoomFactor;
        lastPosition = transform.position;
    }

    public void SetMount(Transform newMount)
    {
        CurrentMount = newMount;
    }

    public void ActivateCanvas(GameObject Canvas)
    {
        Canvas.SetActive(true);
        eSystem.SetSelectedGameObject(Canvas.GetComponentInChildren<Button>().gameObject);
    }

    public void DeactivateCanvas(GameObject Canvas)
    {
        Canvas.SetActive(false);
    }

    public void StartGame(GameObject Menu)
    {
        MainCamera.SetActive(true);
        Menu.SetActive(false);
    }
}