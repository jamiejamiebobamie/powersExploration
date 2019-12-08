using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatPower_OriginalImplementation : PowersSuperClass, IPowerable
{
    private GameObject[] gameObjects;
    private List<ICopyable> Copyables = new List<ICopyable>();

    private Mesh baseMesh;
    private Stimulus stimulus;

    // keeps track of if the player is an object
        // and sneaking due to movement.
    bool movingObject;

    IPowerable copiedPower;

    void Awake()
    {
        movingObject = false;
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
        // factory method??
        PowersSuperClass instanceOfPower = copiedPower.InstantiatePower();
        //instanceOfPower = ;

        //if (copiedPower != null)
        //instanceOfPower = copiedPower.InstantiatePower();
        //instanceOfPower.ActivatePower1();
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

        // in case player is an object and activates power while sneaking.
        movingObject = false;

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
                        copiedPower = testCopyable.GetPower();
                    }
                }
            }
        }

        if (minDist > 5f)
        {
            closestMesh = baseMesh;
            meshStimulusOrigin = Stimulus.origin.Patient;
            copiedPower = null;
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

    public PowersSuperClass InstantiatePower()
    {
        return this;
    }
}
