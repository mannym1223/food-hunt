using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float cameraSpeed;

    private float mouseX;
    //private float mouseY; //to rotate vertically

	private void Start()
	{
        //don't show mouse cursor on screen
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
    {
        //get player horizontal mouse input
        mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(0, cameraSpeed * Time.deltaTime * mouseX, 0);
    }
}
