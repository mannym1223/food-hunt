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
	public float knockbackForce; //control how far player gets knocked back

	private Rigidbody playerBody;
	private float horizontal;
	private float vertical;
	private bool jumping;
	private bool jumpInput;
	private float currentHunger;
	private bool stopPlayer;

	//event to alert other classes that player picked up item
	public delegate void pickupAction();
	public static event pickupAction OnPickup;

	private void Awake()
	{
		playerBody = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	void Start()
	{
		Physics.gravity *= gravityModifier;
		currentHunger = 0f;
		stopPlayer = false;
	}

	// Update is called once per frame
	void Update()
	{
		//don't let player move if not active
		if (stopPlayer)
		{
			return;
		}
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
		if (currentHunger % 100 <= 0.1)
		{
			Debug.Log("Hunger bar: " + currentHunger);
		}
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

	/*** Knock player back in opposite direction of given transform ***/
	private void KnockbackPlayer(Transform givenTransform)
	{
		//push player in opposite direction of object
		playerBody.AddForce(givenTransform.forward.normalized * knockbackForce, ForceMode.Impulse);
	}

	private void OnCollisionEnter(Collision collision)
	{
		//player is not midair anymore
		jumping = false;

		//check if player hit enemy
		if (collision.gameObject.CompareTag("Enemy"))
		{
			//knockback player
			KnockbackPlayer(collision.transform);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		//check if player touched food pickup
		if (other.CompareTag("Food")) {
			//lower hunger bar
			EatFood(other.gameObject.GetComponent<FoodController>().calories);

			//item picked up callback
			if (OnPickup != null)
			{
				OnPickup();
			}
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

	void EndLevel()
	{
		//disable player movement
		stopPlayer = true;
		horizontal = 0f;
		vertical = 0f;
		jumpInput = false;
	}

	private void OnEnable()
	{
		LevelManager.OnLevelComplete += EndLevel;
	}

	private void OnDisable()
	{
		LevelManager.OnLevelComplete -= EndLevel;
	}
}
