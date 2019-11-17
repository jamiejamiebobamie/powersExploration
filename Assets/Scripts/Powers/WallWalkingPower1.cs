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
    Vector3 upNormalFromPower;
    float elapsedTime;
    int hitDistance = 5;
    [SerializeField] Rigidbody rb;
    Quaternion playerStartRotation;
    bool playerNormallyRotated = false;

    //takes in a hitNormal from the wall and returns a Euler rotation.
    Dictionary<Vector3, Vector3> WallRotate = new Dictionary<Vector3, Vector3>();

    Quaternion rot;



    // Start is called before the first frame update
    void Start()
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
        }
        else
        {
            powerActivated = true;
            rb.useGravity = false;
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
                upNormalFromPower = RayCastCardinalDirections();

            Quaternion rot = Quaternion.FromToRotation(Vector3.up, upNormalFromPower);

            //if (WallRotate.ContainsKey(upNormalFromPower))
            //{
            //    rot = Quaternion.Euler(WallRotate[upNormalFromPower]);
            //}


            Debug.Log(upNormalFromPower);

            if (transform.up != upNormalFromPower)
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, .1f);
        }
        else
        {
            // attempting to reset after each use of the power, but it is not working.
            if (!playerNormallyRotated)
            {
                if (transform.rotation != Quaternion.identity)
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.5f);
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
