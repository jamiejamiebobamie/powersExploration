using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : Sense
{
    public int SoundDistance = 50;
    private Transform playerTrans;
    private Vector3 rayDirection;

    protected override void Initialize()
    {
        //Find player position
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;
        // Detect perspective sense if within the detection rate
        if (elapsedTime >= detectionRate) DetectAspect();
    }

    //Detect perspective field of view for the AI Character
    void DetectAspect()
    {
        RaycastHit hit;

        rayDirection = playerTrans.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection,
                out hit, SoundDistance))
        {
            Aspect aspect = hit.collider.GetComponent<Aspect>();
            if (aspect != null)
            {
                Aspect.aspect currentAspect = aspect.GetCurrentAspect();
                //Check the aspect
                if (currentAspect == aspectName)
                {
                    print("Player heard.");
                }
            }
        }
    }
}
