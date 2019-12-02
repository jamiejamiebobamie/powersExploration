using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPower : MonoBehaviour, IPowerable
{

    [SerializeField] private Rigidbody projectile;
    [SerializeField] private Transform barrelEnd;

    private Aspect aspect;
    private Humanoid body;

    private void Start()
    {
        body = GetComponent<Humanoid>();
        aspect = GetComponent<Aspect>();
    }


    public void ActivatePower1()
    {
        FireTranquilizerDart();
    }

    public void ActivatePower2()
    {

        // isIncapacitated = true
        // Ideas:
        // pick up / use / throw item.
        // sneak / crouch
        // lean against wall (fire around corners)
        // pretend to be dead
        PlayDead();
        // return;
    }

    void FireTranquilizerDart()
    {
        Rigidbody projectileInstance;
        projectileInstance = Instantiate(projectile,
            barrelEnd.position, transform.rotation) as Rigidbody;
        projectileInstance.AddForce(barrelEnd.forward * 1350f);
    }

    // this needs some more thought...
    void PlayDead()
    {
        if (body.GetIncapacitated())
        {
            aspect.SetCurrentAspect(Aspect.aspect.Guard);
            body.SetIncapacitated(false);
        }
        else
        {
            aspect.SetCurrentAspect(Aspect.aspect.Incapacitated);
            body.SetIncapacitated(true);
        }
    }

}
