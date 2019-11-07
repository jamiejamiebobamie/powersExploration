using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatPower : MonoBehaviour, IPowerable
{
    private GameObject[] gameObjects;
    private List<GameObject> copyables = new List<GameObject>();

    private Mesh baseMesh;
    public bool guardForm;

    public Aspect aspect;


    void Start()
    {

        baseMesh = gameObject.GetComponent<MeshFilter>().mesh;
        gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in gameObjects)
        {
            if (obj.GetComponent<ICopyable>() != null)
            {
                copyables.Add(obj);
            }
        }
    }

    public void ActivatePower()
    {
        GetComponent<MeshFilter>().mesh = ChooseForm();
    }

    private Mesh ChooseForm()
    {
        Mesh closestMesh = baseMesh;
        float minDist = Mathf.Infinity;
        guardForm = false;
        foreach (GameObject obj in copyables)
        {
            float testDist = Vector3.Distance(transform.position, obj.transform.position);
            if (minDist > testDist)
            {
                minDist = testDist;
                closestMesh = obj.GetComponent<MeshFilter>().mesh;

                if (obj.GetComponent<Guard>() != null)
                    aspect.aspectName = Aspect.aspect.Enemy;
                else
                    aspect.aspectName = Aspect.aspect.Object;
            }
        }
        if (minDist > 5f)
        {
            closestMesh = baseMesh;
            aspect.aspectName = Aspect.aspect.Player;
        }
        return closestMesh;
    }
}