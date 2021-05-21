using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TeamEnum currentTeam = TeamEnum.GREEN;
    public int startingMoney = 2500;
    [HideInInspector]
    public int money = 2000;

    [HideInInspector]
    public List<Living> units = new List<Living>();
    public List<ResourcePoint> resourcePoints = new List<ResourcePoint>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        money = startingMoney;
    }

    public bool TryTakeMoney(int amount)
    {
        if (!HasAmount(amount))
        {
            return false;
        }

        money -= amount;
        return true;
    }

    public bool HasAmount(int amount)
    {
        return money >= amount;
    }
}

public class Team
{
    public static Team BLUE = new Team(new Color(0, 0, 1));
    public static Team GREEN = new Team(new Color(0, 1, 0));
    public static Team RED = new Team(new Color(1, 0, 0));

    private Color color;

    private Team(Color _color)
    {
        color = _color;
    }

    public Color GetColor()
    {
        return color;
    }

    public static Team GetTeam(int id)
    {
        if (id == 0) return BLUE;
        else if (id == 1) return GREEN;
        else return RED;
    }
}

public enum TeamEnum
{
    BLUE,
    GREEN,
    RED
}