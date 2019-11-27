using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPower : MonoBehaviour, IPowerable
{

    [SerializeField] private Rigidbody projectile;
    [SerializeField] private Transform barrelEnd;

    public void ActivatePower1()
    {
        FireTranquilizerDart();
    }

    public void ActivatePower2()
    {
        return;
    }

    void FireTranquilizerDart()
    {
        Rigidbody projectileInstance;
        projectileInstance = Instantiate(projectile,
            barrelEnd.position, transform.rotation) as Rigidbody;
        projectileInstance.AddForce(barrelEnd.forward * 1350f);
    }

}
