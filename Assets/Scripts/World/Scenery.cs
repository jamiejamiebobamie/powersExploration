using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenery : MonoBehaviour, ICopyable, IBurnable, IThrowable
{
    private bool burning;
    public bool isProjectile;
    public bool Orbiting
    {
        get { return orbiting; }

        set {
            orbiting = value;
            if (orbiting)
            {
                Physics.IgnoreCollision(collide, playerCollide, true);
                //rb.useGravity = true;
                rb.isKinematic = true;
            }
            else
            {
                Physics.IgnoreCollision(collide, playerCollide, false);
                //rb.useGravity = true;
                rb.isKinematic = false;
            }
        }
    }

    private bool orbiting;

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

        rotSpeed = random.Next(1,6);
        transSpeed = rotSpeed * 5;// 30;// rotSpeed * 5;
        //transSpeed = random.Next(10, 25);

        collide = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        elapsedTime = 0.0f;
        // set Layer for all scenery to be Ignore Raycast
    }

    [SerializeField] GameObject fireStandIn;
    public Mesh Copy()
    {
        Mesh gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;
        return gameObjectMesh;
    }

    public void Burns()
    {
        Instantiate(fireStandIn, transform.position, Quaternion.identity);
        burning = true;
    }

    public void BecomeProjectile(Vector3 destination)
    {
        Orbiting = false; // need to stagger this so that the projectile can
        //get safely away from the player before setting to false

        isProjectile = true;

        Vector3 directionOfForce = Vector3.Normalize(destination
            - gameObject.transform.position);
        rb.AddForce(directionOfForce * 3000f);
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
        if (hittable != null)
        {
            float hitForce = rb.velocity.magnitude;
            //Debug.Log(hitForce);
            Vector3 hitDirection = Vector3.Normalize(rb.velocity);
            hittable.ApplyHitForce(hitDirection, hitForce);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; // I don't understand how this works.

        //if (elapsedTime >= 4.0f)
        //{
        //    Debug.Log(elapsedTime);
        //}

        if (orbiting)
        {
            Orbit();
        } else
        {
            if (isProjectile && elapsedTime >= 4.0f)
            {
                isProjectile = false;
                //Debug.Log(isProjectile);
            }
        }

    }
}

