using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AutoGather : MonoBehaviour
{
    private NavMeshAgent agent;
    public Vector3 startLoc;
    public Vector3 closestResource;

    public float gatheringDistance = 3f;

    public int maxLoad = 5;
    public int load = 0;
    public GathererState state = GathererState.GOING;
    public bool busy = false;
    public float gatherTime = 3f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startLoc = transform.position;
    }

    private void Update()
    {
        if (agent.remainingDistance < gatheringDistance)
        {
            if (state == GathererState.GOING)
            {
                StartGathering();
            }
            else if (state == GathererState.RETURNING)
            {
                load = 0;
            }
        }
    }

    private void FixedUpdate()
    {
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
            agent.SetDestination(closestResource);
        }
        else if (load >= maxLoad)
        {
            state = GathererState.RETURNING;
            agent.SetDestination(startLoc);
        }
    }

    void StartGathering()
    {
        agent.isStopped = true;
        busy = true;
        state = GathererState.GATHERING;
        StartCoroutine(Gather());
    }

    void EndGathering()
    {
        agent.isStopped = false;
        busy = false;
    }

    IEnumerator Gather()
    {
        float loopTime = gatherTime / maxLoad;

        while (load < maxLoad)
        {
            yield return new WaitForSeconds(loopTime);
            load++;
        }

        EndGathering();
    }
}

public enum GathererState
{
    GATHERING,
    RETURNING,
    GOING,
    CUSTOM
}