using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float delay;

    public DestroySelf(int delay)
    {
        this.delay = delay;
    }

    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, delay);
    }
}
