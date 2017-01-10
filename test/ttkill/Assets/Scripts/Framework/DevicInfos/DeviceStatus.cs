using UnityEngine;
using System.Collections;

public class DeviceStatus : MonoBehaviour {
	private Rect window1;
	private float windowsWidth = 200;
	private float windowsHeight = 200;

	private static int count = 0;
	private static float milliSecond = 0;
	private static float deltaTime = 0.0f;//用于显示帧率的deltaTime
	private static float fps = 0;
	// Use this for initialization
	void Start () {
		window1 = new Rect(0, Screen.height - windowsHeight, windowsWidth, windowsHeight);
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

#if UNITY_EDITOR_OSX
	void OnGUI()
	{
		GUI.Window(0, window1, onWindow1, "游戏状态");  
	}
#endif

	void onWindow1(int windowId)
	{
		GUI.Label(new Rect(10, 20, windowsWidth / 2f, 20), "显卡名称");
		GUI.Label(new Rect(windowsWidth / 2f, 20, windowsWidth / 2f, 20), SystemInfo.graphicsDeviceName);

		GUI.Label(new Rect(10, 50, windowsWidth / 2f, 20), "显存");
		GUI.Label(new Rect(windowsWidth / 2f, 50, windowsWidth / 2f, 20), SystemInfo.graphicsMemorySize.ToString() + "M");

		GUI.Label(new Rect(10, 80, windowsWidth / 2f, 20), "内存大小");
		GUI.Label(new Rect(windowsWidth / 2f, 80, windowsWidth / 2f, 20), SystemInfo.systemMemorySize.ToString() + "M");

		if (++count > 10)
		{
			count = 0;
			milliSecond = deltaTime * 1000.0f;
			fps = 1.0f / deltaTime;
		}
		string fpsText = string.Format("{0:0.0} ms ({1:0.} 帧每秒)", milliSecond, fps);

		GUI.Label(new Rect(10, 110, windowsWidth / 2f, 20), "帧数");
		GUI.Label(new Rect(windowsWidth / 2f - 30, 110, windowsWidth / 2f + 30, 20), fpsText);

//		GUI.Label(new Rect(10, 140, windowsWidth / 2f, 20), "rigibody");
//		GUI.Label(new Rect(windowsWidth / 2f - 30, 140, windowsWidth / 2f + 30, 20), GameObject.FindWithTag("Player").rigidbody.velocity.ToString());
	}
}
