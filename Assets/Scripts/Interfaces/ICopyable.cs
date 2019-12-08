using UnityEngine;
using System.Collections;

public interface ICopyable
{
	Mesh GetMesh();
    Vector3 GetPosition();
    Stimulus.origin GetOriginOfStimulus();
    IPowerable GetPower();
}
