using UnityEngine;
using System.Collections;
using anysdk;

public class MINIAnySDK : MonoBehaviour {


	void Awake()
	{
#if UNITY_ANDROID
		MINIAnySDKManager.Init();
#endif
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
