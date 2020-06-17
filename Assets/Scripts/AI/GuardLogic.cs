using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLogic : MonoBehaviour
{
    // reference to gameObject's humanoid script
    [SerializeField] private Humanoid self;
    // reference to senses scripts
    [SerializeField] private Sight sight;
    [SerializeField] private Touch touch;
    [SerializeField] private Hearing hearing;
    private bool patientSeen;
    // current target.
    private Humanoid targetPatient;
    // list of possible targets
    private List<Humanoid> patients = new List<Humanoid>();
    private void Start()
    {
        patientSeen = false;
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
        if (! self.GetIsBurning())
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
            }
            else
            {
                Vector3 playerLocation = sight.getSeenPatientLocation();
                // attack and chase patient.
                Chase(playerLocation);
            }
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
        // RayCastToTarget every 2 seconds
        // if hit (paitent)
            // Shoot()
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
        Debug.Log(newTarget.name);
        return newTarget;
    }

    private void RayCastToTarget(){}
    private void Shoot(){}
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
