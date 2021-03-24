using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBarController : MonoBehaviour
{
    private Image hungerBar;
	private float maxHunger;

	private void Awake()
	{
		hungerBar = GetComponent<Image>();
	}

	private void Start()
	{
		hungerBar.fillAmount = 0f;
		maxHunger = FindObjectOfType<PlayerController>().maxHunger;
	}

	public void UpdateHungerBar(float currentHunger)
	{
		hungerBar.fillAmount = currentHunger / maxHunger;
	}
}
