using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkingPower1 : MonoBehaviour, IPowerable
{
    // second attempt at the wall walking ability.
    // press mouse button to activate and deactivate.
    // power does not use physics or gravity.

    RaycastHit hitForward,hitLeft,hitRight, hitBackward, hitUp, hitDown;
	bool powerActivated;
    ReturnInfo hitNormal;
    float elapsedTime;
    int hitDistance = 5;
    [SerializeField] Rigidbody rb;
    ReturnInfo testInfo;
    Quaternion playerStartRotation;

    struct ReturnInfo
    {
        public string logString;
        public Vector3 hitNormal;
    }

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;

        playerStartRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        testInfo.hitNormal = Vector3.up;
    }

    public void ActivatePower()
    {
		activatePower();
    }

    // this power will be activated on button down and deactivated on buttonup
    // functionality currently not implemented like this.
    void activatePower()
    {
        if (powerActivated)
        {
            powerActivated = false;
            rb.useGravity = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, playerStartRotation, .5f);
        }
        else
        {
            powerActivated = true;
            rb.useGravity = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (powerActivated)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 1.0f)
                hitNormal = RayCastCardinalDirections();

            Debug.Log(hitNormal.hitNormal);

            Quaternion rot = Quaternion.FromToRotation(Vector3.up, hitNormal.hitNormal);
            if (transform.rotation != rot)
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.05f);
        }
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, playerStartRotation, .05f);
        }

    ReturnInfo RayCastCardinalDirections()
	{
        string logWalls = "";
        int count = 0;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward, hitDistance))
        {
            logWalls += "hitForward";
            testInfo.hitNormal += hitForward.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft, hitDistance))
        {
            logWalls += "hitLeft";
            testInfo.hitNormal += hitLeft.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight, hitDistance))
        {
            logWalls += "hitRight";
            testInfo.hitNormal += hitRight.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitBackward, hitDistance))
        {
            logWalls += "hitBackward";
            testInfo.hitNormal += hitBackward.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitUp, hitDistance))
        {
            logWalls += "hitUp";
            testInfo.hitNormal += hitUp.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown, hitDistance))
        {
            logWalls += "hitDown";
            testInfo.hitNormal += hitDown.normal;
            count++;
        }

        testInfo.logString = logWalls;
        testInfo.hitNormal /= count;
        testInfo.hitNormal = Vector3.Normalize(testInfo.hitNormal);

        if (float.IsNaN(testInfo.hitNormal.x))
            //if (double.IsPositiveInfinity(testInfo.hitNormal.y))
            {
                testInfo.hitNormal = Vector3.up;

        }

        return testInfo;
    }
}
