using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleLinkPower : MonoBehaviour, IPowerable
{
    Dictionary<Vector3, TeleLinkObject> TeleLinkLookUp
        = new Dictionary<Vector3, TeleLinkObject>();

    private void Start()
    {
        //TeleLinkObject[] teleportLocations = FindObjectsOfType<TeleLinkObject>();

        GameObject[] allObjectsInLevel = FindObjectsOfType<GameObject>();

        foreach (GameObject o in allObjectsInLevel)
        {
            ITeleportable telePortComponent = o.GetComponent<ITeleportable>();
            if (telePortComponent != null)
            {
                Vector3 TeleLinkPosition = telePortComponent.ReturnPosition();
                //o = new TeleLinkObject; // need to upcast or downcast o into a TeleLinkObject
                //TeleLinkLookUp.Add(TeleLinkPosition, (TeleLinkObject)o);
            }
        }

    }

    public void ActivatePower()
    {
        return;
    }
}
