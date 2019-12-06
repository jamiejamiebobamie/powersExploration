using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE:
// make sure to set Layer for all scenery to be "Ignore Raycast"
public class Scenery : MonoBehaviour, ICopyable, IThrowable
{
    [SerializeField] GameObject fire;
    private bool burning, isProjectile, orbiting;
    private Stimulus stimulus;

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

    // a reference to the player that the item is orbiting around
    private GameObject orbitPlayer;


    private TelekinesisPower[] playersWithTelekinesis;
    private List<Collider> telekinesisPlayerColliders = new List<Collider>();

    // orbitRotationSpeed = 2to4 good range with orbitTranslationSpeed 10to25
    private float orbitRotationSpeed, orbitTranslationSpeed, orbitHeight,
        baseOrbitHeight, baseTranslationOrbitSpeed, baseRotationOrbitSpeed;

    Collider collide;
    Rigidbody rb;

    System.Random random = new System.Random();

    float elapsedTime;

    void Start()
    {
        stimulus = GetComponent<Stimulus>();
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

        orbitRotationSpeed = random.Next(2, 5);
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

    public Mesh GetMesh()
    {
        Mesh gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;
        return gameObjectMesh;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Stimulus.origin GetOriginOfStimulus()
    {
        return stimulus.GetCurrentOrigin();
    }

    public void Burns()
    {
        GameObject fireInstance = Instantiate(fire,
            transform.position, Quaternion.identity);
        burning = true;
        Destroy(fireInstance, 3f);
    }

    public void BecomeProjectile(Vector3 destination)
    {

        Vector3 directionOfForce = Vector3.Normalize(destination
            - gameObject.transform.position);

        isProjectile = true;
        StopCoroutine("Orbit");
        // this controls the collider:
        Orbiting = false;
        // turning it to false allows the collider to interact
        // with the environment causing the projectile to hit other orbiting
        // throwables, causing the projectile to not hit the target...

        rb.AddForce(directionOfForce * 3000f);





    }

    private IEnumerator TurnOffOrbitingWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    // Need to make this a coroutine.
    public IEnumerator Orbit()
    {
        if (!isProjectile)
        {
            for (; ; )
            {

                Vector3 relativePos = (orbitPlayer.transform.position
                + new Vector3(0, orbitHeight, 0)) - transform.position;

                Quaternion rotation = Quaternion.LookRotation(relativePos);

                Quaternion current = transform.localRotation;

                transform.localRotation = Quaternion.Slerp(current, rotation,
                    Time.deltaTime * orbitRotationSpeed);

                transform.Translate(0, 0,
                    1 * Time.deltaTime * orbitTranslationSpeed);

                yield return null;// new WaitForSeconds(.05f);

            }
        }
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

    public bool GetIsProjectile()
    {
        return isProjectile;
    }

    public bool GetIsBurning()
    {
        return burning;
    }

    // this method also sets Orbiting boolean to true.
    public void SetObjectToOrbit(GameObject objectToOrbit)
    {
        orbitPlayer = objectToOrbit;
        Orbiting = true;
        StartCoroutine("Orbit");
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

        //if (orbiting)
        //    //Orbit();
        //else
            if (isProjectile && elapsedTime >= 4.0f)
                isProjectile = false;
    }
}
