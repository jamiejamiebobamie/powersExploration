using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleLinkPower : MonoBehaviour, IPowerable
{
    private void Start()
    {
        TeleLinkObject[] teleportLocations = FindObjectsOfType<TeleLinkObject>();
    }

    public void ActivatePower()
    {
        return;
    }
}
