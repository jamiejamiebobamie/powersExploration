using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatPower : MonoBehaviour, IPowerable
{
    System.Random random = new System.Random();

    private GameObject[] sceneObjects;
    private Dictionary<int, HashSet<Mesh>> Copyables =
        new Dictionary<int, HashSet<Mesh>>();

    private Mesh baseMesh;
    public Aspect aspect;

    RaycastHit hitForward, hitLeft, hitRight, hitBackward, hitUp, hitDown;

    public bool IsCopying
    {
        get
        {
            return isCopying;
        }
        set
        {
            isCopying = value;
            if (!isCopying)
                gameObject.GetComponent<MeshFilter>().mesh = baseMesh;
        }
    }

    bool isWallWalking, isCopying;
    Vector3 upNormalFromPower;
    float elapsedTime;

    int HitDistance{ get
        {
            return 5;
        }
    }

    [SerializeField] Rigidbody rb;
    Quaternion playerStartRotation;
    bool playerNormallyRotated = false;

    void Start()
    {
        baseMesh = gameObject.GetComponent<MeshFilter>().mesh;
        sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in sceneObjects)
        {
            ICopyable copyable = obj.GetComponent<ICopyable>();
            if (copyable != null)
            {
                int height = (int)copyable.GetPosition().y;
                Mesh sceneObjectsMesh = copyable.GetMesh();

                HashSet<Mesh> setOfMeshesAtGivenHeight;

                HashSet<Mesh> newSet = new HashSet<Mesh>();
                // see if that height has a given mesh in the list
                // if it does add the mesh to the list.
                if (Copyables.TryGetValue(height, out setOfMeshesAtGivenHeight))
                {
                    setOfMeshesAtGivenHeight.Add(sceneObjectsMesh);
                }
                // if it does not insert the list with the single mesh
                else
                {
                    newSet.Add(sceneObjectsMesh);
                    Copyables.Add(height, newSet);
                    Debug.Log(height);
                }
            }
        }


        elapsedTime = 0.0f;

        playerStartRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        upNormalFromPower = Vector3.up;
    }

    public void ActivatePower1()
    {
        WallWalk();
    }

    public void ActivatePower2()
    {
        Copy();
    }

    private void Copy()
    {
        Mesh form = baseMesh;

        if (isCopying)
        {
            isCopying = false;
            aspect.aspectName = Aspect.aspect.Player;
        }
        else
        {
            isCopying = true;

            float roundedHeight = Mathf.Round(transform.position.y);
            int height = (int)roundedHeight;

            List<Mesh> returnedMeshes = new List<Mesh>();

            for (int h = height - 5; h < height + 5; h++)
            {
                HashSet<Mesh> testMeshSetPerHeight;
                if (Copyables.TryGetValue(h, out testMeshSetPerHeight))
                {
                    testMeshSetPerHeight = Copyables[h];
                    foreach (Mesh m in testMeshSetPerHeight)
                    {
                        returnedMeshes.Add(m);
                    }
                }
            }

            if (returnedMeshes.Count > 0)
            {
                aspect.aspectName = Aspect.aspect.Object;
                int index = random.Next(0, returnedMeshes.Count);
                form = returnedMeshes[index];
            }
        }

        GetComponent<MeshFilter>().mesh = form;
    }

    void WallWalk()
    {
        if (isWallWalking)
        {
            isWallWalking = false;
            rb.useGravity = true;
        }
        else
        {
            isWallWalking = true;
            rb.useGravity = false;
            playerNormallyRotated = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isWallWalking)
        { // coroutine
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 1.0f)
            {
                upNormalFromPower = RayCastCardinalDirections();
            }

            Quaternion playerRotationFromPower = Quaternion.FromToRotation(Vector3.up, upNormalFromPower);

            if (transform.up != upNormalFromPower)
                transform.rotation = Quaternion.Lerp(transform.rotation, playerRotationFromPower, .1f);
        }
        else
        {
            if (!playerNormallyRotated)
            {
                if (transform.rotation != Quaternion.identity)
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.05f);
                else
                    playerNormallyRotated = true;
            }
        }

        if (IsCopying && rb.velocity.magnitude > 1f)
                IsCopying = false;
    }

    Vector3 RayCastCardinalDirections()
    {
        int count = 0;
        upNormalFromPower = Vector3.zero;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward, HitDistance))
        {
            upNormalFromPower += hitForward.normal;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward, HitDistance))
        {
            upNormalFromPower += hitForward.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft, HitDistance))
        {
            upNormalFromPower += hitLeft.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight, HitDistance))
        {
            upNormalFromPower += hitRight.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitBackward, HitDistance))
        {
            upNormalFromPower += hitBackward.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitUp, HitDistance))
        {
            upNormalFromPower += hitUp.normal;
            count++;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown, HitDistance))
        {
            upNormalFromPower += hitDown.normal;
            count++;
        }

        if (count == 0)
        {
            isWallWalking = false;
            rb.useGravity = true;
            return Vector3.up;
        }

        upNormalFromPower /= count;
        upNormalFromPower = Vector3.Normalize(upNormalFromPower);

        if (float.IsNaN(upNormalFromPower.x))
            upNormalFromPower = Vector3.up;

        return upNormalFromPower;
    }
}