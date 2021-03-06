﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPower : Power//, IPowerable
{

    [SerializeField] private Rigidbody projectile;
    [SerializeField] private Transform barrelEnd;

    private Stimulus stimulus;

    private void Start()
    {
        stimulus = GetComponent<Stimulus>();
    }

    public override void ActivatePower1()
    {
        FireTranquilizerDart();
    }

    public override void ActivatePower2()
    {

        // isIncapacitated = true

        // Ideas:
        // pick up / use / throw item.
        // sneak / crouch
        // lean against wall (fire around corners)

        // pretend to be dead
        PlayDead();
    }

    private void FireTranquilizerDart()
    {
        Rigidbody projectileInstance;
        projectileInstance = Instantiate(projectile,
            barrelEnd.position, transform.rotation) as Rigidbody;
        projectileInstance.AddForce(barrelEnd.forward * 1350f);
    }

    public Vector3 getBarrelLocation()
    {
        return barrelEnd.position;
    }

    public Vector3 getBarrelDirection()
    {
        return barrelEnd.forward;
    }

    // this needs some more thought...
    private void PlayDead()
    {
        if (stimulus.GetCurrentOrigin() == Stimulus.origin.Incapacitated)
            stimulus.SetCurrentOrigin(Stimulus.origin.Guard);
        else
            stimulus.SetCurrentOrigin(Stimulus.origin.Incapacitated);
    }
}
