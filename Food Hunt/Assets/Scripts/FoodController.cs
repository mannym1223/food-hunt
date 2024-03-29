﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
	public float rotateSpeed;
	public float elevateSpeed;
	public float moveDist;
	public float calories;

	private Vector3 rotation;
	private Vector3 startPoint;
	private Vector3 lowPoint;
	private bool movingDown = true;
	private AudioSource audio;

	private void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	// Start is called before the first frame update
	void Start()
    {
		rotation = new Vector3(0, rotateSpeed, 0);
		startPoint = transform.position;
		lowPoint = transform.position;
		lowPoint.y -= moveDist;
    }

    // Update is called once per frame
    void Update()
    {
		//rotate the food pickup
		transform.Rotate(rotation * Time.deltaTime);

		//move pickup up and down slowly
		if (movingDown) {
			//move pickup downwards
			transform.Translate(new Vector3(0,-(elevateSpeed * Time.deltaTime),0));
			//pickup reached low point
			if ((transform.position - lowPoint).magnitude <= 0.05) {
				movingDown = !movingDown;
			}
		}
		else {
			//move pickup upwards
			transform.Translate(new Vector3(0, elevateSpeed * Time.deltaTime, 0));
			//pickup reached high point
			if ((transform.position - startPoint).magnitude <= 0.05) {
				movingDown = !movingDown;
			}
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		// only player can trigger audio
		if (audio != null && other.CompareTag("Player"))
		{
			Debug.Log("---Audio clip played---");
			audio.Play();
			//wait for audio clip to end before destroying object
			Invoke("DestroyPickup", audio.clip.length);
		}
	}

	private void DestroyPickup()
	{
		//destroy game object
		Destroy(gameObject);
	}
}
