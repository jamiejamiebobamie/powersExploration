using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface for objects that can be "incapacitated".

public interface IKillable
{
    void SetIncapacitated(bool value);
    bool GetIncapacitated();
}
