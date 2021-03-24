using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {

    private Rigidbody rb;

    [Tooltip("Set the speed of the player, public mainly for testing")]
    public float Speed, MaxVelocity;

    private float moveOnXAxis;
    private float moveOnZAxis;
    private float rotateOnXAxis;
    private float rotateOnZAxis;

    private Quaternion targetRotation;
    
    // Jumping
    private float distanceFromGround;
    private Vector3 downVector;
    private bool grounded;
    private CapsuleCollider objectCC;

    private int playerNumber;

    // Aim assist
    private AimAssistCollider aimAssist;
    private float rotateSpeed;

    //TEMPORARY
    private float moveOnXAxisKeyboard;
    private float moveOnZAxisKeyboard;

    [SerializeField] private LayerMask layer;

    private void Start ()
    {
        aimAssist = GetComponentInChildren<AimAssistCollider>();
        rb = GetComponent<Rigidbody>();
       // Speed = Speed * rb.mass;
        playerNumber = GetComponent<CharacterStats>().PlayerNumber;
        objectCC = GetComponent<CapsuleCollider>();
    }
	
	private void FixedUpdate ()
    {
        MoveWithKeyboard();
        MoveWithController();
    }

    private void Update()
    {
        moveOnXAxis = Input.GetAxis("HorizontalJ" + playerNumber) * Speed;
        moveOnZAxis = Input.GetAxis("VerticalJ" + playerNumber) * Speed;

        rotateOnXAxis = Input.GetAxis("HorizontalCameraJ" + playerNumber) * Speed;
        rotateOnZAxis = Input.GetAxis("VerticalCameraJ" + playerNumber) * Speed;

        RotateCharacter();
        Jump();
    }

    private void MoveWithKeyboard()
    {
        moveOnXAxisKeyboard = Input.GetAxis("HorizontalP" + playerNumber) * Speed;
        moveOnZAxisKeyboard = Input.GetAxis("VerticalP" + playerNumber) * Speed;

        //moveOnXAxis *= Time.deltaTime;
        //moveOnZAxis *= Time.deltaTime;
        Vector3 movement = new Vector3(-moveOnXAxisKeyboard * 0.9f, 0.0f, moveOnZAxisKeyboard * 0.9f); //its a - cuz SOMEONE fucked up the scene

        if (rb.velocity.magnitude < MaxVelocity) //rb.AddForce(movement, ForceMode.Force);
        {
            rb.velocity = new Vector3(movement.x * 20, rb.velocity.y, movement.z * 20);
        }
    }  

    private void MoveWithController()
    {
        Vector3 movement = new Vector3(-moveOnXAxis * 0.7f * Time.deltaTime * Speed,   //its a - cuz SOMEONE fucked up the scene
            0.0f, moveOnZAxis * 0.7f * Time.deltaTime * Speed);

        if (rb.velocity.magnitude < MaxVelocity) //rb.AddForce(movement, ForceMode.Force);
        {
            rb.velocity = new Vector3(movement.x * 20, rb.velocity.y, movement.z * 20);
        }
    }

    private void RotateCharacter()
    {
        if (aimAssist.Slow) rotateSpeed = 6;
        else rotateSpeed = 12;

        Vector3 direction = new Vector3(-rotateOnXAxis, 0.0f, rotateOnZAxis);
        if (direction != Vector3.zero) targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (direction != Vector3.zero) rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if(objectCC != null) distanceFromGround = objectCC.bounds.size.y / 2 + 0.25f;
        downVector = Vector3.down;

        Debug.DrawRay(transform.position, downVector * distanceFromGround, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, downVector,out hit, distanceFromGround, layer)) grounded = true;
        else grounded = false;

        //print(playerNumber + " " + distanceFromGround);
        if (Input.GetButtonDown("AJ" + playerNumber) && grounded) rb.AddForce(Vector3.up * Speed * rb.mass * 2, ForceMode.Impulse);
        
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (2 - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (1.5f - 1) * Time.deltaTime;
        }
    }

    public float GetXMovement
    {
        get { return moveOnXAxis / Speed; }
    }

    public float GetZMovement
    {
        get { return moveOnZAxis / Speed;  }
    }
}
