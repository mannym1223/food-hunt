using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> requiredPickups;

	//Event to alert that the level is complete
	[Header("Event called when level ends")]
	public UnityEvent levelCompleteEvent;

	private AudioSource audio;

	private void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	/*** Checks if player has completed the level by acquiring all required
	 * pickups ***/
	public void UpdateLevelPickups()
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
			levelCompleteEvent.Invoke();
		}
	}
}
