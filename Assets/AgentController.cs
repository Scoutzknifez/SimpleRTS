using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    [Header("Animations")]
    public bool hasAnimations = false;
    public Animator animator;

    [Header("Agent")]
    public NavMeshAgent agent;

    [Header("Unit Selection")]
    public GameObject selectedCircle;
    public bool isSelected;

    private void Start()
    {
        GameManager.instance.units.Add(this);
    }

    private void Update()
    {
        CheckSelected();
        DoAnimations();
        HandleSpace();
    }

    void CheckSelected()
    {
        selectedCircle.SetActive(isSelected);
    }

    void DoAnimations()
    {
        if (!hasAnimations)
        {
            return;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        bool isShooting = isSelected && Input.GetKey(KeyCode.Space) && agent.velocity.magnitude < .01;
        animator.SetBool("Shooting", isShooting);
    }

    public void MoveToward(Vector3 position)
    {
        AutoGather ag = GetComponent<AutoGather>();

        if (ag != null)
        {
            // A Gatherer
            ag.busy = true;
            ag.state = GathererState.CUSTOM;
        }

        agent.SetDestination(position);
    }

    void HandleSpace()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            AutoGather ag = GetComponent<AutoGather>();

            if (ag != null)
            {
                ag.busy = false;
            }
        }
    }
}
