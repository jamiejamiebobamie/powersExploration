using UnityEngine;
public class Touch : Sense
{
    void OnTriggerEnter(Collider other)
    {
        Aspect aspect = other.GetComponent<Aspect>();
        if (aspect != null)
        {
            Aspect.aspect currentAspect = aspect.GetCurrentAspect();
            //Check the aspect
            if (currentAspect == aspectName || currentAspect == Aspect.aspect.Sneaking)
            {
                print("Player Touched");
            }
        }
    }
}
