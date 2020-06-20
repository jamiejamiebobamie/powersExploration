using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranquilizerDartProjectile : MonoBehaviour
{
    [SerializeField] private GameObject staticDart;

    void OnCollisionEnter(Collision collision)
    {
        Vector3 point = collision.GetContact(0).point;
        GameObject sd = Instantiate(staticDart, point, gameObject.transform.rotation);
        Humanoid test_H = collision.transform.gameObject.GetComponent<Humanoid>();
        TelekinesisPower test_TP = collision.transform.gameObject.GetComponent<TelekinesisPower>();
        // check to see if humanoid
        if (test_H)
        {
            // check to see if the player has the telekinsis power
            if (test_TP){
                // check to see if the player is blocking with the telekinesis power
                    // and that he has something to block with.
                if (test_TP.GetIsBlocking() && test_TP.GetHasThrowables()){
                    test_TP.BlockedAttack(collision.relativeVelocity);
                }
                // if not blocking, perform normally.
                else
                {
                    sd.transform.SetParent(collision.transform);
                    test_H.IncrementDartCount();
                }
            }
            // if no telekinesis power, perform normally.
            else
            {
                sd.transform.SetParent(collision.transform);
                test_H.IncrementDartCount();
            }
        }
        else
        {
            // scene objects that collide with tranq darts are no longer scene
        //     IThrowable test_ITh = collision.transform.gameObject.GetComponent<IThrowable>();
        //     if (test_ITh != null)
        //     {
        //         test_ITh.setIsThrowable(false);
        //     }
        // }
    }
    Destroy(gameObject);
}
}
