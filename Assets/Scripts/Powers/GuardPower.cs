using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPower : PowersSuperClass, IPowerable
{

    [SerializeField] private Rigidbody projectile;
    [SerializeField] private Transform barrelEnd;

    private Stimulus stimulus;

    private void Start()
    {
        stimulus = GetComponent<Stimulus>();
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
        if (stimulus.GetCurrentOrigin() == Stimulus.origin.Incapacitated)
            stimulus.SetCurrentOrigin(Stimulus.origin.Guard);
        else
            stimulus.SetCurrentOrigin(Stimulus.origin.Incapacitated);
    }

    public PowersSuperClass InstantiatePower()
    {
        PowersSuperClass instanceOfGuardPower = new GuardPower();
        return instanceOfGuardPower;
    }
}
