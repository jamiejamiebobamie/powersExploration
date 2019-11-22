using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleLinkObject : MonoBehaviour, ITeleportable
{
    private void Start()
    {
       GameObject[] allObjectsInLevel = FindObjectsOfType<GameObject>();

        foreach (GameObject o in allObjectsInLevel)
        {
            if (o.GetComponent<TeleLinkPower>() != null)
            {
                return;//idk too tired.
            }
        }
        
    }
    public float ReturnDistance()
    {
        return transform.position;
    }
}
