using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour, ICopyable, IBurnable, IHittable
{
    private bool burning;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Mesh Copy()
	{
		Mesh gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;
		return gameObjectMesh;
	}

    [SerializeField] GameObject fireStandIn;

    public void Burns()
    {
        Instantiate(fireStandIn, transform.position, Quaternion.identity);
        burning = true;
    }

    public void ApplyHitForce(Vector3 hitForce, float hitStrength)
    {
        rb.AddForce(hitForce * hitStrength*1000f);
        // activate ragdoll
    }
}
