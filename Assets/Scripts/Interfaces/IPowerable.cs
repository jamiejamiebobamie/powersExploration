using UnityEngine;
using System.Collections;

public interface IPowerable
{
    void ActivatePower1();
    void ActivatePower2();
    PowersSuperClass InstantiatePower();
}
