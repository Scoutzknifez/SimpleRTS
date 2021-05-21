using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : Unit
{
    public GathererState state = GathererState.IDLE;
    public bool busy = false;

    public Vector3 startLoc;
    public ResourcePoint closestResource;

    public float gatheringDistance = 3f;
    public float gatherTime = 3f;

    public int maxLoad = 5;
    public int load = 0;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        startLoc = transform.position;
        closestResource = FindClosestResourcePoint();
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
                GameManager.instance.money += load;
                load = 0;
            }
        }

        if (!busy)
        {
            EvaluateGoal();
        }
    }

    ResourcePoint FindClosestResourcePoint()
    {
        if (GameManager.instance.resourcePoints.Count == 0)
        {
            return null;
        }

        ResourcePoint closest = GameManager.instance.resourcePoints[0];
        float closestDistance = Vector3.Distance(startLoc, closest.transform.position);

        for (int i = 1; i < GameManager.instance.resourcePoints.Count; i++)
        {
            ResourcePoint currentPoint = GameManager.instance.resourcePoints[i];
            float distance = Vector3.Distance(startLoc, currentPoint.transform.position);

            if (distance < closestDistance)
            {
                closest = currentPoint;
            }
        }

        return closest;
    }

    void EvaluateGoal()
    {
        if (load < maxLoad && state != GathererState.GATHERING)
        {
            state = GathererState.GOING;
            if (closestResource != null)
            {
                MoveTo(closestResource.transform.position);
            }
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