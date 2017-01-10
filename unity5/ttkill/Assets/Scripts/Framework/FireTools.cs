using UnityEngine;
using System.Collections;

public class FireTools : MonoBehaviour {
	private HeroController controller;
	private MouseFollow_ForRPG_CSharp mouseFollow;
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag ("Player");
		if (go != null) {
			controller = go.GetComponent<HeroController>();		
		}

		mouseFollow  = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseFollow_ForRPG_CSharp>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
#if UNITY_EDITOR_OSX
	void OnGUI()
	{
//		GUI.Label (new Rect (10, 30, 100, 20), "HeroSpeed:");
//		controller.speed = GUI.HorizontalSlider (new Rect (80, 30, 200, 40), controller.speed, 1, 10);
//
//		GUI.Label (new Rect (250, 30, 100, 20), controller.speed.ToString ());
//
//		GUI.Label (new Rect ( 10, 60, 100, 20), "View Distance:");
//		mouseFollow.distance = GUI.HorizontalSlider (new Rect (80, 60, 200, 40), this.mouseFollow.distance, 5, 20);
//		GUI.Label (new Rect (250, 60, 100, 20), mouseFollow.distance.ToString ());
//
//		GUI.Label (new Rect ( 10, 90, 100, 20), "View angle:");
//		mouseFollow.ViewAngle = GUI.HorizontalSlider(new Rect (80, 90, 200, 40), this.mouseFollow.ViewAngle, 20, 70);
//		GUI.Label (new Rect (250, 90, 100, 20), mouseFollow.ViewAngle.ToString ());
//		if (GUI.Button (new Rect (10, 100, 100, 30), "Next"))
//			Application.LoadLevel("ttkill copy");
	}
#endif
}
