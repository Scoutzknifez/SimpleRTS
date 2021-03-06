using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlEntities : MonoBehaviour
{
    public static ControlEntities instance;

    [Header("Group Selection Box")]
    public RectTransform groupSelectionBox;
    public Camera cam;
    public Vector2 startGroupSelectionPosition;

    [Space]
    public List<Living> selectedUnits = new List<Living>();

    private static int[] positionsPerRing = new int[] { 6, 14, 24 };
    private static int maxPositions = 1 + positionsPerRing[0] + positionsPerRing[1] + positionsPerRing[2];

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

    // Update is called once per frame
    void Update()
    {
        SelectUnits();
        MoveUnits();
    }

    void SelectUnit(Living living)
    {
        if (selectedUnits.Count < maxPositions)
        {
            selectedUnits.Add(living);
            living.isSelected = true;
        }
    }

    void SelectUnits()
    {
        // Single Unit Selection
        if (Input.GetMouseButtonDown(0))
        {
            ClearSelectedUnits();
            startGroupSelectionPosition = Input.mousePosition;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Living living = hit.collider.GetComponent<Living>();
                if (living != null)
                {
                    SelectUnit(living);
                }
            }
        }

        // Group Unit Selection Start
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }

        // Group Unit Selection End
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }
    }

    void UpdateSelectionBox(Vector2 currentMousePosition)
    {
        if (!groupSelectionBox.gameObject.activeInHierarchy)
        {
            groupSelectionBox.gameObject.SetActive(true);
        }

        float x = currentMousePosition.x - startGroupSelectionPosition.x;
        float y = currentMousePosition.y - startGroupSelectionPosition.y;

        groupSelectionBox.sizeDelta = new Vector2(Mathf.Abs(x), Mathf.Abs(y));
        groupSelectionBox.anchoredPosition = startGroupSelectionPosition + new Vector2(x / 2, y / 2);
    }

    void ReleaseSelectionBox()
    {
        groupSelectionBox.gameObject.SetActive(false);

        Vector2 min = groupSelectionBox.anchoredPosition - (groupSelectionBox.sizeDelta / 2);
        Vector2 max = groupSelectionBox.anchoredPosition + (groupSelectionBox.sizeDelta / 2);

        foreach (Living living in GameManager.instance.units)
        {
            Vector3 screenPosOfAC = cam.WorldToScreenPoint(living.transform.position);

            if (screenPosOfAC.x > min.x && screenPosOfAC.x < max.x && screenPosOfAC.y > min.y && screenPosOfAC.y < max.y)
            {
                SelectUnit(living);
            }
        }
    }

    void ClearSelectedUnits()
    {
        foreach (Living living in selectedUnits)
        {
            living.isSelected = false;
        }

        selectedUnits.Clear();
    }

    void MoveUnits()
    {
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count != 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                int unitCount = selectedUnits.Count;
                if (unitCount == 1)
                {
                    if (!(selectedUnits[0] is Unit))
                    {
                        return;
                    }

                    Unit unit = (Unit) selectedUnits[0];

                    unit.MoveTo(hit.point);
                    unit.hasCustomJob = true;

                    Fighter fighter = GetComponent<Fighter>();

                    if (fighter != null)
                    {
                        fighter.state = FighterState.GOING;
                    }
                }
                else
                {
                    List<Vector3> positions = GetPositionsAroundArea(hit.point, new float[] { 2.5f, 5f, 7.5f }, positionsPerRing);

                    int index = 0;
                    foreach (Living living in selectedUnits)
                    {
                        if (!(living is Unit))
                        {
                            continue;
                        }

                        Unit unit = (Unit) living;

                        unit.MoveTo(positions[index++]);
                        unit.hasCustomJob = true;

                        if (index >= maxPositions)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    List<Vector3> GetPositionsAroundArea(Vector3 rootPosition, float[] distance, int[] count)
    {
        List<Vector3> positions = new List<Vector3>();
        positions.Add(rootPosition);

        for (int i = 0; i < distance.Length; i++)
        {
            positions.AddRange(GetPositionsAroundArea(rootPosition, distance[i], count[i]));
        }

        return positions;
    }

    List<Vector3> GetPositionsAroundArea(Vector3 rootPosition, float distance, int count)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            float angleFromRoot = 360f * ((float)i / count);
            Vector3 direction = ApplyAngleToVector(new Vector3(1, 0), angleFromRoot);

            Vector3 uniquePosition = rootPosition + (direction * distance);
            positions.Add(uniquePosition);
        }

        return positions;
    }

    Vector3 ApplyAngleToVector(Vector3 forward, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * forward;
    }
}
