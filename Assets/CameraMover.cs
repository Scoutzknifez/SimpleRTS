using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float cameraMoveSpeed = 25f;
    public float rotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        DoMovement();
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
}
