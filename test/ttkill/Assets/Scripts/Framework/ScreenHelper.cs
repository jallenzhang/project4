using UnityEngine;
using System.Collections;
using System;

public class ScreenHelper : MonoBehaviour {
	private static ScreenHelper helper;

	public static ScreenHelper Instance
	{
		get
		{
			return helper;
		}
	}

	public int Width
	{
		get
		{
			UIRoot root = GameObject.FindObjectOfType<UIRoot>();
			float s = (float)root.activeHeight / Screen.height;
			int width =  Mathf.CeilToInt(Screen.width * s);
			return width;
		}
	}

	public int Height
	{
		get
		{
			UIRoot root = GameObject.FindObjectOfType<UIRoot>();
			float s = (float)root.activeHeight / Screen.height;
			int height =  Mathf.CeilToInt(Screen.height * s);
			return height;
		}
	}


	static public float getCameraFOV(float currentFOV)
	{
		UIRoot root = GameObject.FindObjectOfType<UIRoot>();
		Debug.Log("root.manualHeight " + root.manualHeight);
		float scale =Convert.ToSingle(root.manualHeight / 640f);
		return currentFOV * scale;
	}

	void Awake()
	{
		helper = this;
	}

	// Use this for initialization
	void Start () {
	
	}
}
