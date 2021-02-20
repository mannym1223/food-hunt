using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float jumpForce;
	public float gravityModifier;
	public Camera mainCamera;

	private Rigidbody playerBody;
	private float horizontal;
	private float vertical;
	private bool jumping;
	private bool jumpInput;
	private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
		playerBody = GetComponent<Rigidbody>();
		Physics.gravity *= gravityModifier;
		cameraOffset = transform.position - mainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		//read inputs from player
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		jumpInput = Input.GetButton("Jump");

    }

	private void FixedUpdate() {
		//check if player can jump
		if (jumpInput && !jumping) {
			PlayerJump();
		}
		MovePlayer();

		//keep camera at offset
		mainCamera.transform.position = transform.position - cameraOffset;
		//keep camera looking at player
		mainCamera.transform.LookAt(transform);
	}

	/*** Move player upwards using a force ***/
	private void PlayerJump() {
		playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		jumping = true;
	}

	/*** Move player based on inputs ***/
	private void MovePlayer() {
		//move player left or right
		transform.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
		//move player forward or backward
		transform.Translate(Vector3.forward * vertical * speed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
		//player is not midair anymore
		jumping = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		//check if player touched food pickup
		if (other.CompareTag("Food")) {
			//destroy the food object
			Destroy(other.gameObject);

			//TODO do something for the player
		}
	}
}
