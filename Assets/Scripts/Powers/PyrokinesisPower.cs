using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyrokinesisPower : PowersSuperClass, IPowerable
{
    [SerializeField] private GameObject fire;
    private float particleSystemDuration;
    [SerializeField] private bool npc;

    private void Awake()
    {
        if (fire != null)
        {
            particleSystemDuration =
                fire.GetComponent<ParticleSystem>().main.duration / 2.5f;
        } else
        {
            particleSystemDuration = 3f;
        }
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
        GameObject fireEffect;

        if (Physics.Raycast(ray, out hit))
        {
            // Spawn particle effect at hit point
            if (fire != null)
            {
                fireEffect = Instantiate(fire,
                    hit.point, Quaternion.identity);
            }
            else
            {
                fireEffect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                fireEffect.transform.position = hit.point;
                particleSystemDuration = 3f;
            }

            Destroy(fireEffect, particleSystemDuration);

            IBurnable burnable =
                hit.transform.gameObject.GetComponent<IBurnable>();

            if (burnable != null)
                burnable.Burns();
        }
	}
    public PowersSuperClass InstantiatePower()
    {
        PowersSuperClass instanceOfTelekinesisPower = new PyrokinesisPower();
        return instanceOfTelekinesisPower;
    }
}