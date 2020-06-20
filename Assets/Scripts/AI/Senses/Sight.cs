using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Sense
{
    GameObject[] allSceneObjects;
    List<Humanoid> humanoids = new List<Humanoid>();
    Humanoid self;
    private bool targetSeen;
    private Humanoid seenTarget;
    [SerializeField] Stimulus.origin targetOriginType;

    protected override void Initialize()
    {
        self = GetComponent<Humanoid>();
        allSceneObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject o in allSceneObjects)
        {
            Humanoid testHumanoid = o.GetComponent<Humanoid>();
            if (testHumanoid != null && testHumanoid != self)
            {
                Stimulus test_S = o.GetComponent<Stimulus>();
                if (test_S.GetCurrentOrigin() == targetOriginType)
                {
                    humanoids.Add(testHumanoid);
                }
            }
        }
        targetSeen = false;
    }
    // Update is called once per frame
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;
        targetSeen = false;
        // Detect perspective sense if within the detection rate
        if (elapsedTime >= detectionRate)
        {
            resetSeen();
            DetectAspect();
        };
    }
    public bool getTargetSeen(){
        return targetSeen;
    }
    public Vector3 getSeenTargetLocation(){
        return seenTarget.GetPosition();
    }
    public Stimulus.origin getDesiredTarget(){
        return targetOriginType;
    }

    public void resetSeen(){
        targetSeen = false;
        seenTarget = null;
    }
    //Detect perspective field of view for the AI Character
    void DetectAspect()
    {
        foreach (Humanoid h in humanoids)
        {
            RaycastHit hit;
            Vector3 rayDirection;
            //Direction from current position to player position
            rayDirection = h.GetPosition() - transform.position;
            int FieldOfView = 60;
            int ViewDistance = 100;
            //Check the angle between the AI character's forward
            //vector and the direction vector between player and AI
            if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
            {
                // Detect if player is within the field of view
                if (Physics.Raycast(transform.position, rayDirection,
                    out hit, ViewDistance))
                {
                    // testing Stimulus even after initialization ensures the
                        // correct types of humanoids are added to the the
                        // 'humanoid' list, because copycat can change his
                        // stimulus type and all characters can crouch.
                    Stimulus stimulus = hit.collider.GetComponent<Stimulus>();
                    if (stimulus != null)
                    {
                        Stimulus.origin currentOrigin = stimulus.GetCurrentOrigin();
                        //Check the origin of the stimulus.
                        // Ignore. “Sneaking” players CAN be seen.
                        if (currentOrigin == desiredStimulusOrigin
                            || currentOrigin == Stimulus.origin.Sneaking)
                        {
                              print(desiredStimulusOrigin + " seen!");
                              // print(playerTrans.transform.gameObject.name);
                              seenTarget = h;
                              targetSeen = true;
                        }
                    }
                }
            }
        }
    }
}
