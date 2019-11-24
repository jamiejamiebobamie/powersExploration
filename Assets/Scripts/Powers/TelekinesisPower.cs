using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisPower : MonoBehaviour, IPowerable
{
    // issues: throwable objects are being dequeue
    // and then immediately enqueued if within range of player (4f)
    // need to put UpdateQueue() on a timer (coRoutine?)

    Queue<IThrowable> throwables = new Queue<IThrowable>();
    Scenery[] allScenery;
    GameObject playerRef;
    float elapsedTime, compareTime;
    int count;

    bool isBlocking;

    void Start()
    {
        allScenery = FindObjectsOfType<Scenery>();
        elapsedTime = 0.0f;
        compareTime = 0.0f;
        count = 0;
    }

    public void ActivatePower1()
    {
        if (!isBlocking)
            Throw();
    }

    public void ActivatePower2()
    {
        if (isBlocking)
        {
            isBlocking = false;
            foreach(IThrowable throwable in throwables)
            {
                float baseOrbitHeight = throwable.GetOrbitHeight();
                throwable.SetOrbitHeight(baseOrbitHeight);

                float baseOrbitTranslationSpeed =
                    throwable.GetOrbitTranslationSpeed();
                throwable.SetOrbitTranslationSpeed(
                    baseOrbitTranslationSpeed);

                float baseOrbitRotationSpeed =
                    throwable.GetOrbitRotationSpeed();
                throwable.SetOrbitRotationSpeed(
                    baseOrbitRotationSpeed);
            }
        } else
        {
            isBlocking = true;
            foreach (IThrowable throwable in throwables)
            {
                float baseOrbitHeight = throwable.GetOrbitHeight();
                throwable.SetOrbitHeight(baseOrbitHeight/4f);

                float baseOrbitTranslationSpeed =
                    throwable.GetOrbitTranslationSpeed();
                throwable.SetOrbitTranslationSpeed(
                    baseOrbitTranslationSpeed * 1.5f);

                float baseOrbitRotationSpeed =
                    throwable.GetOrbitRotationSpeed();
                throwable.SetOrbitRotationSpeed(
                    baseOrbitRotationSpeed * 2f);
            }
        }
    }

    void Throw()
    {
        Ray ray = new Ray();
        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            IThrowable throwThisOne = throwables.Dequeue();
            throwThisOne.BecomeProjectile(hit.point);
        }

    }

    // this should be iterating over an IThrowable and not Scenery!!!
    void UpdateQueue()
    {
        foreach (Scenery scenery in allScenery)
        {
            float distanceFromPlayer = (transform.position
                - scenery.transform.position).magnitude;

            if (!throwables.Contains(scenery)
                && distanceFromPlayer < 4f && !scenery.isProjectile)
            {
                throwables.Enqueue(scenery);
                scenery.orbitPlayer = gameObject;
                scenery.Orbiting = true;
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
