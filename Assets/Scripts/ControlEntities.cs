using UnityEngine;
using UnityEngine.AI;

public class ControlEntities : MonoBehaviour
{
    public GameObject selectedEntity;
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                NavMeshAgent agent = selectedEntity.GetComponent<NavMeshAgent>();
                agent.SetDestination(hit.point);
            }
        }
    }
}
