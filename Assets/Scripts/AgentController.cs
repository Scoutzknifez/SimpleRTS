using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AgentController : MonoBehaviour
{
    [Header("Animations")]
    public bool hasAnimations = false;
    public Animator animator;
    public bool isAttacking = false;

    [Header("Agent")]
    public NavMeshAgent agent;

    private void Update()
    {
        DoAnimations();
    }

    void DoAnimations()
    {
        if (!hasAnimations)
        {
            return;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        bool isShooting = isAttacking && agent.velocity.magnitude < .01;
        animator.SetBool("Shooting", isShooting);

        StartCoroutine(StopShooting());
    }

    IEnumerator StopShooting()
    {
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
    }

    public void LookAt(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
    }

    public void MoveToward(Vector3 position)
    {
        /*
        AutoGather ag = GetComponent<AutoGather>();

        if (ag != null)
        {
            // A Gatherer
            ag.busy = true;
            ag.state = GathererState.CUSTOM;
        }
        */
        agent.SetDestination(position);
    }
}
