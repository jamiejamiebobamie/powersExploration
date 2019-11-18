using UnityEngine;
using System.Collections;

public interface IThrowable
{
	void BecomeProjectile(Vector3 destination);
    void Orbit();
}
