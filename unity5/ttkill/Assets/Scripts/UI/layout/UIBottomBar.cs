using UnityEngine;
using System.Collections;

public class UIBottomBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<UISprite>().width = (int)(((float)ScreenHelper.Instance.Width) / transform.localScale.x);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
