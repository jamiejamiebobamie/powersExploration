using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : Sense
{
    public int SoundDistance = 50;
    private Transform playerTrans;
    private Vector3 rayDirection;
    private bool patientHeard;

    protected override void Initialize()
    {
        //Find player position
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        patientHeard = false;
    }

    // Update is called once per frame
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;
        // Detect perspective sense if within the detection rate
        if (elapsedTime >= detectionRate) DetectAspect();
    }
    public bool getPatientHeard(){
        return patientHeard;
    }
    public Vector3 getSeenPatientLocation(){
        return playerTrans.position;
    }
    //Detect perspective field of view for the AI Character
    void DetectAspect()
    {
        RaycastHit hit;

        rayDirection = playerTrans.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection,
                out hit, SoundDistance))
        {
            Stimulus stimulus = hit.collider.GetComponent<Stimulus>();
            if (stimulus != null)
            {
                Stimulus.origin currentOrigin = stimulus.GetCurrentOrigin();
                //Check the aspect
                if (currentOrigin == desiredStimulusOrigin)
                {
                    print("Heard target.");
                    patientHeard = true;
                }
            }
        }
    }
}
