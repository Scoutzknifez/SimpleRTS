using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Camera cam;

    public float cameraMoveSpeed = 25f;
    public float cameraZoomSpeed = 5f;
    public float currentCameraZoom = 25f;
    public float maxZoomOut = 100;
    public float minZoomOut = 10;
    public float cameraSmoothing = 5f;
    public float rotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        DoMovement();
        DoZoom();
        DoRotation();
    }

    void DoMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveVector = transform.right * x + transform.forward * z;
        moveVector *= cameraMoveSpeed * Time.deltaTime;

        transform.position += moveVector;
    }

    void DoZoom()
    {
        currentCameraZoom -= Input.mouseScrollDelta.y * cameraZoomSpeed;
        if (currentCameraZoom > maxZoomOut)
        {
            currentCameraZoom = maxZoomOut;
        }
        else if (currentCameraZoom < minZoomOut)
        {
            currentCameraZoom = minZoomOut;
        }

        Vector3 desiredLoc = new Vector3(0, currentCameraZoom, -currentCameraZoom);

        Vector3 smoothedZoom = Vector3.Lerp(cam.transform.localPosition, desiredLoc, Time.deltaTime * cameraSmoothing);

        cam.transform.localPosition = smoothedZoom;
    }

    void DoRotation()
    {
        float rotation = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotation += Time.deltaTime * rotationSpeed;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotation -= Time.deltaTime * rotationSpeed;
        }

        transform.Rotate(0, rotation, 0);
    }

    private void OnValidate()
    {
        DoZoom();
    }
}
