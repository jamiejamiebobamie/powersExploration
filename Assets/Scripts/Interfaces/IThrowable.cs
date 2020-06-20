using UnityEngine;
using System.Collections;

// An interface for objects that can be thrown by telekinesis and
    // become telekinetic projectiles.


public interface IThrowable
{
    bool GetIsProjectile();
    
    void setIsThrowable(bool boolean);

    IEnumerator SetIsProjectileToFalse();
    void BecomeProjectile(Vector3 destination);

    IEnumerator Orbit();

    void SetOrbitHeight(float desiredHeight);
    float GetOrbitHeight();

    void SetOrbitTranslationSpeed(float desiredSpeed);
    float GetOrbitTranslationSpeed();

    void SetOrbitRotationSpeed(float desiredSpeed);
    float GetOrbitRotationSpeed();

    void SetObjectToOrbit(GameObject objectToOrbit);

    Vector3 GetPosition();
}
