using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Orbit : MonoBehaviour
{
    public Transform target;

    [Range(-100, 100)]
    public float rotSpeed;

    [Range(1, 50)]
    public float transSpeed;

    // a coroutine would be better here.
    void Update()
    {
        Vector3 relativePos = (target.position + new Vector3(0, 0.5f, 0)) - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * rotSpeed);

        transform.Translate(0, 0, 3 * Time.deltaTime * transSpeed);
    }
}
