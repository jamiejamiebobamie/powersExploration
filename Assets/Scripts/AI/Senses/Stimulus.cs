using UnityEngine;
public class Stimulus : MonoBehaviour
{
    // the senses sense stimuli from an origin.
    public enum origin
    {
        Patient,
        Guard,
        Sneaking,
        Object,
        Incapacitated,
    }

    [SerializeField] private origin stimulusSource;

    public void SetCurrentOrigin(origin value)
    {
        stimulusSource = value;
    }

    public origin GetCurrentOrigin()
    {
        return stimulusSource;
    }

}
