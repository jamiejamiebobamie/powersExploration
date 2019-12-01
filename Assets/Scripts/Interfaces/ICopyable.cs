using UnityEngine;
using System.Collections;

public interface ICopyable
{
	Mesh GetMesh();
    Vector3 GetPosition();
    Aspect.aspect GetAspect();
}
