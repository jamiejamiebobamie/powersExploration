using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour, ICopyable, IBurnable, IHittable//, //ITargetable//, ITranquilizable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] GameObject fire;

     // boolean to show humanoid has been hit
    private bool isStaggered ,isIncapacitated, isBurning;

    private int hitCount;

    private Aspect.aspect aspect;


    private void Start()
    {
        aspect = GetComponent<Aspect>().currentAspect;

        isBurning = false;
        isStaggered = false;
        isIncapacitated = false;
        hitCount = 0;
    }

    public Vector3 GetLocation()
    {
        return transform.position;
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

    public Aspect.aspect GetAspect()
    {
        return aspect;
    }

    public void Burns()
    {
        GameObject fireInstance = Instantiate(fire, transform.position, Quaternion.identity);
        isBurning = true;
        Destroy(fireInstance, 3f);
        // other functionality. instantiate "burned" model after x seconds, etc.
    }

    public void ApplyHitForce(Vector3 hitForce, float hitStrength)
    {
        //Debug.Log(rb);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(hitForce * hitStrength * 10f);
        // activate ragdoll
        isStaggered = true;
    }

    // implement a RecoverFromHit method.


    public void setIncapacitated(bool value)
    {
        isIncapacitated = value;
    }

    public bool getIncapacitated()
    {
        return isIncapacitated;
    }
}
