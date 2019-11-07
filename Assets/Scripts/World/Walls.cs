using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public enum Direction
    {
        North,
        South,
        East,
        West,
        Floor,
        Cieling,
    }

    public Direction wallDirection;

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 test = new Vector3(0,0,0);

        // this isn't working. i don't know how Quaternions work...
        // why would it not be constant?

        // the hit normal is constant

        Quaternion rot = transform.rotation;

        Debug.Log(rot);

        if (rot == new Quaternion(0.5f, -0.5f, -0.5f, 0.5f))
        {
            wallDirection = Direction.West; // (0.5f, -0.5f, -0.5f, 0.5f)
        }
        else if (rot == new Quaternion(0.0f, 0.0f, -0.7f, 0.7f))
        {
            wallDirection = Direction.North; // (0.0f, 0.0f, -0.7f, 0.7f)
        }
        //else if (rot == new Vector3(180f, 0f, 0f))
        //{
        //    wallDirection = Direction.South;
        //}
        //else if (rot == new Vector3(-90f, 0f, 0f))
        //{
        //    wallDirection = Direction.Cieling;
        //}
        else if (rot == new Quaternion(0.7f, 0.0f, 0.0f, 0.7f))
        {
            wallDirection = Direction.Floor; // (0.7f, 0.0f, 0.0f, 0.7f)
        }

        Debug.Log(wallDirection);

    }

    void SetAlignment()
    {
        Debug.Log("hi");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
