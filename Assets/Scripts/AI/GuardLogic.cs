using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLogic : MonoBehaviour
{
    // reference to gameObject's humanoid script
    [SerializeField] private Humanoid self;
    [SerializeField] private GuardPower abilities;
    // reference to senses scripts
    [SerializeField] private Sight sight;
    [SerializeField] private Touch touch;
    [SerializeField] private Hearing hearing;
    private bool patientSeen;
    private bool isFiring;
    // current target.
    private Humanoid targetPatient;
    // list of possible targets
    private List<Humanoid> patients = new List<Humanoid>();
    private void Start()
    {
        patientSeen = false;
        isFiring = false;
        Object[] potentialPatients =
            Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (object obj in potentialPatients)
        {
            GameObject go = (GameObject)obj;
            Humanoid testForHumanoid = go.GetComponent<Humanoid>();
            if (testForHumanoid)
            {
                Stimulus.origin testForStimulus
                    = testForHumanoid.GetOriginOfStimulus();
                if (testForStimulus == Stimulus.origin.Patient && !patients.Contains(testForHumanoid))
                {
                    patients.Add(testForHumanoid);
                }
            }
        }
        if (patients.Count > 0)
        {
            targetPatient = ChooseNewTargetPatient();
            if (targetPatient == null)
                targetPatient = GetComponent<Humanoid>();
        }
        else
        {
            targetPatient = GetComponent<Humanoid>();
        }
    }
    void Update()
    {
        if (!self.GetIsBurning() && !self.GetIsStaggered() && !self.GetIncapacitated()) 
        {
            if (!sight.getPatientSeen()
                && !hearing.getPatientHeard()
                && !touch.getPatientTouched())
            {
                // Check if we're near the destination position
                if (Vector3.Distance(targetPatient.GetPosition(),
                    transform.position) <= 1.0f)
                    targetPatient = ChooseNewTargetPatient();
                Wander();
                if (isFiring)
                {
                    StopCoroutine("RayCastToTarget");
                    isFiring = false;
                }
            }
            else
            {
                Vector3 playerLocation = sight.getSeenPatientLocation();
                if (Vector3.Distance(playerLocation,
                    transform.position) <= 15.0f)
                    {
                        Quaternion tarRot = Quaternion.LookRotation(playerLocation
                            - transform.position);

                        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
                            2.0f * Time.deltaTime);
                            playerLocation = transform.position;
                    }
                else
                {
                    Chase(playerLocation);
                }
                if (!isFiring)
                {
                    playerLocation = sight.getSeenPatientLocation();
                    if (Vector3.Distance(playerLocation,
                        transform.position) <= 15.0f)
                        {
                    // abilities.ActivatePower1();
                        StartCoroutine("RayCastToTarget");
                        isFiring = true;
                } else {
                    StopCoroutine("RayCastToTarget");
                }
            }
        }
    }
    else
    {
        StopCoroutine("RayCastToTarget");
    }
}
    private void Wander()
    {
        // Set up quaternion for rotation toward destination
        Quaternion tarRot = Quaternion.LookRotation(targetPatient.GetPosition()
            - transform.position);
        // Update rotation and translation
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
            2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 1.0f * Time.deltaTime));
    }

    private void Chase(Vector3 goHere)
    {
        // Set up quaternion for rotation toward destination
        Quaternion tarRot = Quaternion.LookRotation(goHere
            - transform.position);
        // Update rotation and translation
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
            2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 5.0f * Time.deltaTime));
    }

    private Humanoid ChooseNewTargetPatient()
    {
        float minDistance = Mathf.Infinity;
        Humanoid newTarget = null;
        foreach (Humanoid patient in patients)
        {
            if (!patient.GetIncapacitated())
            {
                float testDistance
                    = Vector3.Distance(transform.position,
                    patient.GetPosition());
                if (testDistance < minDistance)
                {
                    minDistance = testDistance;
                    newTarget = patient;
                }
            }
        }
        return newTarget;
    }

    private IEnumerator RayCastToTarget()
    {
        while (true)
        {
            Debug.Log("fire");
            //
            // Ray ray = new Ray();
            // Vector3 gunLocation = abilities.getBarrelLocation()+transform.forward*10f;
            // ray.origin = gunLocation;
            // ray.direction = transform.forward;
            //
            // RaycastHit hit;
            // if (Physics.Raycast(ray, out hit))
            // {
            //     GameObject test = hit.transform.gameObject;
            //     // Humanoid testHumanoid = test.GetComponent<Humanoid>();
            //     Debug.Log(test,targetPatient);
            //         if (test == targetPatient){
                        // FireTranquilizerDart()
                        abilities.ActivatePower1();
            //         }
            // }
            yield return new WaitForSeconds(2f);

        }
    }
}

// STATES:
// NOT TARGET -- HUNT LOST TARGET
/*
 * create vector that forms a direction to last sighting
 *
 * create waypoints to patrol from last sighting position
 *
 * use chance to have guards approach and/or shoot scenery around
 * waypoints
 *
 * revert back to NO TARGET -- PATROL once waypoints have been visited.
 *
 */

// NO TARGET -- PATROL
/*
 * raycast 360 degrees around guard and store the position with the
 * longest distance that is not the second to last
 * position visited.
 *
 * move to that position.
 *
 * choose a new position.
 * (go to NO TARGET -- PATROL.)
 *
 */

// FOUND TARGET -- IN RANGE
/*
 * shoot target with Tranquilizer.
 *
 * other logic ... like runaway, play dead, and shoot from cover...
 * seems overly-complicated to implement.
 *
 * go to FOUND TARGET -- OUT OF RANGE state if target moves out of range.
 *
 */

// FOUND TARGET -- OUT OF RANGE
/*
 * move toward target until in range.
 * go to FOUND TARGET -- IN RANGE state.
 *
 */
