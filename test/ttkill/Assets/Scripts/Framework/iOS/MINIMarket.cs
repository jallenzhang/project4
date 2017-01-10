//#define US_VERSION
using UnityEngine;
using System.Collections;

public class MINIMarket : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if US_VERSION && UNITY_IPHONE
		MINIPaymentManager.init();
#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
