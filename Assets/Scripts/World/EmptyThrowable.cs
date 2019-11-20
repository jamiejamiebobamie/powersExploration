using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyThrowable : MonoBehaviour, IThrowable
{

    public bool isProjectile;
    private bool orbiting;

    public bool Orbiting
    {
        get { return orbiting; }

        set {
            orbiting = value;
            if (orbiting)
            {
                Physics.IgnoreCollision(collide, playerCollide, true);
                rb.isKinematic = true;
            }
            else
            {
                Physics.IgnoreCollision(collide, playerCollide, false);
                rb.isKinematic = false;
                foreach (Transform children in transform)
                {
                    Rigidbody childRb = children.GetComponent<Rigidbody>();
                    if (childRb != null)
                    {
                        childRb.isKinematic = false;
                        childRb.useGravity = false;
                        //Physics.IgnoreCollision(collide, playerCollide, false);
                    }
                }
            }
        }
    }


    private GameObject player;

    //[Range(-100, 100)] // 2 to 4 good range with transSpeed 20
    public float rotSpeed; // need to make getters and setters

    //[Range(1, 50)] // 10 to 25 good range with rotSpeed 2 to 4
    public float transSpeed; // need to make getters and setters

    Collider collide;
    Rigidbody rb;
    Collider playerCollide;

    System.Random random = new System.Random();

    float elapsedTime;

    void Start()
    {
        isProjectile = false;
        orbiting = false;

        player = GameObject.FindGameObjectWithTag("Player");// playerRef;
        playerCollide = player.GetComponent<Collider>();

        rotSpeed = random.Next(2,5);
        transSpeed = rotSpeed * 5;

        collide = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        elapsedTime = 0.0f;
        // NOTE:
        // make sure to set Layer for all scenery to be "Ignore Raycast"
    }

    public void BecomeProjectile(Vector3 destination)
    {
        Orbiting = false;

        isProjectile = true;

        Vector3 directionOfForce = Vector3.Normalize(destination
            - gameObject.transform.position);
        rb.AddForce(directionOfForce * 3000f);
    }

    // coroutine?
    public void Orbit()
    {
        Vector3 relativePos = (player.transform.position
            + new Vector3(0, rotSpeed, 0))- transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation,
            Time.deltaTime * rotSpeed);

        transform.Translate(0, 0, 1 * Time.deltaTime * transSpeed);
    }

    // how to only allow, scenery isProjectile?
    private void OnCollisionEnter(Collision collision)
    {
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
        if (hittable != null)
        {
            float hitForce = rb.velocity.magnitude;
            Vector3 hitDirection = Vector3.Normalize(rb.velocity);

            Debug.Log(hittable);
            hittable.ApplyHitForce(hitDirection, hitForce);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; // I don't understand how this works.

        if (orbiting)
        {
            Orbit();
        } else
        {
            if (isProjectile && elapsedTime >= 4.0f)
            {
                isProjectile = false;
            }
        }

    }
}

