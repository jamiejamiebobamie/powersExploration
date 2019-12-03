using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatPower_OriginalImplementation : MonoBehaviour, IPowerable
{
    private GameObject[] gameObjects;
    private List<ICopyable> Copyables = new List<ICopyable>();

    private Mesh baseMesh;
    public Stimulus stimulus;

    // keeps track of if the player is an object and sneaking due to movement.
    bool movingObject;
    private Rigidbody rb;

    void Start()
    {
        movingObject = false;
        rb = GetComponent<Rigidbody>();
        baseMesh = gameObject.GetComponent<MeshFilter>().mesh;

        // future / final implementation will add copyables to Copyables list
        // with a Singleton Observable.
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
        // ShedSkin();
        // revert to base form and fire the current mesh like a projectile.
        return;
    }

    private void Copy()
    {
        Mesh closestMesh = baseMesh;
        Stimulus.origin meshStimulusOrigin = Stimulus.origin.Patient;
        float minDist = Mathf.Infinity;

        foreach (ICopyable copyable in Copyables)
        {
            float testDist = Vector3.Distance(transform.position,
                copyable.GetPosition());

            if (minDist > testDist)
            {
                minDist = testDist;
                closestMesh = copyable.GetMesh();
                meshStimulusOrigin = copyable.GetOriginOfStimulus();
            }
        }

        if (minDist > 5f)
        {
            closestMesh = baseMesh;
            meshStimulusOrigin = Stimulus.origin.Patient;
        }

        GetComponent<MeshFilter>().mesh = closestMesh;
        stimulus.SetCurrentOrigin(meshStimulusOrigin);
    }

    private void Update()
    {
        if (rb.velocity.magnitude > .00001
            && stimulus.GetCurrentOrigin() == Stimulus.origin.Object)
        {
            stimulus.SetCurrentOrigin(Stimulus.origin.Sneaking);
            movingObject = true; // storing the Object aspect;
        }
        else if (rb.velocity.magnitude < .000005 && movingObject)
        {
            stimulus.SetCurrentOrigin(Stimulus.origin.Object);
            movingObject = false;
        }
    }
}
