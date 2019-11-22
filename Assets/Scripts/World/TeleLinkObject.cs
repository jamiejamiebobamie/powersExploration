using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleLinkObject : MonoBehaviour, ITeleportable
{
    private void Start()
    {
       //GameObject[] allObjectsInLevel = FindObjectsOfType<GameObject>();

       // foreach (GameObject o in allObjectsInLevel)
       // {
       //     if (o.GetComponent<TeleLinkPower>() != null)
       //     {
       //         ReturnDistance();
       //     }
       // }
        
    }
    public Vector3 ReturnPosition()
    {
        return transform.position;
    }
}
