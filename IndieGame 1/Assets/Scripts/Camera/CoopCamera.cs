using UnityEngine;
using System.Collections;

public class CoopCamera : MonoBehaviour
{
    [Header("Targets to follow")]
    [SerializeField] private Transform _player1;
    [SerializeField] private Transform _player2;

    [Header("Camera properties")]

    [Tooltip("The scaling factor of the zoom")]
    [SerializeField] private float _zoomFactor = 1.5f;

    [Tooltip("The velocity of the smooth camera interpolation")]
    [SerializeField] private float _followTimeDelta = 0.8f;

    [Tooltip("The minimum and maximum ammount of zoom (the lower bound is also the minimum distance before the camera starts zooming out)")]
    [SerializeField] private Vector2 _zoomBounds = new Vector2(0.5f, 10f);


    private Camera _cam;
    private Vector3 _twoPlayerCam;
    private Vector3 _newCamPos;

    private float _zoomDistance = 0;
    private float _startOffset = 0;

    private bool _constrain = true;

	// Use this for initialization
	void Start ()
    {
        _cam = GetComponent<Camera>();
        GameManager.Instance.MainCamera = _cam;

        //GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        //for (int i = 0; i < cameras.Length; i++)
        //{
        //    if (cameras[i] != this.gameObject)
        //    {
        //        transform.position = cameras[i].transform.position;
        //        transform.rotation = cameras[i].transform.rotation;
        //        cameras[i].SetActive(false);
        //    }
        //}

        _newCamPos = transform.position;

        // Get the starting distance offset of the camera by raycasting the ground
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity, 1 << 0);
        _startOffset = hitInfo.distance - 0.5f; // Subtract the distance from the ground to the player's origin - 0.5
    }

    public void ReplaceExistingCamera()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != this.gameObject)
            {
                transform.position = cameras[i].transform.position;
                transform.rotation = cameras[i].transform.rotation;
                _newCamPos = cameras[i].transform.position;
                cameras[i].SetActive(false);
                //FixedUpdate();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Dual Camera
        if (_player1.gameObject.activeInHierarchy && _player2.gameObject.activeInHierarchy)
        {
            DualCameraFollow(_cam, _player1, _player2);
        }
        else
        { 
            // Single Camera
            if (!_player2.gameObject.activeInHierarchy && _player1.gameObject.activeInHierarchy)
                CameraFollow(_cam, _player1);

            if (!_player1.gameObject.activeInHierarchy && _player2.gameObject.activeInHierarchy)
                CameraFollow(_cam, _player2);
        }

        // Constrain the position of the players (is this the right place for this code?)
        if (_constrain)
        {
            ConstrainToView(_player1);
            ConstrainToView(_player2);
        }
    }

    /// <summary>
    /// Follows smoothly one object on the screen
    /// </summary>
    /// <param name="cam">The camera to use</param>
    /// <param name="t">The transform of the object to follow</param>
    public void CameraFollow(Camera cam, Transform t)
    {
        // Get the new position of the camera
        Vector3 cameraDestination = t.position - cam.transform.forward * _startOffset;
        //cameraDestination -= cam.transform.forward * _zoomDistance * _zoomFactor;

        // Update the position of the camera
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, _followTimeDelta * Time.deltaTime);

        // Snap when close enough to prevent annoying slerp behavior
        if ((_newCamPos - cam.transform.position).sqrMagnitude <= float.Epsilon)
            cam.transform.position = _newCamPos;
    }

    /// <summary>
    /// Follows smoothly two objects on the screen, centered on their midpoint and zooming out when they get to far away
    /// </summary>
    /// <param name="cam">The camera to use</param>
    /// <param name="t1">The first object to follow</param>
    /// <param name="t2">The second object to follow</param>
    public void DualCameraFollow(Camera cam, Transform t1, Transform t2)
    {
        // Get the distance between objects
        Vector3 distanceVector = t1.position - t2.position;

        // Midpoint of the distance
        Vector3 midpoint = t1.position - distanceVector * 0.5f;

        // Center the camera on the midpoint and zoom to the startOffset distance
        Vector3 cameraDestination = midpoint - cam.transform.forward * _startOffset;

        // Start zooming the camera if distance is bigger than X
        if(distanceVector.magnitude > _zoomBounds.x)
        {
            _zoomDistance = Mathf.Clamp((distanceVector.magnitude - _zoomBounds.x), 0.0f, _zoomBounds.y);
            cameraDestination -= cam.transform.forward * _zoomDistance * _zoomFactor;
        }
        // Stop updating camera position if zoomed to the max
        if (_zoomDistance < _zoomBounds.y) _newCamPos = cameraDestination;

        // Update the position of the camera
        cam.transform.position = Vector3.Slerp(cam.transform.position, _newCamPos, _followTimeDelta * Time.deltaTime);

        // Snap when close enough to prevent annoying slerp behavior
        if ((_newCamPos - cam.transform.position).sqrMagnitude <= float.Epsilon)
            cam.transform.position = _newCamPos;
    }

    /// <summary>
    /// Constrains the object to the bounds of the camera's view
    /// </summary>
    public void ConstrainToView(Transform gameObject)
    {
        // Gets the position of the object in the viewport and clamps the position
        Vector3 pos = _cam.WorldToViewportPoint(gameObject.position);
        pos.x = Mathf.Clamp(pos.x, 0.0f, 1.0f);
        pos.y = Mathf.Clamp(pos.y, 0.0f, 1.0f);

        // Set the position to the clamped value
        gameObject.position = _cam.ViewportToWorldPoint(pos);
    }

    public bool Constrain
    {
        get { return _constrain; }
        set { _constrain = value; }
    }
}
