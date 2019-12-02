﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour, ICopyable, IBurnable, IHittable//, //ITargetable//, ITranquilizable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] GameObject fire;

     // boolean to show humanoid has been hit
    private bool isStaggered ,isIncapacitated, isBurning;
    private int hitCount;

    private Stimulus origin;


    private void Start()
    {
        origin = GetComponent<Stimulus>();
        rb = GetComponent<Rigidbody>();


        isBurning = false;
        isStaggered = false;
        isIncapacitated = false;
        hitCount = 0;
    }

    public Mesh GetMesh()
	{
		Mesh gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;
		return gameObjectMesh;
	}

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Stimulus.origin GetOriginOfStimulus()
    {
        return origin.GetCurrentOrigin();
    }

    public void Burns()
    {
        GameObject fireInstance = Instantiate(fire,
            transform.position, Quaternion.identity);
        fireInstance.transform.parent = gameObject.transform;
        isBurning = true;
        Destroy(fireInstance, 3f);
        isIncapacitated = true;
        // other functionality. instantiate "burned" model after x seconds, etc.
    }

    // implement a RecoverFromHit method.
    public void ApplyHitForce(Vector3 hitForce, float hitStrength)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(hitForce * hitStrength * 10f);
        // activate ragdoll
        isStaggered = true;
    }

    // maybe not neccessary...
    public void SetIncapacitated(bool value)
    {
        isIncapacitated = value;
    }

    public bool GetIncapacitated()
    {
        return isIncapacitated;
    }
}
