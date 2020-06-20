using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    // only one.
    Humanoid player;

    [SerializeField] private Humanoid self;
    [SerializeField] private Power power;
    [SerializeField] private float powerOneDelay;
    [SerializeField] private float powerTwoDelay;
    [SerializeField] private float attackDistance;
    private bool isFiring;

    // reference to senses scripts
    [SerializeField] private Sight sight;
    [SerializeField] private Touch touch;
    [SerializeField] private Hearing hearing;
    private bool targetSeen;
    private Humanoid target;
    private List<Humanoid> targets = new List<Humanoid>();
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Humanoid>();

        targetSeen = false;
        isFiring = false;
        Object[] potentialTargets =
            Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (object obj in potentialTargets)
        {
            GameObject go = (GameObject)obj;
            Humanoid test_H = go.GetComponent<Humanoid>();
            if (test_H != null)
            {
                // object instance comparison wasn't working.
                    // object scene names are unique.
                if (test_H.name != self.name)
                {
                    Stimulus.origin test_S = test_H.GetOriginOfStimulus();

                    if (test_S == sight.getDesiredTarget())// && !targets.Contains(test_H))
                    {
                        targets.Add(test_H);
                    }
                }
            }
        }
        target = ChooseNewTarget();
        Debug.Log(target);
    }
    void Update()
    {
        // Debug.Log(target);
        // if not dead.
        if (!self.GetIncapacitated())
        {
            // if target is dead.
                // choose a new target.
            if (target.GetIncapacitated())
            {
                target = ChooseNewTarget();
                sight.resetSeen();
            }
            // if you can't sense the target.
            if (!sight.getTargetSeen()
                && !hearing.getPatientHeard()
                && !touch.getPatientTouched())
            {
                // walk casually to the player.
                    // just for testing...
                Vector3 goHere = player.GetPosition();
                Wander(goHere);
                // if firing. stop.
                if (isFiring)
                {
                    StopCoroutine("RayCastToTarget");
                    isFiring = false;
                }
            }
            // if you can sense the target.
            else if (sight.getTargetSeen())
            {
                Vector3 playerLocation = sight.getSeenTargetLocation();
                if (Vector3.Distance(playerLocation,
                    transform.position) <= 15.0f)
                    {
                        Quaternion tarRot = Quaternion.LookRotation(playerLocation
                            - transform.position);

                        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
                            2.0f * Time.deltaTime);
                            playerLocation = transform.position;
                    }
                else
                {
                    Chase(playerLocation);
                }
                if (!isFiring)
                {
                    playerLocation = sight.getSeenTargetLocation();
                    if (Vector3.Distance(playerLocation,
                        transform.position) <= attackDistance)
                        {
                        StartCoroutine("RayCastToTarget");
                        isFiring = true;
                    }
                    else
                    {
                        StopCoroutine("RayCastToTarget");
                    }
            }
        }
        else
        {
            // follow the other senses...
        }
    }
    else
    {
        StopCoroutine("RayCastToTarget");
    }
}
    private void Wander(Vector3 gohere)
    {
        // Set up quaternion for rotation toward destination
        Quaternion tarRot = Quaternion.LookRotation(gohere
            - transform.position);
        // Update rotation and translation
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
            2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 1.0f * Time.deltaTime));
    }

    private void Chase(Vector3 goHere)
    {
        // Set up quaternion for rotation toward destination
        Quaternion tarRot = Quaternion.LookRotation(goHere
            - transform.position);
        // Update rotation and translation
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot,
            2.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, 5.0f * Time.deltaTime));
    }

    private Humanoid ChooseNewTarget()
    {
        float minDistance = Mathf.Infinity;
        Humanoid newTarget = null;
        foreach (Humanoid t in targets)
        {
            if (!t.GetIncapacitated())
            {
                float testDistance
                    = Vector3.Distance(transform.position,
                    t.GetPosition());
                if (testDistance < minDistance)
                {
                    minDistance = testDistance;
                    newTarget = t;
                }
            }
        }
        return newTarget;
    }

    private IEnumerator RayCastToTarget()
    {
        while (true)
        {
            Debug.Log("fire!");
                if (sight.getTargetSeen()){
                    // FireTranquilizerDart();
                    power.ActivatePower1();
                }
                yield return new WaitForSeconds(powerOneDelay);
            }
        }
}

// STATES:
// NOT TARGET -- HUNT LOST TARGET
/*
 * create vector that forms a direction to last sighting
 *
 * create waypoints to patrol from last sighting position
 *
 * use chance to have guards approach and/or shoot scenery around
 * waypoints
 *
 * revert back to NO TARGET -- PATROL once waypoints have been visited.
 *
 */

// NO TARGET -- PATROL
/*
 * raycast 360 degrees around guard and store the position with the
 * longest distance that is not the second to last
 * position visited.
 *
 * move to that position.
 *
 * choose a new position.
 * (go to NO TARGET -- PATROL.)
 *
 */

// FOUND TARGET -- IN RANGE
/*
 * shoot target with Tranquilizer.
 *
 * other logic ... like runaway, play dead, and shoot from cover...
 * seems overly-complicated to implement.
 *
 * go to FOUND TARGET -- OUT OF RANGE state if target moves out of range.
 *
 */

// FOUND TARGET -- OUT OF RANGE
/*
 * move toward target until in range.
 * go to FOUND TARGET -- IN RANGE state.
 *
 */
