using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchstickAI : MonoBehaviour
{
    private PyrokinesisPower pyrokinesis;

    private List<Humanoid> humans;
    private Humanoid hunted;

    private void Start()
    {
        pyrokinesis = GetComponent<PyrokinesisPower>();
        humans = new List<Humanoid>();
        hunted = null;

        Object[] potentialHumanoids =
            Resources.FindObjectsOfTypeAll(typeof(GameObject));

        foreach (Object obj in potentialHumanoids)
        {
            GameObject gb = (GameObject)obj;
            Humanoid testForHumanoid = gb.GetComponent<Humanoid>();

            // don't hunt yourself 
            if (testForHumanoid != null)
            {
                MatchstickAI testForMatchstickAI = gb.GetComponent<MatchstickAI>();
                if (testForMatchstickAI == null)
                {
                    humans.Add(testForHumanoid);
                    Debug.Log(gb.name);
                }
            }
        }
    }

    private void Update()
    {
        if (humans.Count > 0)
        {
            if (hunted == null)
            {
                hunted = humans[humans.Count-1]; // testing
                 Debug.Log(hunted.name);
            }

            RemoveIncapacitatedPlayers();
            Hunt();
            pyrokinesis.ActivatePower1();
        }
    }

    private void RemoveIncapacitatedPlayers()
    {
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetIncapacitated())
            {
                if (hunted == humans[i])
                {
                    hunted = null;
                }
                    humans.Remove(humans[i]);// = null;
            }
        }
    }

    private void Hunt()
    {
        // Set up quaternion for rotation toward destination
        Quaternion tarRot = Quaternion.LookRotation(hunted.transform.position
                                                        - transform.position);

        // Update rotation and translation
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
            10.0f * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, 1.0f * Time.deltaTime));
    }
}
