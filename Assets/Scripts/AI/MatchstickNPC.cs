using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchstickNPC : Humanoid
{

    private List<Humanoid> humans;
    private Humanoid hunted;

    private Humanoid[] humanoids;

    void Start()
    {
        hunted = null;
        humanoids = FindObjectsOfType<Humanoid>();

        foreach (Humanoid humanoid in humanoids)
        {
            //Debug.Log(obj.name);
            //Humanoid humanoid = obj.GetComponent<Humanoid>();
            //if (humanoid != this)
            //{
                humans.Add(humanoid);
            //}
        }
    }

    void Update()
    {
        if (humans.Count > 0)
        {
            if (hunted == null)
            {
                hunted = humans[0]; // testing
            }

            //RemoveIncapacitatedPlayers();
            Hunt();
        }

    }

    private void RemoveIncapacitatedPlayers()
    {
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].getIncapacitated())
            {
                if (hunted == humans[i])
                {
                    hunted = null;
                }
                humans[i] = null;
            }

            Debug.Log(humans[i].name);

        }
    }

    private void Hunt()
    {
        // Set up quaternion for rotation toward destination
        Quaternion tarRot = Quaternion.LookRotation(hunted.GetLocation()
                                                        - transform.position);

        // Update rotation and translation
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
            2.0f * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, 1.0f * Time.deltaTime));
    }




}
