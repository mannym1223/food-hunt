using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed;
    public float minDistance;
    [HideInInspector]
    public float maxDistance;
    [Tooltip("Amount of space to put between camera and obstacles when present")]
    public float padding;

    private float mouseX;
    private float mouseY;
    private Vector3 angleOffset;
    private float angleHorizontal = 0f;
    private float angleVertical = 0f;

    private Vector3 currentOffset;
    private RaycastHit hit;
    public LayerMask mask;

    private void Start()
    {
        //don't show mouse cursor on screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentOffset = transform.position - player.transform.position;
        maxDistance = currentOffset.magnitude;
        angleOffset = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //get player mouse input
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

    private void FixedUpdate()
    {
        Vector3 direction = currentOffset;

        if (Physics.Raycast(player.transform.position, direction, out hit, maxDistance, mask))
        {
            Debug.DrawLine(player.transform.position, hit.point, Color.red);
            //only change magnitude of offset between player and camera
            currentOffset *= (hit.point - player.transform.position).magnitude / currentOffset.magnitude;
        }
        else
        {
            Debug.DrawLine(player.transform.position, transform.position, Color.yellow);
            currentOffset *= maxDistance / currentOffset.magnitude;
        }
    }

    private void LateUpdate()
    {
        // We apply the initial rotation to the camera.
        Quaternion initialRotation = Quaternion.Euler(angleOffset);

        Vector3 rot = transform.rotation.eulerAngles;

        angleVertical -= mouseY * cameraSpeed * Time.deltaTime;

        // We clamp the angle along the X axis to be between the min and max pitch.
        //angleVertical = Mathf.Clamp(angleVertical, mMinPitch, mMaxPitch);

        rot.y += mouseX * cameraSpeed * Time.deltaTime;
        Quaternion newRot = Quaternion.Euler(angleVertical, rot.y, 0.0f) * initialRotation;

        transform.rotation = newRot;

        //base rotation on camera view
        Vector3 forward = transform.rotation * Vector3.forward;
        Vector3 right = transform.rotation * Vector3.right;
        Vector3 up = transform.rotation * Vector3.up;

        //move camera
        Vector3 targetPos = player.transform.position;
        Vector3 desiredPosition = targetPos
            + forward * currentOffset.z
            + right * currentOffset.x
            + up * currentOffset.y;

        transform.position = desiredPosition;
    }
}
