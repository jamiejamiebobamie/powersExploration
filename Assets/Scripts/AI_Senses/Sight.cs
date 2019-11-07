using UnityEngine;
public class Sight : Sense
{

    public int FieldOfView = 60;
    public int ViewDistance = 100;
    private Transform playerTrans;
    private Vector3 rayDirection;

    protected override void Initialize()
    {
        //Find player position
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        // Detect perspective sense if within the detection rate
        if (elapsedTime >= detectionRate) DetectAspect();

    }

    //Detect perspective field of view for the AI Character
    void DetectAspect()
    {
        RaycastHit hit;

        //Direction from current position to player position
        rayDirection = playerTrans.position - transform.position;

        //Check the angle between the AI character's forward
        //vector and the direction vector between player and AI
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection,
                out hit, ViewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                if (aspect != null)
                {
                    //Check the aspect. Ignore “Sneaking” by including it in the acceptable aspects.
                    if (aspect.aspectName == aspectName || aspect.aspectName == Aspect.aspect.Sneaking)
                    {
                        //if (player.GetComponent<CapsuleController>().moving == true && player.GetComponent<copycat_script>().humanoidForm != true)
                          print("Player Seen");
                    }
                }
            }
        }
    }
}
