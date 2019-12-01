using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBurnable
{
    void Burns();
    bool GetIncapacitated();
    void SetIncapacitated(bool value);
}
