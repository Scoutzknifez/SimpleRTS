using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : MonoBehaviour
{
    public float maxHealth = 1f;
    public float health = 1f;
    public TeamEnum teamEnum;
    private Team team;

    [Header("Nav Agent")]
    public AgentController ac;
    public bool hasCustomJob = false;

    [Header("Unit Selection")]
    public GameObject selectedCircle;
    public TMP_Text textCircle;
    public bool isSelected;

    public void Awake()
    {
        team = Team.GetTeam((int)teamEnum);
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        Color teamColor = team.GetColor();
        textCircle.color = teamColor;
        textCircle.faceColor = teamColor;

        GameManager.instance.units.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSelected();
        ReleaseUnitOnSpace();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public bool isDead()
    {
        return health <= 0f;
    }

    public void Die()
    {
        health = 0f;

        GameManager.instance.units.Remove(this);
        ControlEntities.instance.selectedUnits.Remove(this);
        Destroy(gameObject);

        // TODO Play animation of death here
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

    void CheckSelected()
    {
        selectedCircle.SetActive(isSelected);
    }

    public void LookAt(Vector3 position)
    {
        ac.LookAt(position);
    }

    public void MoveTo(Vector3 position)
    {
        ac.MoveToward(position);
    }

    public bool OnSameTeamAs(Unit unit)
    {
        return teamEnum == unit.teamEnum;
    }

    void ReleaseUnitOnSpace()
    {
        if (Input.GetKey(KeyCode.Space) && isSelected)
        {
            Release();
        }
    }
}