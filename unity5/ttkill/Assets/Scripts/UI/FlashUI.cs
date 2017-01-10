using UnityEngine;
using System.Collections;

public class FlashUI : DialogBase {

	public static void PopUp()
	{
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/FlashDialog");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDemo()
	{
		SettingManager.Instance.FirstIn = 0;
		Application.LoadLevel("changjing01");
	}
}
