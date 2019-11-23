using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyrokinesisPower : MonoBehaviour, IPowerable
{
    [SerializeField] GameObject fireStandIn;

	public void ActivatePower1()
	{
        RaycastToBurnable();
    }

    public void ActivatePower2()
    {
        return;
    }

    void RaycastToBurnable()
	{
		Ray ray = new Ray();
		ray.origin = Camera.main.transform.position;
		ray.direction = Camera.main.transform.forward;

		RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Spawn particle effect at hit point
            Instantiate(fireStandIn, hit.point, Quaternion.identity);
            IBurnable burnable = hit.transform.gameObject.GetComponent<IBurnable>();
            if (burnable != null)
                burnable.Burns();
        }
	}
    //https://docs.unity3d.com/ScriptReference/ParticleSystem.EmitParams-position.html

    // spawning a particle system

//    using UnityEngine;

//// In this example we have a Particle System emitting aligned particles; we then emit and override the position and velocity every 2 seconds.
//public class ExampleClass : MonoBehaviour
//{
//    private ParticleSystem system;

//    void Start()
//    {
//        // A simple particle material with no texture.
//        Material particleMaterial = new Material(Shader.Find("Particles/Standard Unlit"));

//        // Create a Particle System.
//        var go = new GameObject("Particle System");
//        go.transform.Rotate(-90, 0, 0); // Rotate so the system emits upwards.
//        system = go.AddComponent<ParticleSystem>();
//        go.GetComponent<ParticleSystemRenderer>().material = particleMaterial;

//        // Every 2 seconds we will emit.
//        InvokeRepeating("DoEmit", 2.0f, 2.0f);
//    }

//    void DoEmit()
//    {
//        // Any parameters we assign in emitParams will override the current system's when we call Emit.
//        // Here we will override the position and velocity. All other parameters will use the behavior defined in the Inspector.
//        var emitParams = new ParticleSystem.EmitParams();
//        emitParams.position = new Vector3(0.0f, 0.0f, 0.0f);
//        emitParams.velocity = new Vector3(0.0f, 0.0f, -2.0f);
//        system.Emit(emitParams, 1);
//    }
//}
}