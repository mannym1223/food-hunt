using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
	public float visionStrength;
	public GameObject[] path;

	private Rigidbody enemyBody;
	private int currentPath;
	private Transform playerTransform;
	private NavMeshAgent agent;
	private AudioSource audio;

    // Start is called before the first frame update
    void Start() {
		enemyBody = GetComponent<Rigidbody>();
		playerTransform = GameObject.Find("Player").transform;
		agent = GetComponent<NavMeshAgent>();
		audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
		//if player is within range, take priority over path
		Vector3 playerDistance = (transform.position - playerTransform.position);
		if (playerDistance.magnitude < visionStrength) {
			ChangeDestination(playerTransform.position);
		}
		else {
			//distance from enemy to next path point
			Vector3 pathDistance = (transform.position - path[currentPath].transform.position);
			//check if enemy reached a path point
			if (pathDistance.magnitude < 0.2) {
				//move to next path point in list
				currentPath = (currentPath + 1) % path.Length;
			}
			ChangeDestination(path[currentPath].transform.position);
		}
		
    }

	private void OnCollisionEnter(Collision collision)
	{
		//play hit sound when colliding with player
		if (collision.gameObject.CompareTag("Player"))
		{
			audio.Play();
		}
	}

	private void ChangeDestination(Vector3 destination) {
		agent.destination = destination;
	}
}
