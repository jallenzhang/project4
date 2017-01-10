using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioPlayer))]
public class GameManager : MonoBehaviour {
	[HideInInspector]
	public AudioPlayer audioScript;

	void Awake()
	{
		Ultilities.gm = this;
	}

	// Use this for initialization
	void Start () {
		audioScript = GetComponent<AudioPlayer>();
//		UpdateStatus();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateStatus()
	{
//		audioScript = GetComponent<AudioPlayer>();
		audioScript.toggleBGM();
		audioScript.toggleFX();
	}
}
