using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> requiredPickups;

	public delegate void levelCompleteAction();
	public static event levelCompleteAction OnLevelComplete;

	private AudioSource audio;

	private void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		PlayerController.OnPickup += UpdateLevelPickups;
	}

	private void OnDisable()
	{
		PlayerController.OnPickup -= UpdateLevelPickups;
	}
	/*** Checks if player has completed the level by acquiring all required
	 * pickups ***/
	void UpdateLevelPickups()
	{
		//decrease number of pickups left
		requiredPickups.RemoveAt(requiredPickups.Count-1);
		Debug.Log("Number of pickups left: " + requiredPickups.Count);
		if (requiredPickups.Count == 0)
		{
			//end the level
			Debug.Log("You Win!");
			//play level complete audio
			audio.Play();
			//alert other objects that level is over
			OnLevelComplete();
		}
	}
}
