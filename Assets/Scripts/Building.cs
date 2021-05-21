using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Building : Placeable
{
    public Queue<Unit> toBeProduced = new Queue<Unit>();

    public GameObject[] toSpawnOnPlace;
    public Component[] toEnableOnPlace;
    public Transform spawnArea;

    public new void Place(TeamEnum teamEnum)
    {
        base.Place(teamEnum);
        PlaceSpawnables();
        EnableOnPlace();
    }

    void PlaceSpawnables()
    {
        foreach (GameObject spawnObject in toSpawnOnPlace)
        {
            Instantiate(spawnObject, spawnArea.position, spawnObject.transform.rotation);
        }
    }

    void EnableOnPlace()
    {
        foreach (Component component in toEnableOnPlace)
        {
            if (component is Behaviour)
            {
                Behaviour behaviour = (Behaviour)component;
                behaviour.enabled = true;
            }
            else if (component is Collider)
            {
                Collider collider = (Collider)component;
                collider.enabled = true;
            }
        }
    }
}
