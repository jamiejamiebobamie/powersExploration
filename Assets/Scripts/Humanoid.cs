using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour, ICopyable, IBurnable, IHittable, IKillable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] GameObject fire;
     // boolean to show humanoid has been hit
    private bool isStaggered ,isIncapacitated, isBurning;
    [SerializeField] private Stimulus stimulus;
    System.Random random = new System.Random();
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        isBurning = false;
        isStaggered = false;
        isIncapacitated = false;
    }
    // ICopyable ---
    public Mesh GetMesh() { return gameObject.GetComponent<MeshFilter>().mesh; }
    public Vector3 GetPosition() { return transform.position; }
    public Stimulus.origin GetOriginOfStimulus() { return stimulus.GetCurrentOrigin();}
    public IPowerable GetPower()
    {
        IPowerable testPower = GetComponent<IPowerable>();
        return testPower;
    }
    // ----
    // IBurnable ---
    public void Burns()
    {
        GameObject fireInstance = Instantiate(fire,
            transform.position, Quaternion.identity);
        fireInstance.transform.parent = gameObject.transform;
        isBurning = true;
        Destroy(fireInstance, 5f);
        isIncapacitated = true;
        // other functionality.
        // instantiate "burned" model after x seconds, etc.
        Destroy(gameObject,5f); // destroy npc.
    }
    public bool GetIsBurning()
    {
        return isBurning;
    }
    // ----
    // IHittable ---
    public void ApplyHitForce(Vector3 hitForce, float hitStrength)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(hitForce * hitStrength * 10f);
        // activate ragdoll
        if (isStaggered)
            isIncapacitated = true;
        else
        {
            int randomBool = random.Next(0, 2);
            if (randomBool > 0)
            {
                // Debug.Log("true");
                isStaggered = true;
            }
        }
    }
    public bool GetIsStaggered() { return isStaggered; }
    public void SetIsStaggered(bool value) { isStaggered = value; }
    // ----
    // IKillable ---
    public void SetIncapacitated(bool value) { isIncapacitated = value; }
    public bool GetIncapacitated() { return isIncapacitated; }
    // ----
}
