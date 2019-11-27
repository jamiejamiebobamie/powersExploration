using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatPower_OriginalImplementation : MonoBehaviour, IPowerable
{
    private GameObject[] gameObjects;
    private List<ICopyable> Copyables = new List<ICopyable>();

    private Mesh baseMesh;
    public Aspect aspect;

    private Rigidbody rb;

    // keeps track of if the player is an object and sneaking due to movement.
    bool movingObject, standingPlayer;

    void Start()
    {
        movingObject = false;
        standingPlayer = false;
        rb = GetComponent<Rigidbody>();
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
        Aspect.aspect meshAspect = Aspect.aspect.Player;
        float minDist = Mathf.Infinity;

        foreach (ICopyable copyable in Copyables)
        {
            float testDist = Vector3.Distance(transform.position,
                copyable.GetPosition());

            if (minDist > testDist)
            {
                minDist = testDist;
                closestMesh = copyable.GetMesh();
                meshAspect = copyable.GetAspect();
            }
        }

        if (minDist > 5f)
        {
            closestMesh = baseMesh;
            meshAspect = Aspect.aspect.Player;
        }

        aspect.aspectName = meshAspect;
        GetComponent<MeshFilter>().mesh = closestMesh;
    }

    // if you're not moving and you're human/the player, you're sneaking.
    // if you're moving as an object, you're sneaking.
    // if you're moving and you're human/the player, you're human/the player.
    // if you're moving as a guard, you're an enemy.

    // not working:
    //    if (rb.velocity.magnitude< .00001 && aspect.aspectName == Aspect.aspect.Player)
    //{
    //    aspect.aspectName = Aspect.aspect.Sneaking;
    //}
    //else if (rb.velocity.magnitude > .00001 && aspect.aspectName == Aspect.aspect.Object)
    //{
    //    aspect.aspectName = Aspect.aspect.Sneaking;
    //    movingObject = true; // storing the Object aspect;
    //}
    //else if (rb.velocity.magnitude< .000005 && movingObject)
    //{
    //    aspect.aspectName = Aspect.aspect.Object;
    //    movingObject = false;
    //}
    //else if (rb.velocity.magnitude > .000005 && !movingObject)
    //{
    //    aspect.aspectName = Aspect.aspect.Player;
    //}

    private void Update()
    {
        if (rb.velocity.magnitude > .00001
            && aspect.aspectName == Aspect.aspect.Object)
        {
            aspect.aspectName = Aspect.aspect.Sneaking;
            movingObject = true; // storing the Object aspect;
        }
        else if (rb.velocity.magnitude < .000005 && movingObject)
        {
            aspect.aspectName = Aspect.aspect.Object;
            movingObject = false;
        }
    }
}
