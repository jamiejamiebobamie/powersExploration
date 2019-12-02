using UnityEngine;
public class Aspect : MonoBehaviour
{

    public enum aspect
    {
        Patient, // if movingObject == player
        Guard, // if copying Guard form
        Sneaking, // only works for hearing
        Object,
        Incapacitated,
        //Object, // if not moving
        // if you're an object, you're sneaking.
        // you're not thinking aout this...
    }

    [SerializeField] private aspect currentAspect;
    //private aspect aspectNameStored;

    public void SetCurrentAspect(aspect value)
    {
        currentAspect = value;
    }

    public aspect GetCurrentAspect()
    {
        return currentAspect;
    }



}
