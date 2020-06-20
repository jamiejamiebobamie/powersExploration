using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE:
// make sure to set Layer for all scenery to be "Ignore Raycast"
public class Scenery : MonoBehaviour, ICopyable, IThrowable
{
    [SerializeField] GameObject fire;

    private bool isBurning, isProjectile, isOrbiting, isThrowable;
    public Stimulus stimulus;

    public bool IsOrbiting
    {
        get { return isOrbiting; }
        set {
            isOrbiting = value;
            if (isOrbiting)
            {
                //foreach (Collider playerCollide in telekinesisPlayerColliders)
                //{
                //    Physics.IgnoreCollision(collide, playerCollide, true);
                //}
                rb.isKinematic = true;
            }
            else
            {
                //foreach (Collider playerCollide in telekinesisPlayerColliders)
                //{
                //    Physics.IgnoreCollision(collide, playerCollide, false);
                //}
                rb.isKinematic = false;
            }
        }
    }

    // a reference to the target that the item is orbiting around
    private GameObject orbitTarget;

    //private TelekinesisPower[] playersWithTelekinesis;
    //private List<Collider> telekinesisPlayerColliders = new List<Collider>();

    // orbitRotationSpeed = 2to4 good range with orbitTranslationSpeed 10to25
    private float orbitRotationSpeed, orbitTranslationSpeed, orbitHeight,
        baseOrbitHeight, baseTranslationOrbitSpeed, baseRotationOrbitSpeed;

    private Collider collide;
    private Rigidbody rb;

    System.Random random = new System.Random();

    void Start()
    {
        stimulus = GetComponent<Stimulus>();
        isProjectile = false;
        isOrbiting = false;
        isThrowable = true;

        //playersWithTelekinesis = FindObjectsOfType<TelekinesisPower>();

        //foreach (TelekinesisPower power in playersWithTelekinesis)
        //{
        //    Collider test_collider = power.gameObject.GetComponent<Collider>();
        //    if (test_collider != null)
        //    {
        //        telekinesisPlayerColliders.Add(test_collider);
        //    }
        //}

        orbitRotationSpeed = random.Next(2, 5);
        baseRotationOrbitSpeed = orbitRotationSpeed;

        orbitTranslationSpeed = orbitRotationSpeed * 5;
        baseTranslationOrbitSpeed = orbitTranslationSpeed;

        orbitHeight = orbitRotationSpeed + random.Next(-2, 2); //+ 2f;
        baseOrbitHeight = orbitHeight;

        collide = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    // ICopyable ---
    public Mesh GetMesh()
    { return gameObject.GetComponent<MeshFilter>().mesh; }
    public Vector3 GetPosition() { return transform.position; }
    public Stimulus.origin GetOriginOfStimulus()
        { return stimulus.GetCurrentOrigin(); }
    // ---

    // IBurnable ---
    public void Burns()
    {
        GameObject fireInstance = Instantiate(fire,
            transform.position, Quaternion.identity);
        isBurning = true;
        Destroy(fireInstance, 3f);
    }
    public bool GetIsBurning()
    {
        return isBurning;
    }
    // ---

    // IThrowable ---
    public void BecomeProjectile(Vector3 destination)
    {
        Vector3 directionOfForce = Vector3.Normalize(destination
            - gameObject.transform.position);

        isProjectile = true;
        StopCoroutine("Orbit");
        IsOrbiting = false;         // controls the collider:

        rb.AddForce(directionOfForce * 3000f);
        StartCoroutine("SetIsProjectileToFalse");
    }

    public IEnumerator SetIsProjectileToFalse()
    {
        yield return new WaitForSeconds(2f);
        isProjectile = false;
    }

    public IEnumerator Orbit()
    {
        if (!isProjectile)
        {
            for (;;)
            {
                Vector3 relativePos = (orbitTarget.transform.position
                + new Vector3(0, orbitHeight, 0)) - transform.position;

                Quaternion rotation = Quaternion.LookRotation(relativePos);

                Quaternion current = transform.localRotation;

                transform.localRotation = Quaternion.Slerp(current, rotation,
                    Time.deltaTime * orbitRotationSpeed);

                transform.Translate(0, 0,
                    1 * Time.deltaTime * orbitTranslationSpeed);

                yield return new WaitForSeconds(.03f);

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

    public void setIsThrowable(bool boolean)
    {
        isThrowable = boolean;
    }

    public bool GetIsProjectile()
    {
        return isProjectile;
    }

    public void SetObjectToOrbit(GameObject objectToOrbit)
    {
        orbitTarget = objectToOrbit;
        IsOrbiting = true;
        StartCoroutine("Orbit");
    }
    // ---

    private void OnCollisionEnter(Collision collision)
    {
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
        if (hittable != null && isProjectile)
        {
            float hitForce = rb.velocity.magnitude;
            Vector3 hitDirection = Vector3.Normalize(rb.velocity);

            hittable.ApplyHitForce(hitDirection, hitForce, orbitTarget.name);
        }
    }
}
