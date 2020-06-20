using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisPower : Power//, IPowerable
{
    List<IThrowable> possibleThrowables = new List<IThrowable>();
    List<Humanoid> humanoids = new List<Humanoid>();

    List<IThrowable> throwables = new List<IThrowable>();
    GameObject[] allSceneObjects;
    private bool isBlocking;

    [SerializeField] Humanoid self;// = GetComponent<Humanoid>();

    // use: OnCollisionEnter to set the IThrowable to orbit.
        // Remove UpdateQueue() method / implementation.

void Start()
    {
        allSceneObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject o in allSceneObjects)
        {
            IThrowable testThrowable = o.GetComponent<IThrowable>();
            if(testThrowable != null)
            {
                possibleThrowables.Add(testThrowable);
            }

            Humanoid testHumanoid = o.GetComponent<Humanoid>();
            if (testHumanoid != null && testHumanoid != self)
            {
                humanoids.Add(testHumanoid);
                // Debug.Log(testHumanoid);
            }
        }
        StartCoroutine("UpdateThrowables");
    }

    public override void ActivatePower1()
    {
        if (!isBlocking)
            Throw();
    }

    public override void ActivatePower2()
    {
        if (isBlocking)
        {
            isBlocking = false;
            AttackMode();

        } else
        {
            isBlocking = true;
            DefenseMode();
        }
    }

    public bool GetIsBlocking(){return isBlocking;}
    public bool GetHasThrowables(){return throwables.Count>0;}

    void DefenseMode()
    {
        SetSpeedAndHeight(heightModifier: 4f,
            transSpeedModifier: 2f, rotSpeedModifier: 4f);//4,1.5,2
    }

    void AttackMode()
    {
        SetSpeedAndHeight(heightModifier: 1f,
            transSpeedModifier: 1f, rotSpeedModifier: 1f);
    }

    void SetSpeedAndHeight(float heightModifier,
        float transSpeedModifier, float rotSpeedModifier)
    {
        foreach (IThrowable throwable in throwables)
        {
            float baseOrbitHeight = throwable.GetOrbitHeight();
            throwable.SetOrbitHeight(baseOrbitHeight / heightModifier);

            float baseOrbitTranslationSpeed =
                throwable.GetOrbitTranslationSpeed();
            throwable.SetOrbitTranslationSpeed(
                baseOrbitTranslationSpeed * transSpeedModifier);

            float baseOrbitRotationSpeed =
                throwable.GetOrbitRotationSpeed();
            throwable.SetOrbitRotationSpeed(
                baseOrbitRotationSpeed * rotSpeedModifier);
        }
    }

    // need to ensure projectile is clear
        // of player and player's orbiting objects.
        // might switch to non-physics-based implementations...
    void Throw()
    {
        if (throwables.Count > 0)
        {
            Humanoid possibleTarget = null;
            float minAngle= Mathf.Infinity;

            foreach (Humanoid h in humanoids)
            {
                if (!h.GetIsStaggered())
                {
                    Vector3 positionOfHumanoid = h.GetPosition();
                    Vector3 direction = positionOfHumanoid - transform.position;
                    float distance = direction.magnitude;

                    if (distance < 30f)
                    {
                        float angleToHumanoid
                            = Vector3.Angle(direction, transform.forward);

                        if (angleToHumanoid
                            < minAngle)
                        {
                            minAngle = angleToHumanoid;
                            possibleTarget = h;
                        }
                    }
                }
            }
            Vector3 shootHere = Vector3.zero;
            // if there is a target
            if (possibleTarget != null)
            {
                shootHere = possibleTarget.GetPosition();
            }
            else
            {
                Ray ray = new Ray();
                ray.origin = Camera.main.transform.position;
                ray.direction = Camera.main.transform.forward;

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                    shootHere = hit.point;
            }
            // find the IThrowable that is closest to the target.
            IThrowable throwThisOne = FindClosestThrowable(shootHere);
            throwThisOne.BecomeProjectile(shootHere);
        }
    }

    public void BlockedAttack(Vector3 directionOfAttack)
    {
        IThrowable throwableThatBlockedAttack = FindClosestThrowable(directionOfAttack);

        Vector3 upOrDownVec = Vector3.zero;
        bool ricochetLeft = Random.value >= 0.5;
        if(ricochetLeft)
        {
            upOrDownVec = Vector3.up;
        }
        else
        {
            upOrDownVec = Vector3.down;
        }
        Vector3 perpVec = Vector3.Cross(directionOfAttack, upOrDownVec).normalized;
        Vector3 shootHere = self.GetPosition() + perpVec * 5.0f;
        throwableThatBlockedAttack.BecomeProjectile(shootHere);
    }

    private IThrowable FindClosestThrowable(Vector3 shootHere)
    {
        IThrowable throwThisOne = null;

        float minDistanceToTarget = Mathf.Infinity;
        foreach (IThrowable t in throwables)
        {
            Vector3 positionOfThrowable = t.GetPosition();
            Vector3 vectorToThrow = shootHere - positionOfThrowable;
            float distanceToThrow = vectorToThrow.magnitude;
            if (minDistanceToTarget > distanceToThrow)
            {
                minDistanceToTarget = distanceToThrow;
                throwThisOne = t;
            }
        }
        throwables.Remove(throwThisOne);
        return throwThisOne;
    }

    private IEnumerator UpdateThrowables()
    {
        while (true)
        {
            foreach (IThrowable t in possibleThrowables)
            {
                float distanceFromPowerOwner = (transform.position
                    - t.GetPosition()).magnitude;

                if (!throwables.Contains(t)
                    && distanceFromPowerOwner < 2f && !t.GetIsProjectile())
                {
                    throwables.Add(t);
                    t.SetObjectToOrbit(gameObject);
                    // add new IThrowable to throwables and call the correct method
                        // to set the throwable's height, rotation speed, translation, speed of orbit.
                    if (isBlocking)
                    {
                        DefenseMode();
                    }
                    else
                    {
                        AttackMode();
                    }
                }
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}
