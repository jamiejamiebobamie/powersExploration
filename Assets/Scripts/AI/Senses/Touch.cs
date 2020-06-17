using UnityEngine;
public class Touch : Sense
{
    private Transform playerTrans;
    private bool patientTouched;
    protected override void Initialize()
    {
        //Find player position
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        patientTouched = false;
    }
    public bool getPatientTouched(){
        return patientTouched;
    }
    public Vector3 getSeenPatientLocation(){
        return playerTrans.position;
    }
    void OnTriggerEnter(Collider other)
    {
        Stimulus stimulus = other.GetComponent<Stimulus>();
        if (stimulus != null)
        {
            Stimulus.origin currentOrigin = stimulus.GetCurrentOrigin();
            //Check the aspect
            if (currentOrigin == desiredStimulusOrigin
                || currentOrigin == Stimulus.origin.Sneaking)
            {
                print("Touched " + desiredStimulusOrigin + "!");
                patientTouched = true;
            }
        }
    }
}
