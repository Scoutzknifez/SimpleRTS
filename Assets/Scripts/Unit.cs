using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : Living
{
    [Header("Nav Agent")]
    public AgentController ac;
    public bool hasCustomJob = false;

    public new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        ReleaseUnitOnSpace();
    }

    public void Release()
    {
        hasCustomJob = false;
        GetAgent().isStopped = false;

        Gatherer gatherer = GetComponent<Gatherer>();
        if (gatherer != null)
        {
            gatherer.busy = false;
        }
    }

    public void Stop()
    {
        GetAgent().isStopped = true;

        Gatherer gatherer = GetComponent<Gatherer>();
        if (gatherer != null)
        {
            gatherer.busy = true;
        }
    }

    public NavMeshAgent GetAgent()
    {
        return ac.agent;
    }

    public void LookAt(Vector3 position)
    {
        ac.LookAt(position);
    }

    public void MoveTo(Vector3 position)
    {
        ac.MoveToward(position);
    }

    void ReleaseUnitOnSpace()
    {
        if (Input.GetKey(KeyCode.Space) && isSelected)
        {
            Release();
        }
    }
}