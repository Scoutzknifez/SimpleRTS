using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public LayerMask DebugLayer;
    public Camera cam;

    public GameObject attemptingToPlace = null;
    public Shader highlightShader;
    public NameBuildingMapping[] buildingMenu;

    // Update is called once per frame
    void Update()
    {
        if (attemptingToPlace == null) {
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
            attemptingToPlace.transform.position = hit.point;
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttemptToPlace();
        }
    }

    public void SetPlaceableOnCursor(string name)
    {
        NameBuildingMapping mapping = Array.Find(buildingMenu, mapping => mapping.buildingName == name);
        attemptingToPlace = Instantiate(mapping.buildingPrefab);

        SetShader(highlightShader, attemptingToPlace.transform.GetChild(0).gameObject);
    }

    public void AttemptToPlace()
    {
        SetShader("Standard", attemptingToPlace.transform.GetChild(0).gameObject);
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
    public GameObject buildingPrefab;
}