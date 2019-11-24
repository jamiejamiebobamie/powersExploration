using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE:
// make sure to set Layer for all scenery to be "Ignore Raycast"
public class Scenery : MonoBehaviour, ICopyable, IBurnable, IThrowable
{
    [SerializeField] GameObject fireStandIn;
    private bool isBurning;
    public bool isProjectile;
    public bool Orbiting
    {
        get { return orbiting; }

        set {
            orbiting = value;
            if (orbiting)
            {
                foreach (Collider playerCollide in telekinesisPlayerColliders)
                {
                    Physics.IgnoreCollision(collide, playerCollide, true);
                }
                rb.isKinematic = true;
            }
            else
            {
                foreach (Collider playerCollide in telekinesisPlayerColliders)
                {
                    Physics.IgnoreCollision(collide, playerCollide, false);
                }

                rb.isKinematic = false;
            }
        }
    }
    private bool orbiting;
    public GameObject orbitPlayer; // a reference to the player
                                    // that the item is orbiting around

    private TelekinesisPower[] playersWithTelekinesis;
    private List<Collider> telekinesisPlayerColliders = new List<Collider>();

    // orbitRotationSpeed = 2 to 4 good range with orbitTranslationSpeed 20
    // orbitTranslationSpeed = 10 to 25 good range with orbitRotationSpeed 2 to 4
    // need to make getters and setters
    public float orbitRotationSpeed, orbitTranslationSpeed;

    private float orbitHeight, baseOrbitHeight,
        baseTranslationOrbitSpeed, baseRotationOrbitSpeed;

    Collider collide;
    Rigidbody rb;

    System.Random random = new System.Random();

    float elapsedTime;

    void Start()
    {
        isProjectile = false;
        orbiting = false;

        playersWithTelekinesis = FindObjectsOfType<TelekinesisPower>();

        foreach (TelekinesisPower power in playersWithTelekinesis)
        {
            Collider test_collider = power.gameObject.GetComponent<Collider>();
            if (test_collider != null)
            {
                telekinesisPlayerColliders.Add(test_collider);
            }
        }

        orbitRotationSpeed = random.Next(2,5);
        baseRotationOrbitSpeed = orbitRotationSpeed;

        orbitTranslationSpeed = orbitRotationSpeed * 5;
        baseTranslationOrbitSpeed = orbitTranslationSpeed;
        orbitHeight = orbitRotationSpeed + 2f;
        baseOrbitHeight = orbitHeight;

        collide = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        elapsedTime = 0.0f;
    }

    public Mesh Copy()
    {
        Mesh gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;
        return gameObjectMesh;
    }

    public void Burns()
    {
        Instantiate(fireStandIn, transform.position, Quaternion.identity);
        isBurning = true;
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

        Vector3 relativePos = (orbitPlayer.transform.position
            + new Vector3(0, orbitHeight, 0))- transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation,
            Time.deltaTime * orbitRotationSpeed);

        transform.Translate(0, 0,
            1 * Time.deltaTime * orbitTranslationSpeed);
    }

    public void SetOrbitHeight(float desiredHeight)
    {
        orbitHeight = desiredHeight;
    }

    public float GetOrbitHeight()
    {
        return baseOrbitHeight;
    }

    public void SetOrbitTranslationSpeed(float desiredSpeed)
    {
        orbitTranslationSpeed = desiredSpeed;
    }

    public float GetOrbitTranslationSpeed()
    {
        return baseTranslationOrbitSpeed;
    }

    public void SetOrbitRotationSpeed(float desiredSpeed)
    {
        orbitRotationSpeed = desiredSpeed;
    }

    public float GetOrbitRotationSpeed()
    {
        return baseRotationOrbitSpeed;
    }

    // how to only allow, scenery isProjectile?
    private void OnCollisionEnter(Collision collision)
    {
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
        if (hittable != null)
        {
            float hitForce = rb.velocity.magnitude;
            Vector3 hitDirection = Vector3.Normalize(rb.velocity);

            hittable.ApplyHitForce(hitDirection, hitForce);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

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

