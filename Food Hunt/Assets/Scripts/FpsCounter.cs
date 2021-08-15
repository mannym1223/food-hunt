using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsDisplay;

	private void Start()
	{
		Application.targetFrameRate = 120;
	}

	// Update is called once per frame
	void Update()
    {
        float fps = 1 / Time.unscaledDeltaTime;
        fps = Mathf.Round(fps);
        fpsDisplay.text = fps.ToString();
    }
}
