using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed;
    public Vector3 cameraOffset;
    public float collisionPadding;
    public Vector3 angleOffset;
    [Tooltip("The damping factor to smooth the changes in position and rotation of the camera.")]
    public float damping;
    public float minPitch;
    public float maxPitch;

    private RaycastHit hit;
    public LayerMask mask;

    private Vector3 currentOffset;
    private float offsetScale = 1f;
    private float angleX = 0.0f;
    private float maxMagnitude;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentOffset = cameraOffset;
        maxMagnitude = cameraOffset.magnitude;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(player.transform.position, currentOffset, out hit, maxMagnitude, mask))
        {
            Debug.DrawLine(player.transform.position, hit.point, Color.red);
            //only change magnitude of offset between player and camera
            offsetScale = (hit.point - player.transform.position).magnitude / (currentOffset.magnitude + collisionPadding);
        }
        else
        {
            Debug.DrawLine(player.transform.position, transform.position, Color.yellow);
            offsetScale = 1f;
        }
    }

    private void LateUpdate()
    {
        float mx, my;
        mx = Input.GetAxis("Mouse X");
        my = Input.GetAxis("Mouse Y");

        // We apply the initial rotation to the camera.
        Quaternion initialRotation = Quaternion.Euler(angleOffset);

        Vector3 eu = transform.rotation.eulerAngles;

        angleX -= my * cameraSpeed;

        // We clamp the angle along the X axis to be between the min and max pitch.
        angleX = Mathf.Clamp(angleX, minPitch, maxPitch);

        eu.y += mx * cameraSpeed;
        Quaternion newRot = Quaternion.Euler(angleX, eu.y, 0.0f) * initialRotation;

        transform.rotation = newRot;

        Vector3 forward = transform.rotation * Vector3.forward * cameraOffset.z * offsetScale;
        Vector3 right = transform.rotation * Vector3.right * cameraOffset.x * offsetScale;
        Vector3 up = transform.rotation * Vector3.up * cameraOffset.y * offsetScale;

        Vector3 desiredPosition = player.transform.position
            + forward + right + up;

        desiredPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        
        currentOffset = desiredPosition - player.transform.position;
        transform.position = desiredPosition;
    }
}
