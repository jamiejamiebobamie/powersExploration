using UnityEngine;
public class Stimulus : MonoBehaviour
{
    [SerializeField] private origin stimulusSource;
    // the senses sense stimuli from an origin.
    public enum origin
    {
        Patient,
        Guard,
        Sneaking,
        Object,
        Incapacitated,
    }
    public void SetCurrentOrigin(origin value) { stimulusSource = value; }
    public origin GetCurrentOrigin() { return stimulusSource; }
}
