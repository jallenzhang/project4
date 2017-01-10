//#define  US_VERSION
using UnityEngine;
using System.Collections;

public class AdvantureLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GotoLevelMenu()
	{
		Ultilities.CleanMemory();
#if US_VERSION
		Application.LoadLevel("chuanggan02_us");
#else
		Application.LoadLevel("chuanggan02");
#endif

	}
}
