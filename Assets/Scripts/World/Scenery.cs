using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenery : MonoBehaviour, ICopyable
{
    public Mesh Copy()
    {
        Mesh gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;
        return gameObjectMesh;
    }
}
