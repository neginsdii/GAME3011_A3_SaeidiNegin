using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float WalkSpeed = 5;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private float JumpForce = 5;

    //Components

    private Rigidbody rigidbody;
    public GameObject followTarget;

    Vector2 inputVector = Vector2.zero;
    Vector3 MoveDirection = Vector3.zero;
    public Vector2 lookInput = Vector2.zero;
    public float AimSensetivity = 1;


    private void Awake()
    {


        rigidbody = GetComponent<Rigidbody>();

    }
    void Start()
    {
    }

    void Update()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Movement

        if (!(inputVector.magnitude > 0)) MoveDirection = Vector3.zero;

        MoveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;

        Vector3 MovementDirection = MoveDirection * WalkSpeed * Time.deltaTime;
        transform.position += MovementDirection;

        //Aiming/Looking
        followTarget.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * AimSensetivity, Vector3.up);
        //     followTarget.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * AimSensetivity, Vector3.left);
        var angles = followTarget.transform.localEulerAngles;
        angles.z = 0;
        var angle = followTarget.transform.localEulerAngles.x;
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 70)
        {
            angles.x = 70;
        }
        followTarget.transform.localEulerAngles = angles;

        //player rotation
        transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
        followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        if (Input.GetButtonDown("Jump"))
        {


            rigidbody.AddForce((transform.up + MoveDirection) * JumpForce, ForceMode.Impulse);

        }
    }




}
