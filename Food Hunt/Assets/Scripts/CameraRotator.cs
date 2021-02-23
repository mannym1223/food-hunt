using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{

    public float cameraSpeed;

    private float mouseDirection;
    private Vector3 mousePosition;

	private void Start()
	{
        mousePosition = Input.mousePosition;
	}

	// Update is called once per frame
	void Update()
    {
        //get player horizontal mouse input
        mouseDirection = Input.mousePosition.x - mousePosition.x;

        transform.Rotate(0, cameraSpeed * Time.deltaTime * mouseDirection, 0);

        mousePosition = Input.mousePosition;
    }
}
