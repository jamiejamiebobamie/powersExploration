using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLogic : MonoBehaviour
{
    private Humanoid targetPatient;

    private List<Humanoid> patients = new List<Humanoid>();

    private void Start()
    {
        Object[] potentialPatients =
            Resources.FindObjectsOfTypeAll(typeof(GameObject));

        foreach (object obj in potentialPatients)
        {
            GameObject test = (GameObject)obj;
            Humanoid testForHumanoid = test.GetComponent<Humanoid>();
            if (testForHumanoid != null)
            {
                Stimulus.origin testForStimulus
                    = testForHumanoid.GetOriginOfStimulus();
                if (testForStimulus == Stimulus.origin.Patient)
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
        // Check if we're near the destination position
        if (Vector3.Distance(targetPatient.GetPosition(),
            transform.position) <= 1.0f)
            targetPatient = ChooseNewTargetPatient();

        Wander();
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
