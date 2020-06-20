using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkingPower : MonoBehaviour//, IPowerable
{

    // raycast to wall and using the hit normal
    // recieve an enum: floor, north, south, east west, cieling.
    // the player lerps to the correct...
    //                          ...rb force
    //                          ...rotation
    //                          ...forward/backward/right/left controls (no lerp)
    //                          from his current settings.

    //                          capsule controller grounded sensor should control force
    //                          no rb gravity for character, just force of power.
    // the grounded bool needs to be controlled by reaching a certain distance from the plane / axis of the wall / ground.

        // hit normal
        // west (1,0,0)
        // north (0,0,-1)
        // east (-1,0,0)
        // south (0,0,1)

    bool rayCasted;
    RaycastHit hit;
    Quaternion rotateToPosition;
    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ActivatePower1()
    {
        setGravity();
    }

    public void ActivatePower2()
    {
        return;
    }

    // need to raycast for layers == walls only

    void setGravity()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 50))
        {
            Debug.Log(hit.normal);
            //Debug.Log(transform.TransformDirection(Vector3.forward));



            //Vector3 rotateToPositionEuler = transform.rotation.eulerAngles + (hit.normal * 90);
            //rotateToPosition = Quaternion.Euler(rotateToPositionEuler);
            //rayCasted = true;
            //transform.rotation = Quaternion.Euler(rotateToPositionEuler);
            //Debug.Log(rotateToPositionEuler);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(hit.normal * -10f);
        //Debug.Log(rb.velocity.magnitude);
        //if (rayCasted)
        //{
        //    if (transform.rotation != rotateToPosition)
        //    {
        //        Quaternion.Slerp(transform.rotation, rotateToPosition, Time.deltaTime * 5.0f);
        //    }
        //    else
        //    {
        //        rayCasted = false;
        //    }
        //}
    }
}
