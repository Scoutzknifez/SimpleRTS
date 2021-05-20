using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public LayerMask DebugLayer;
    public Camera cam;
    
    [HideInInspector]
    public NameBuildingMapping attemptingToPlace = null;
    [HideInInspector]
    public GameObject onCursor;
    public Shader highlightShader;
    public NameBuildingMapping[] buildingMenu;

    // Update is called once per frame
    void Update()
    {
        if (attemptingToPlace == null || attemptingToPlace.buildingPrefab == null) {
            onCursor = null;
            return;
        }

        ShowPlacable();
        HandleClick();
    }

    void ShowPlacable()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, ~DebugLayer))
        {
            onCursor.transform.position = hit.point;
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttemptToPlace();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // Cancel placement
            CleanCursor();
        }
    }

    void CleanCursor()
    {
        Destroy(onCursor);
        attemptingToPlace = null;
    }

    public void SetPlaceableOnCursor(string name)
    {
        attemptingToPlace = Array.Find(buildingMenu, mapping => mapping.buildingName == name);
        onCursor = Instantiate(attemptingToPlace.buildingPrefab);
        SetShader(highlightShader, onCursor.transform.GetChild(0).gameObject);
    }

    public void AttemptToPlace()
    {
        if (!GameManager.instance.TryTakeMoney(attemptingToPlace.cost))
        {
            CleanCursor();
            return;
        }

        Placeable placeable = onCursor.GetComponent<Placeable>();
        TeamEnum team = GameManager.instance.currentTeam;

        if (placeable.GetType() == typeof(Building))
        {
            Building building = (Building)placeable;
            building.Place(team);
        } else
        {
            placeable.Place(team);
        }



        SetShader("Standard", onCursor.transform.GetChild(0).gameObject);
        attemptingToPlace = null;
    }

    void SetShader(string name, GameObject go)
    {
        SetShader(Shader.Find(name), go);
    }

    void SetShader(Shader shader, GameObject go)
    {
        foreach (Material mat in go.GetComponent<Renderer>().materials)
        {
            mat.shader = shader;
        }
    }
}

[System.Serializable]
public class NameBuildingMapping
{
    public string buildingName;
    public int cost;
    public GameObject buildingPrefab;
}