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

    void PlayDead()
    {
        if (body.GetIncapacitated())
        {
            aspect.setCurrentAspect(Aspect.aspect.Guard);
            body.SetIncapacitated(false);
        }
        else
        {
            aspect.setCurrentAspect(Aspect.aspect.Incapacitated);
            body.SetIncapacitated(true);
        }
    }

}
