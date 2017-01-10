﻿//#define US_VERSION

using UnityEngine;
using System.Collections;

public class FriendsDialog : DialogBase
{

	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/haoyou_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/haoyou");
#endif
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
