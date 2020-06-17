using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranquilizerDartProjectile : MonoBehaviour
{
    [SerializeField] private GameObject staticDart;

    void OnCollisionEnter(Collision collision)
    {
        Vector3 point = collision.GetContact(0).point;
        Instantiate(staticDart, point, gameObject.transform.rotation);
        Humanoid test_H = collision.transform.gameObject.GetComponent<Humanoid>();
        if (test_H)
        {
            test_H.IncrementDartCount();
        }
        Destroy(gameObject);
    }
}
