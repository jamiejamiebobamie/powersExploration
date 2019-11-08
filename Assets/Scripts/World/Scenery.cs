using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenery : MonoBehaviour, ICopyable, IBurnable
{
    [SerializeField] GameObject fireStandIn;
    public Mesh Copy()
    {
        Mesh gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;
        return gameObjectMesh;
    }

    public void Burns()
    {
        Instantiate(fireStandIn, transform.position, Quaternion.identity);
    }
}
