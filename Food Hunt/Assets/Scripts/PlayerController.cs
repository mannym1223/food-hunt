﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable] public class UnityEventFloat : UnityEvent<float> { }

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float jumpForce;
	public Camera mainCamera;
	public float maxHunger; //amount of hunger player can have
	public float hungerGain; //controls how fast player gets hungry
	public float knockbackForce; //control how far player gets knocked back

	private Rigidbody playerBody;

	//variable static to be updated by unity events
	private static float horizontal;
	private static float vertical;
	private static bool jumping;
	private static bool jumpInput;
	private static float currentHunger;
	private static bool stopPlayer;

	private bool isKnockedback;

	//amount of time to wait between hunger updates
	public float hungerWaitTime;
	public WaitForSeconds waitForHunger;

	//event to alert that player picked up item
	public UnityEvent playerPickupEvent;

	//event to alert that hunger has been updated
	public UnityEventFloat hungerUpdateEvent;

	//event to alert game over
	public UnityEvent gameOverEvent;

	private void Awake()
	{
		playerBody = GetComponent<Rigidbody>();
		waitForHunger = new WaitForSeconds(hungerWaitTime);
	}

	// Start is called before the first frame update
	void Start()
	{
		currentHunger = 0f;
		stopPlayer = false;
		jumping = false;
		isKnockedback = false;
		StartCoroutine(UpdateHunger());
	}

	// Update is called once per frame
	void Update()
	{
		//don't let player move if not active
		if (stopPlayer || isKnockedback)
		{
			return;
		}
		//read inputs from player
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");
		jumpInput = Input.GetButton("Jump");
	}
	private void FixedUpdate()
	{
		//check if player can jump
		if (jumpInput && !jumping)
		{
			PlayerJump();
		}
		MovePlayer();
	}

	private IEnumerator UpdateHunger()
	{
		//keep updating while player is active and hunger limit not reached
		while (!stopPlayer && currentHunger <= maxHunger)
		{
			yield return waitForHunger;

			//increase hunger over time
			currentHunger += hungerGain;
			hungerUpdateEvent.Invoke(currentHunger);
		}
		//reached hunger limit
		if (currentHunger >= maxHunger)
		{
			gameOverEvent.Invoke();
			Debug.Log("Game Over");
			DisableMovement();
		}
	}

	/*** Stops all player movement ***/
	public void DisableMovement()
	{
		Debug.Log("Disabled movement");
		//disable player movement
		
		stopPlayer = true;
		horizontal = 0f;
		vertical = 0f;
		jumpInput = false;
	}

	/*** Move player upwards ***/
	private void PlayerJump() {
		playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		jumping = true;
	}

	/*** Move player based on inputs ***/
	private void MovePlayer() {
		//base movement on camera view
		Vector3 forward = mainCamera.transform.forward;
		Vector3 right = mainCamera.transform.right;
		//prevent excessive movement on y axis
		forward *= vertical;
		forward.y = 0f;
		right *= horizontal;
		right.y = 0f;
		//keep speed consistent
		Vector3 direction = (forward + right).normalized;
		
		//move player
		playerBody.MovePosition(transform.position + direction * speed * Time.deltaTime);

	}

	/*** Knock player back in opposite direction of given transform ***/
	private void KnockbackPlayer(Transform givenTransform)
	{
		Vector3 direction = givenTransform.forward + givenTransform.right + givenTransform.up;
		direction.y += 1f;
		direction.Normalize();

		//reset velocity of player
		playerBody.velocity.Set(0f, 0f, 0f);

		//push player in opposite direction of object
		playerBody.AddForce(direction * knockbackForce, ForceMode.Impulse);
		isKnockedback = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		//check if player hit enemy
		if (collision.gameObject.CompareTag("Enemy"))
		{	
			//knockback player if not already being knocked back
			if (!isKnockedback)
			{
				KnockbackPlayer(collision.transform);
				currentHunger += collision.gameObject.GetComponent<EnemyController>().damage;
				hungerUpdateEvent.Invoke(currentHunger);
			}
		}
		else
		{
			//player is not midair anymore
			jumping = false;
			//player is not being knocked back anymore
			isKnockedback = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		//check if player touched food pickup
		if (other.CompareTag("Food")) {
			//lower hunger bar
			EatFood(other.gameObject.GetComponent<FoodController>().calories);

			//item picked up callback
			playerPickupEvent.Invoke();
		}
	}
	/*** Lowers player hunger bar based on food given ***/
	private void EatFood(float calories) {
		currentHunger -= calories;
		//don't let hunger go below zero
		if (currentHunger < 0) {
			currentHunger = 0f;
		}
		hungerUpdateEvent.Invoke(currentHunger);
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
