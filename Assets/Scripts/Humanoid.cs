using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour, ICopyable, IBurnable, IHittable
{
    private bool isBurning;
    private bool isStaggered; // boolean to show humanoid has been hit

    [SerializeField] private Rigidbody rb;
    [SerializeField] GameObject fireStandIn;

    private Aspect.aspect aspect;

    private void Start()
    {
        aspect = GetComponent<Aspect>().aspectName;
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

    public virtual bool IsGuard()
    {
        Debug.Log("hola");

        return true;// this should return false... but "override" keyword isn't working...
    }

    public Aspect.aspect GetAspect()
    {
        return aspect;
    }

    public void Burns()
    {
        Instantiate(fireStandIn, transform.position, Quaternion.identity);
        isBurning = true;
        // other functionality. instantiate "burned" model after x seconds, etc.
    }

    public void ApplyHitForce(Vector3 hitForce, float hitStrength)
    {
        Debug.Log(rb);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(hitForce * hitStrength * 10f);
        // activate ragdoll
        isStaggered = true;
    }

    // implement a RecoverFromHit method.
}
