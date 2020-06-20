using UnityEngine;
public class Sense : MonoBehaviour
{
    // this value represents the type of origin the entity is searching for.
    // as there might be several types of origins of Stimuli that the entity
    // is searching for, this needs to be refactored.
    protected Stimulus.origin desiredStimulusOrigin = Stimulus.origin.Patient;

    public float detectionRate = 1.0f;
    protected float elapsedTime = 0.0f;

    protected virtual void Initialize() { }
    protected virtual void UpdateSense() { }

    void Start()
    {
        elapsedTime = 0.0f;
        Initialize();
    }

    void Update()
    {
        UpdateSense();
    }
}
