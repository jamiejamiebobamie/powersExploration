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

    [SerializeField] private origin currentOrigin;

    public void SetCurrentOrigin(origin value)
    {
        currentOrigin = value;
    }

    public origin GetCurrentOrigin()
    {
        return currentOrigin;
    }

}
