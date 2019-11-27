using UnityEngine;
using System.Collections;

public interface ICopyable
{
	Mesh GetMesh();
    Vector3 GetPosition();
    bool IsGuard();
    Aspect.aspect GetAspect();
}
