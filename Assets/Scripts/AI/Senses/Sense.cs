using UnityEngine;
public class Sense : MonoBehaviour
{
    public Aspect.aspect aspectName = Aspect.aspect.Patient;

    public float detectionRate = 1.0f;
    protected float elapsedTime = 0.0f;

    protected virtual void Initialize() { }
    protected virtual void UpdateSense() { }

    public GameObject player;

    // Use this for initialization
    void Start()
    {
        elapsedTime = 0.0f;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSense();
    }
}
