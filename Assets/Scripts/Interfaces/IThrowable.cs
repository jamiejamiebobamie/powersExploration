using UnityEngine;
using System.Collections;

public interface IThrowable
{
    bool GetIsProjectile();
    void BecomeProjectile(Vector3 destination);

    void Orbit();

    void SetOrbitHeight(float desiredHeight);
    float GetOrbitHeight();

    void SetOrbitTranslationSpeed(float desiredSpeed);
    float GetOrbitTranslationSpeed();

    void SetOrbitRotationSpeed(float desiredSpeed);
    float GetOrbitRotationSpeed();

    void SetObjectToOrbit(GameObject objectToOrbit);

    Vector3 GetPosition();
}
