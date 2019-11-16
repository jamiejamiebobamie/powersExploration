using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkingPower1 : MonoBehaviour, IPowerable
{

    RaycastHit hitForward,hitLeft,hitRight, hitBackward;
	bool powerActivated;
    ReturnInfo logTheWalls;
    float elapsedTime;
    int hitDistance = 5;
    [SerializeField] Rigidbody rb;

    Quaternion playerStartRotation;

    struct ReturnInfo
    {
        public string logString;
        public Vector3 hitNormal;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerStartRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        elapsedTime = Time.deltaTime;
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

            // need to implement a second or half second delay on calling method.
            logTheWalls = RayCastCardinalDirections();
            Debug.Log(logTheWalls.hitNormal);

            // just for testing
            Debug.Log(logTheWalls.logString);

            //Vector3 rot = (logTheWalls.hitNormal);// + transform.TransformDirection(Vector3.forward)) / 2;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, logTheWalls.hitNormal);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.05f);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, playerStartRotation, .05f);
        }
            //} else if (Vector3.forward.z > Vector3.forward.x)
            //{
            //    if (transform.rotation.z - playerStartRotation.z > .002f)
            //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, playerStartRotation.z), .05f);
            //    else
            //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, playerStartRotation.x), .05f);

            //}
        }

    ReturnInfo RayCastCardinalDirections()
	{
        ReturnInfo testInfo = new ReturnInfo();
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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitRight, hitDistance))
        {
            logWalls += "hitUp";
            testInfo.hitNormal += hitRight.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitBackward, hitDistance))
        {
            logWalls += "hitDown";
            testInfo.hitNormal += hitBackward.normal;
            count++;
        }

        testInfo.logString = logWalls;
        testInfo.hitNormal /= count;

        return testInfo;
    }
}
