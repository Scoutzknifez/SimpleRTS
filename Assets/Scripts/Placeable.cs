using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : Living
{
    public bool placed = false;

    public new void Start()
    {
        // Used to override the usual behavior in Unit.cs
    }

    public void Place(TeamEnum teamEnum)
    {
        SetTeam(teamEnum);
        placed = true;
        GameManager.instance.units.Add(this);
    }
}
