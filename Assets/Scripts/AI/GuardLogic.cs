using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLogic : MonoBehaviour
{
    private Humanoid targetPatient;
    public GameObject Floor;

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



        //targetPosition = new Vector3(Random.Range(Floor.transform.position.x
        //    - Floor.transform.localScale.x / 2,
        //    Floor.transform.position.x + Floor.transform.localScale.x / 2),
        //    2, Random.Range(Floor.transform.position.z
        //    - Floor.transform.localScale.z / 2, Floor.transform.position.z
        //    + Floor.transform.localScale.z / 2));
        Debug.Log(newTarget.name);
        return newTarget;
    }
}
