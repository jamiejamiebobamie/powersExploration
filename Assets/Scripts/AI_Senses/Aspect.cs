using UnityEngine;
public class Aspect : MonoBehaviour
{

    public enum aspect
    {
        Player, // if movingObject == player
        Enemy, // if copying Guard form
        Sneaking, // only works for hearing
        Object,
        //Object, // if not moving
        // if you're an object, you're sneaking.
        // you're not thinking aout this...
        
    }

    public aspect aspectName;
    private aspect aspectNameStored;
}
