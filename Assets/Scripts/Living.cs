using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Living : MonoBehaviour
{
    [Header("Living Properties")]
    public float maxHealth = 1f;
    public float health = 1f;

    [Header("Team")]
    public TeamEnum teamEnum;
    private Team team;

    [Header("Unit Selection")]
    public GameObject selectedCircle;
    public TMP_Text textCircle;
    public bool isSelected;

    public void Awake()
    {
        health = maxHealth;
    }

    public void Start()
    {
        SetTeam(Team.GetTeam((int)teamEnum));
        GameManager.instance.units.Add(this);
    }

    // Update is called once per frame
    public void Update()
    {
        CheckSelected();
    }

    public void UpdateTeamColors()
    {
        Color teamColor = team.GetColor();
        textCircle.color = teamColor;
        textCircle.faceColor = teamColor;
    }

    void CheckSelected()
    {
        selectedCircle.SetActive(isSelected);
    }

    public void SetTeam(TeamEnum _teamEnum)
    {
        teamEnum = _teamEnum;
        SetTeam(Team.GetTeam((int)_teamEnum));
    }

    public void SetTeam(Team _team)
    {
        team = _team;
        UpdateTeamColors();
    }

    public Team GetTeam()
    {
        return team;
    }

    public bool OnSameTeamAs(Unit unit)
    {
        return teamEnum == unit.teamEnum;
    }

    public bool isDead()
    {
        return health <= 0f;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        health = 0f;

        GameManager.instance.units.Remove(this);
        ControlEntities.instance.selectedUnits.Remove(this);
        Destroy(gameObject);

        // TODO Play animation of death here
    }
}
