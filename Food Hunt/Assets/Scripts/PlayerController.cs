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
	private float currentHunger;

	// Start is called before the first frame update
	void Start()
	{
		playerBody = GetComponent<Rigidbody>();
		Physics.gravity *= gravityModifier;
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
	}

	/*** Move player upwards ***/
	private void PlayerJump() {
		playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		jumping = true;
	}

	/*** Move player based on inputs ***/
	private void MovePlayer() {
		//base movement on camera view
		Vector3 moveForward = mainCamera.transform.forward;
		Vector3 moveRight = mainCamera.transform.right;
		//prevent movement on y axis
		moveForward.y = 0f;
		moveRight.y = 0f;
		//keep speed consistent
		moveForward.Normalize();
		moveRight.Normalize();

		//move player left or right
		transform.Translate(moveRight * horizontal * speed * Time.deltaTime);
		//move player forward or backward
		transform.Translate(moveForward * vertical * speed * Time.deltaTime);
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
