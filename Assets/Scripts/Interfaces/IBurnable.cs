using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface for objects that can be burnt.
public interface IBurnable
{
    void Burns();
    bool GetIsBurning();
}
