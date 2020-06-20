using UnityEngine;
public class Sight : Sense
{

    public int FieldOfView = 60;
    public int ViewDistance = 100;
    private Transform playerTrans;
    private Vector3 rayDirection;
    private bool patientSeen;

    protected override void Initialize()
    {
        //Find player position
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        patientSeen = false;
    }
    // Update is called once per frame
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;
        // Detect perspective sense if within the detection rate
        if (elapsedTime >= detectionRate) DetectAspect();
    }
    public bool getPatientSeen(){
        return patientSeen;
    }
    public Vector3 getSeenPatientLocation(){
        return playerTrans.position;
    }
    //Detect perspective field of view for the AI Character
    void DetectAspect()
    {
        RaycastHit hit;
        //Direction from current position to player position
        rayDirection = playerTrans.position - transform.position;
        //Check the angle between the AI character's forward
        //vector and the direction vector between player and AI
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection,
                out hit, ViewDistance))
            {
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
                          patientSeen = true;
                    }
                }
            }
        }
    }
}
