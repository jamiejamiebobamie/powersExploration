using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleLinkPower : MonoBehaviour, IPowerable
{

    [SerializeField] int numberOfTeleportPositions;

    Vector3[] TeleLinkPositions;
    int TeleLinkPositionsCount = 0;
    GameObject[] TeleLinkPositionsMarkers;
    [SerializeField] GameObject marker;
    private bool teleportPlayer;
    Vector3 portHere;

    Vector3 currentPosition;

    private void Start()
    {
        TeleLinkPositions = new Vector3[numberOfTeleportPositions];
        TeleLinkPositionsMarkers = new GameObject[numberOfTeleportPositions];
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
                //Direction from current position to player position.
                rayDirection = position - transform.position;

                //Check the angle between the player's forward vector and the 
                //direction vector between the player and the teleport position.
                if ((Vector3.Angle(rayDirection, transform.forward)) < 20)
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
                teleportPlayer = true;
                portHere = portLocation;
                // need to implement a Lerp/Slerp to location
                //currentPosition = portLocation;
                //transform.position = portLocation;
                // do i need to set the rotation?
            } else
            {
                teleportPlayer = false;
            }
        }
    }

    private void Update()
    {
        if (teleportPlayer)
            transform.Translate(transform.position - portHere, Space.World);// no idea.

    }

    void AddPositionToTeleLinkPositions()
    {
        if (TeleLinkPositionsCount >= 10)
        {
            TeleLinkPositionsCount = 0;
        } else
        {
            TeleLinkPositionsCount++;
        }

        TeleLinkPositions[TeleLinkPositionsCount] = transform.position;

        Destroy(TeleLinkPositionsMarkers[TeleLinkPositionsCount]);
        TeleLinkPositionsMarkers[TeleLinkPositionsCount] =
            Instantiate(marker, transform.position, transform.rotation);
    }

}
