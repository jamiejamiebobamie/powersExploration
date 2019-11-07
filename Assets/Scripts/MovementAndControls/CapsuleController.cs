using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleController : MonoBehaviour
{
    private float speed = 0.3f;
    private float rotSpeed = 3;
    private float jumpHeight = 400;
    private bool jumpPressed = false;
    private bool grounded;

    [SerializeField]
    private Rigidbody rb;
    private float hAxis;
    private float vAxis;
    private bool Jump;
    private bool movingObject;

    private Aspect.aspect aspectNameStored;
    public Aspect aspectRef;

    private IPowerable power;

    // Start is called before the first frame update
    void Start()
    {
        power = GetComponent<IPowerable>();
        rb = GetComponent<Rigidbody>();
        power = GetComponent<IPowerable>();
        jumpPressed = false;
        grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        vAxis = Input.GetAxis("Vertical") * speed;
        hAxis = Input.GetAxis("Horizontal") * rotSpeed;
        jumpPressed = Input.GetKeyDown(KeyCode.Space);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            power.ActivatePower();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (aspectRef.aspectName != Aspect.aspect.Sneaking)
            {
                aspectNameStored = aspectRef.aspectName;
                aspectRef.aspectName = Aspect.aspect.Sneaking;
            }
            else
            {
                aspectRef.aspectName = aspectNameStored;
            }
        }

        Debug.Log(aspectRef.aspectName);

    }

    private void FixedUpdate()
    {
        transform.Translate(0, 0, vAxis);
        transform.Rotate(0, hAxis, 0);

        if (jumpPressed && grounded)
        {
            rb.AddForce(0, jumpHeight, 0);
        }
        if (rb.velocity.magnitude > .00001 && aspectRef.aspectName == Aspect.aspect.Object)
            {
                aspectRef.aspectName = Aspect.aspect.Sneaking;
                movingObject = true; // storing the Object aspect;
            }
        else if (rb.velocity.magnitude < .000005 && movingObject)
            {
                aspectRef.aspectName = Aspect.aspect.Object;
                movingObject = false;
            }
    }
}
