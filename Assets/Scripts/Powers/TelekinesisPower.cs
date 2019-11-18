using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisPower : MonoBehaviour, IPowerable
{

    Queue<IThrowable> throwables = new Queue<IThrowable>();
    Scenery[] allScenery;
    GameObject playerRef;
    float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        allScenery = FindObjectsOfType<Scenery>();
        elapsedTime = 0.0f;
    }



    public void ActivatePower()
    {
        Throw();
    }

    void Throw()
    {
        Ray ray = new Ray();
        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("hit");
            IThrowable throwThisOne = throwables.Dequeue();
            throwThisOne.BecomeProjectile(hit.point);
        }

    }

    void UpdateQueue()
    {
        foreach (Scenery scenery in allScenery)
        {
            float distanceFromPlayer = (transform.position - scenery.transform.position).magnitude;
            if (!throwables.Contains(scenery)&& distanceFromPlayer < 5f)
            {
                throwables.Enqueue(scenery);
                scenery.Orbiting = true;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 4.0f)
            UpdateQueue();

    }
}
