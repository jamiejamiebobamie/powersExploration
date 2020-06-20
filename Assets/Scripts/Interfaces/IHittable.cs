using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface for objects that can be hit by telekinetic projectiles.
public interface IHittable
{
    void ApplyHitForce(Vector3 hitDirection, float hitStrength, string projectileOwner);
    bool GetIsStaggered();
    void SetIsStaggered(bool value);
}
