using UnityEngine;
using System.Collections;

public interface IThrowable
{
	void BecomeProjectile(Vector3 destination);

    void Orbit();

    void SetOrbitHeight(float desiredHeight);
    float GetOrbitHeight();

    void SetOrbitTranslationSpeed(float desiredSpeed);
    float GetOrbitTranslationSpeed();

    void SetOrbitRotationSpeed(float desiredSpeed);
    float GetOrbitRotationSpeed();
}
