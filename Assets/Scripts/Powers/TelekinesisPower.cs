using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisPower : MonoBehaviour, IPowerable
{
    List<IThrowable> possibleThrowables = new List<IThrowable>();
    List<Humanoid> humanoids = new List<Humanoid>();

    Queue<IThrowable> throwables = new Queue<IThrowable>();
    GameObject[] allSceneObjects;
    private float elapsedTime;
    private bool isBlocking;

    void Start()
    {
        Humanoid selfHumanoidScript = GetComponent<Humanoid>();

        allSceneObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject o in allSceneObjects)
        {
            IThrowable testThrowable = o.GetComponent<IThrowable>();
            if(testThrowable != null)
            {
                possibleThrowables.Add(testThrowable);
            }

            Humanoid testHumanoid = o.GetComponent<Humanoid>();
            if (testHumanoid != null && testHumanoid != selfHumanoidScript)
            {
                humanoids.Add(testHumanoid);
            }
        }

        elapsedTime = 0.0f;
    }

    public void ActivatePower1()
    {
        //if (!isBlocking)
            Throw();
    }

    public void ActivatePower2()
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

    void DefenseMode()
    {
        SetSpeedAndHeight(heightModifier: 4f,
            transSpeedModifier: 1.5f, rotSpeedModifier: 2f);
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
            IThrowable throwThisOne = throwables.Dequeue();

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

            throwThisOne.BecomeProjectile(shootHere);

        }

        //if (throwables.Count > 0)
        //{
        //    IThrowable loadNextOne = throwables.Peek();
        //    loadNextOne.SetOrbitHeight(4f);
        //    loadNextOne.SetOrbitTranslationSpeed(5f);
        //    loadNextOne.SetOrbitRotationSpeed(10f);
        //}
    }

    void UpdateQueue()
    {
        foreach (IThrowable t in possibleThrowables)
        {
            float distanceFromPlayer = (transform.position
                - t.GetPosition()).magnitude;

            if (!throwables.Contains(t)
                && distanceFromPlayer < 2f && !t.GetIsProjectile())
            {
                throwables.Enqueue(t);
                t.SetObjectToOrbit(gameObject);
            }
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime % 2.0f < 1)
            UpdateQueue();
    }
}
