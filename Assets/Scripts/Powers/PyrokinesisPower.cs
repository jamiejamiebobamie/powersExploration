using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyrokinesisPower : MonoBehaviour, IPowerable
{
    [SerializeField] private GameObject fire;
    private float particleSystemDuration;
    [SerializeField] private bool npc;

    private void Start()
    {
        particleSystemDuration =
            fire.GetComponent<ParticleSystem>().main.duration/2.5f;
    }

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

        if (npc)
        {
            ray.origin = transform.position;
            ray.direction = transform.forward;
        }
        else
        {
            ray.origin = Camera.main.transform.position;
            ray.direction = Camera.main.transform.forward;
        }

		RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Spawn particle effect at hit point
            GameObject fireEffect = Instantiate(fire,
                hit.point, Quaternion.identity);

            Destroy(fireEffect, particleSystemDuration);

            IBurnable burnable =
                hit.transform.gameObject.GetComponent<IBurnable>();

            if (burnable != null)
                burnable.Burns();
        }
	}
}