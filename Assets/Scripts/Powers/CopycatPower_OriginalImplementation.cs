using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatPower_OriginalImplementation : MonoBehaviour, IPowerable
{
    private GameObject[] gameObjects;
    private List<ICopyable> Copyables = new List<ICopyable>();

    private Mesh baseMesh;
    private Stimulus stimulus;

    // keeps track of if the player is an object
        // and sneaking due to movement.
    bool movingObject;
    private Rigidbody rb;

    void Start()
    {
        movingObject = false;
        rb = GetComponent<Rigidbody>();
        baseMesh = gameObject.GetComponent<MeshFilter>().mesh;
        stimulus = GetComponent<Stimulus>();


        gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in gameObjects)
        {
            ICopyable test_ICopyable = obj.GetComponent<ICopyable>();
            if (test_ICopyable != null)
            {
                Copyables.Add(test_ICopyable);
            }
        }
        StartCoroutine("UpdateForm");
    }

    public void ActivatePower1()
    {
        // activate the power of the
            // character or object you're copying.
        return;
    }

    public void ActivatePower2()
    {
        Copy();
    }

    private void Copy()
    {
        Mesh closestMesh = baseMesh;
        Stimulus.origin meshStimulusOrigin = Stimulus.origin.Patient;
        float minDist = Mathf.Infinity;

        foreach (ICopyable copyable in Copyables)
        {
            Vector3 positionOfCopyable = copyable.GetPosition();

            float testDist = Vector3.Distance(transform.position,
                positionOfCopyable);

            if (minDist > testDist)
            {
                Ray testLineOfSight = new Ray();

                testLineOfSight.origin = transform.position;
                testLineOfSight.direction =
                    positionOfCopyable - transform.position;

                RaycastHit hit;

                // test if the copyable can be seen by the player
                if(Physics.Raycast(testLineOfSight, out hit))
                {
                    ICopyable testCopyable =
                        hit.transform.gameObject.GetComponent<ICopyable>();

                    if (testCopyable != null)
                    {
                        minDist = testDist;
                        closestMesh = testCopyable.GetMesh();
                        meshStimulusOrigin = testCopyable.GetOriginOfStimulus();
                    }
                }
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

    public IEnumerator UpdateForm()
    {
        Vector3 storePosition = Vector3.zero;
        float distance = 0f;

        while (true)
        {
            distance = Vector3.Distance(storePosition,
                transform.position);

            if (distance > .5f
                && stimulus.GetCurrentOrigin() == Stimulus.origin.Object)
            {
                stimulus.SetCurrentOrigin(Stimulus.origin.Sneaking);
                movingObject = true; // storing the Object aspect;
            }
            else if (distance < .5f && movingObject)
                {
                    stimulus.SetCurrentOrigin(Stimulus.origin.Object);
                    movingObject = false;
                }
            storePosition = transform.position;
            yield return new WaitForSeconds(.25f);
        }
    }
}
