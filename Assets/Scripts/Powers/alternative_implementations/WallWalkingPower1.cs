﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkingPower1 : MonoBehaviour//, IPowerable
{
    // second attempt at the wall walking ability.
    // press mouse button to activate and deactivate.
    // power does not use physics or gravity.

    RaycastHit hitForward,hitLeft,hitRight, hitBackward, hitUp, hitDown;
	bool powerActivated;
    Vector3 upNormalFromPower;
    float elapsedTime;
    int hitDistance = 5;
    [SerializeField] Rigidbody rb;
    Quaternion playerStartRotation;
    bool playerNormallyRotated = false;

    //takes in a hitNormal from the wall and returns a Euler rotation.
    Dictionary<Vector3, Vector3> WallRotate = new Dictionary<Vector3, Vector3>();

    Quaternion rot;
    //Vector3 forw;

    // Start is called before the first frame update
    void Awake()
    {
        // floor
        WallRotate.Add(new Vector3(0,1,0), new Vector3(0, 0, 90));

        // cieling
        WallRotate.Add(new Vector3(0, -1, 0), new Vector3(0, 0, -90));

        // north
        WallRotate.Add(new Vector3(0, 0, 1), new Vector3(0, 0, -180));


        elapsedTime = 0.0f;

        playerStartRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        upNormalFromPower = Vector3.up;
        //forw = transform.forward;
    }

    public void ActivatePower1()
    {
		activatePower();
    }

    public void ActivatePower2()
    {
        return;
    }

    // this power will be activated on button down and deactivated on buttonup
    // functionality currently not implemented like this.
    void activatePower()
    {
        if (powerActivated)
        {
            powerActivated = false;
            rb.useGravity = true;
            //rb.isKinematic = true;
        }
        else
        {
            powerActivated = true;
            rb.useGravity = false;
            //rb.isKinematic = false;
            playerNormallyRotated = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (powerActivated)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 1.0f)
            {
                upNormalFromPower = RayCastCardinalDirections();
                //forw = transform.forward;
            }


            //Quaternion rot1 = Quaternion.FromToRotation(Vector3.forward, forw);

            Quaternion rot2 = Quaternion.FromToRotation(Vector3.up, upNormalFromPower);
            // multiplying two quaternions has the effect of combinign two rotations
            // rotating one way and then rotating another.

            //Quaternion rot3 = Quaternion.FromToRotation(Vector3.forward, transform.forward);

            //rot2 *= transform.rotation;
            //Quaternion rot3 = rot2* rot1;

            //if (WallRotate.ContainsKey(upNormalFromPower))
            //{
            //    rot = Quaternion.Euler(WallRotate[upNormalFromPower]);
            //}


            Debug.Log(rot);

            if (transform.up != upNormalFromPower)
                //if (elapsedTime >= 1.0f)
                transform.rotation = Quaternion.Lerp(transform.rotation, rot2, .1f);
        }
        else
        {
            // attempting to reset after each use of the power, but it is not working.
            if (!playerNormallyRotated)
            {
                if (transform.rotation != Quaternion.identity)
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.05f);
                else
                    playerNormallyRotated = true;
            }

            //upNormalFromPower = Vector3.zero;
            //gameObject.transform.up = Vector3.up;
        }
    }

    Vector3 RayCastCardinalDirections()
	{
        //string logWalls = "";
        int count = 0;
        upNormalFromPower = Vector3.zero;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward, hitDistance))
        {
            upNormalFromPower += hitForward.normal;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward, hitDistance))
        {
            //logWalls += "hitForward";
            upNormalFromPower += hitForward.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft, hitDistance))
        {
            //logWalls += "hitLeft";
            upNormalFromPower += hitLeft.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight, hitDistance))
        {
            //logWalls += "hitRight";
            upNormalFromPower += hitRight.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitBackward, hitDistance))
        {
            //logWalls += "hitBackward";
            upNormalFromPower += hitBackward.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitUp, hitDistance))
        {
            //logWalls += "hitUp";
            upNormalFromPower += hitUp.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown, hitDistance))
        {
            //logWalls += "hitDown";
            upNormalFromPower += hitDown.normal;
            count++;
        }

        if (count == 0)
        {
            powerActivated = false;
            rb.useGravity = true;
            return Vector3.up;
        }

        upNormalFromPower /= count;
        upNormalFromPower = Vector3.Normalize(upNormalFromPower);

        if (float.IsNaN(upNormalFromPower.x))
        //if (double.IsPositiveInfinity(testInfo.hitNormal.y))
        {
            upNormalFromPower = Vector3.up;

        }

        return upNormalFromPower;
    }
}
