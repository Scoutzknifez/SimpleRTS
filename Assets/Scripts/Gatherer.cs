using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : Unit
{
    public Vector3 startLoc;
    public Vector3 closestResource;

    public float gatheringDistance = 3f;

    public int maxLoad = 5;
    public int load = 0;
    public GathererState state = GathererState.IDLE;
    public bool busy = false;
    public float gatherTime = 3f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        startLoc = transform.position;
    }

    private void FixedUpdate()
    {
        if (hasCustomJob)
        {
            return;
        }


        if (GetAgent().hasPath && GetAgent().remainingDistance < gatheringDistance)
        {
            if (state == GathererState.GOING || state == GathererState.IDLE)
            {
                StartGathering();
            }
            else if (state == GathererState.RETURNING)
            {
                load = 0;
            }
        }

        if (!busy)
        {
            EvaluateGoal();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(closestResource, 5f);
    }

    void EvaluateGoal()
    {
        if (load < maxLoad && state != GathererState.GATHERING)
        {
            state = GathererState.GOING;
            MoveTo(closestResource);
        }
        else if (load >= maxLoad)
        {
            state = GathererState.RETURNING;
            MoveTo(startLoc);
        }
    }

    void StartGathering()
    {
        StartCoroutine(Gather());
    }

    IEnumerator Gather()
    {
        Stop();
        state = GathererState.GATHERING;

        float loopTime = gatherTime / maxLoad;

        while (load < maxLoad)
        {
            yield return new WaitForSeconds(loopTime);
            load++;
        }

        EndGathering();
    }

    void EndGathering()
    {
        Release();
    }
}

public enum GathererState
{
    GOING,
    GATHERING,
    RETURNING,
    IDLE,
    CUSTOM
}