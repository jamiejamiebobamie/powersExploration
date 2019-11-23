using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleLinkPower : MonoBehaviour, IPowerable
{
    List<Vector3> TeleLinkPositions = new List<Vector3>();

    //struct TeleportNode
    //{
    //    public Vector3 Location;
    //    public List<TeleportNode> Neighbors;
    //}

    //TeleportNode[] TeleportNodeArray;

    Vector3 currentPosition;


    [SerializeField] GameObject marker;

    private void Start()
    {
        GameObject[] allObjectsInLevel = FindObjectsOfType<GameObject>();

        foreach (GameObject o in allObjectsInLevel)
        {
            ITeleportable telePortComponent = o.GetComponent<ITeleportable>();
            if (telePortComponent != null)
            {
                Vector3 TeleLinkPosition = telePortComponent.ReturnPosition();

                TeleLinkPositions.Add(TeleLinkPosition);
                //TeleportNode newNode = new TeleportNode();
                //newNode.Location = TeleLinkPosition;
            }
        }

        //TeleportNodeArray = new TeleportNode[TeleLinkPositions.Count - 1];

        //foreach (Vector3 positionA in TeleLinkPositions)
        //{
        //    foreach (Vector3 positionB in TeleLinkPositions)
        //    {
        //        if (positionA != positionB)
        //        {
        //            Ray ray = new Ray();

        //            Vector3 rayDirection = positionA - positionB;
        //            float distance = rayDirection.magnitude;

        //            ray.direction = rayDirection;
        //            ray.origin = positionA;

        //            RaycastHit hit = new RaycastHit();
        //            if (Physics.Raycast(ray, out hit, distance))
        //            {
        //                break; // we hit something and the way is not clear.
        //            }
        //            else
        //            {
        //                TeleportNode newNode = new TeleportNode();

        //            }
        //        }
        //    }


        //    // add a TeleportNode for each TeleLinkPosition in TeleLinkPositions.

        //}
    }

    public void ActivatePower1()
    {
        Port();
    }

    public void ActivatePower2()
    {
        AddPositionToTeleLinkPositions();
    }

    void Port()
    {
        RaycastHit hit;

        float minDistance = Mathf.Infinity;
        float distance;

        Vector3 rayDirection;

        // highly unlikely the player will
        // have put down a teleLinkPosition right on the origin.
        Vector3 portLocation = Vector3.zero;
 
        foreach (Vector3 position in TeleLinkPositions)
        {
            if (position != currentPosition)
            {
                //Direction from current position to player position
                rayDirection = position - transform.position;

                //Check the angle between the player's forward vector and 
                //the direction vector between the player and the teleport position
                if ((Vector3.Angle(rayDirection, transform.forward)) < 30)
                {
                    distance = rayDirection.magnitude;

                    // Detect if anything is obstructing the player's
                    // view to the port position.
                    if (!Physics.Raycast(transform.position, rayDirection,
                        out hit, distance))
                    {
                        if (distance < minDistance)
                        {
                            portLocation = position;
                        }
                    }
                }
            }

            if (portLocation != Vector3.zero)
            {
                // need to implement a Lerp/Slerp to location
                currentPosition = portLocation;
                transform.position = portLocation;
                // do i need to set the rotation?
            }
        }
    }

    void AddPositionToTeleLinkPositions()
    {
        TeleLinkPositions.Add(transform.position);
        Instantiate(marker, transform.position, transform.rotation);
    }

}
