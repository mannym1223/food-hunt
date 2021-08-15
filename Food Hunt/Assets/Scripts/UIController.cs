using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
	//hunger bar variables
	public Image hungerBar;
	private float maxHunger;

	//end screen variables
	public GameObject endScreenPanel;
	public TextMeshProUGUI winText;
	public TextMeshProUGUI loseText;

	private void Start()
	{
		hungerBar.fillAmount = 0f;
		maxHunger = FindObjectOfType<PlayerController>().maxHunger;
	}

	private void EnableEndScreen()
	{
		//display panel and activate end messages
		endScreenPanel.SetActive(true);
	}

	public void DisplayWinScreen()
	{
		EnableEndScreen();
		loseText.enabled = false;
	}

	public void DisplayLoseScreen()
	{
		EnableEndScreen();
		winText.enabled = false;
	}

	public void UpdateHungerBar(float currentHunger)
	{
		hungerBar.fillAmount = currentHunger / maxHunger;
	}

	
}
