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

    [SerializeField] private Rigidbody rb;
    private float hAxis;
    private float vAxis;
    private bool Jump;
    private bool movingObject;

    public Stimulus.origin aspectNameStored;
    [SerializeField] private IPowerable[] powers;
    [SerializeField] private IPowerable power1,power2;

    // Start is called before the first frame update
    void Start()
    {
        powers = GetComponents<IPowerable>();

        power1 = powers[0];

        if (powers.Length > 1)
            power2 = powers[1];
        else
            power2 = powers[0];

        //rb = GetComponent<Rigidbody>();
        //power = GetComponent<IPowerable>();
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
            power1.ActivatePower1();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            power2.ActivatePower2();
        }

    }

    private void FixedUpdate()
    {
        transform.Translate(0, 0, vAxis);
        transform.Rotate(0, hAxis, 0);

        if (jumpPressed && grounded)
        {
            rb.AddForce(0, jumpHeight, 0);
        }

    }
}
