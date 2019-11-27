using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatPower_OriginalImplementation : MonoBehaviour, IPowerable
{
    private GameObject[] gameObjects;
    private List<ICopyable> Copyables = new List<ICopyable>();

    private Mesh baseMesh;
    public bool guardForm;

    public Aspect aspect;


    void Start()
    {

        baseMesh = gameObject.GetComponent<MeshFilter>().mesh;
        gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in gameObjects)
        {
            ICopyable test_ICopyable = obj.GetComponent<ICopyable>();
            if (test_ICopyable != null)
            {
                Copyables.Add(test_ICopyable);
            }
        }
    }

    public void ActivatePower1()
    {
        Copy();
    }

    public void ActivatePower2()
    {
        return;
    }

    private void Copy()
    {
        Mesh closestMesh = baseMesh;
        float minDist = Mathf.Infinity;
        guardForm = false;
        foreach (ICopyable copyable in Copyables)
        {
            float testDist = Vector3.Distance(transform.position, copyable.GetPosition());
            if (minDist > testDist)
            {
                minDist = testDist;
                closestMesh = copyable.GetMesh();

                if (copyable.IsGuard())
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
        GetComponent<MeshFilter>().mesh = closestMesh;
    }
}
