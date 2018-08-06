using UnityEngine;
using System.Collections;

public class ScaleUI : MonoBehaviour {

    private float ScreenRate = 480.0f / 800.0f;
	void OnEnable()
	{   
		ChangeScale();
		UICamera.onScreenResize += ChangeScale;
	}

	void ChangeScale()
	{
        this.transform.localScale = Vector3.one * Mathf.Clamp01((Screen.width / (float)Screen.height) * ScreenRate);
	}

	void OnDisable()
	{
		UICamera.onScreenResize -= ChangeScale;
	}

}
