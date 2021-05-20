using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Placeable
{
    public Queue<Unit> toBeProduced = new Queue<Unit>();

    public GameObject[] toSpawnOnPlace;
    public Transform spawnArea;

    public new void Place(TeamEnum teamEnum)
    {
        base.Place(teamEnum);
        PlaceSpawnables();
    }

    void PlaceSpawnables()
    {
        foreach (GameObject spawnObject in toSpawnOnPlace)
        {
            Instantiate(spawnObject, spawnArea.position, spawnObject.transform.rotation);
        }
    }
}
