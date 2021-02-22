using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float jumpForce;
	public float gravityModifier;
	public Camera mainCamera;
	public float maxHunger; //amount of hunger player can have
	public float hungerGain; //controls how fast player gets hungry

	private Rigidbody playerBody;
	private float horizontal;
	private float vertical;
	private bool jumping;
	private bool jumpInput;
	private Vector3 cameraOffset;
	private float currentHunger;

	// Start is called before the first frame update
	void Start()
	{
		playerBody = GetComponent<Rigidbody>();
		Physics.gravity *= gravityModifier;
		cameraOffset = transform.position - mainCamera.transform.position;
		currentHunger = 0f;
	}

	// Update is called once per frame
	void Update()
	{
		//read inputs from player
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		jumpInput = Input.GetButton("Jump");


		//reached hunger limit
		if (currentHunger >= maxHunger) {
			Debug.Log("Game Over");
			return;
		}
		//increase hunger over time
		currentHunger += hungerGain;
		Debug.Log("Hunger bar: " + currentHunger);
		
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

	/*** Move player upwards ***/
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
			//lower hunger bar
			EatFood(other.gameObject.GetComponent<FoodController>().calories);

			//destroy the food object
			Destroy(other.gameObject);
		}
	}
	/*** Lowers player hunger bar based on food given ***/
	private void EatFood(float calories) {
		currentHunger -= calories;
		//don't let hunger go below zero
		if (currentHunger < 0) {
			currentHunger = 0f;
		}
	}
}
