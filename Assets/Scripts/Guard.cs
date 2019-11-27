using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Humanoid
{
    Vector3 targetPosition;
    public GameObject Floor;

    private void Start()
    {
        targetPosition = transform.position;
    }

    public override bool IsGuard()
    {
        return true;
    }

    void Update()
    {
        // Check if we're near the destination position
        if (Vector3.Distance(targetPosition, transform.position) <= 5.0f)
            ChooseNewTargetPosition();

        Wander();
    }

    private void Wander()
    {
        // Set up quaternion for rotation toward destination
        Quaternion tarRot = Quaternion.LookRotation(targetPosition - transform.position);

        // Update rotation and translation
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
            2.0f * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, 1.0f * Time.deltaTime));
    }

    private void ChooseNewTargetPosition()
    {
        targetPosition = new Vector3(Random.Range(Floor.transform.position.x
            - Floor.transform.localScale.x / 2,
            Floor.transform.position.x + Floor.transform.localScale.x / 2),
            2, Random.Range(Floor.transform.position.z
            - Floor.transform.localScale.z / 2, Floor.transform.position.z
            + Floor.transform.localScale.z / 2));
    }
}
