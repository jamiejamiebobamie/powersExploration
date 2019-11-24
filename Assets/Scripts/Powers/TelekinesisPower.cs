using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisPower : MonoBehaviour, IPowerable
{
    List<IThrowable> possibleThrowables = new List<IThrowable>();
    Queue<IThrowable> throwables = new Queue<IThrowable>();
    GameObject[] allSceneObjects;
    private float elapsedTime;
    private bool isBlocking;

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
        }
        elapsedTime = 0.0f;
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
            AttackMode();

        } else
        {
            isBlocking = true;
            DefenseMode();
        }
    }

    void DefenseMode()
    {
        foreach (IThrowable throwable in throwables)
        {
            float baseOrbitHeight = throwable.GetOrbitHeight();
            throwable.SetOrbitHeight(baseOrbitHeight / 4f);

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

    void AttackMode()
    {
        foreach (IThrowable throwable in throwables)
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
    }

    // need to ensure projectile is clear
    // of player and player's orbiting objects.
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

    void UpdateQueue()
    {
        foreach (IThrowable t in possibleThrowables)
        {
            float distanceFromPlayer = (transform.position
                - t.GetPosition()).magnitude;

            if (!throwables.Contains(t)
                && distanceFromPlayer < 4f && !t.GetIsProjectile())
            {
                throwables.Enqueue(t);
                t.SetOrbitPlayer(gameObject);
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
