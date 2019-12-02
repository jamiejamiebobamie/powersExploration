using UnityEngine;
public class Touch : Sense
{
    void OnTriggerEnter(Collider other)
    {
        Stimulus stimulus = other.GetComponent<Stimulus>();
        if (stimulus != null)
        {
            Stimulus.origin currentOrigin = stimulus.GetCurrentOrigin();
            //Check the aspect
            if (currentOrigin == desiredStimulusOrigin
                || currentOrigin == Stimulus.origin.Sneaking)
            {
                print("Touched " + desiredStimulusOrigin + "!");
            }
        }
    }
}
