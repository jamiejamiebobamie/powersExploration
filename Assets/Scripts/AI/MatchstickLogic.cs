using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchstickLogic : MonoBehaviour
{

    //private List<Humanoid> humans;
    //private Humanoid hunted;

    //void Start()
    //{
    //    hunted = null;
    //    private Humanoid[] humanoids = FindObjectsOfType<Humanoid>();

    //    foreach (Humanoid humanoid in humanoids)
    //    {
    //        // don't hunt yourself
    //        if (humanoid != this)
    //        {
    //            humans.Add(humanoid);
    //        }
    //    }
    //}

    //void Update()
    //{
    //    if (humans.Count > 0)
    //    {
    //        if (hunted == null)
    //        {
    //            hunted = humans[0]; // testing
    //        }

    //        RemoveIncapacitatedHumans();
    //        Hunt();
    //    }

    //}

    //private void RemoveIncapacitatedPlayers()
    //{
    //    for (int i = 0; i < humans.Count; i++)
    //    {
    //        if (humans[i].getIncapacitated())
    //        {
    //            if (hunted == humans[i])
    //            {
    //                hunted = null;
    //            }
    //            humans[i] = null;
    //        }

    //        Debug.Log(humans[i].name);

    //    }
    //}

    //private void Hunt()
    //{
    //    // Set up quaternion for rotation toward destination
    //    Quaternion tarRot = Quaternion.LookRotation(hunted.transform.position
    //                                                    - transform.position);

    //    // Update rotation and translation
    //    transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
    //        2.0f * Time.deltaTime);

    //    transform.Translate(new Vector3(0, 0, 1.0f * Time.deltaTime));
    //}




}
