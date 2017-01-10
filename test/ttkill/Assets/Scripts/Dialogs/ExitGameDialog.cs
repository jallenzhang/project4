//#define US_VERSION

using UnityEngine;
using System.Collections;

public class ExitGameDialog : DialogBase {

	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/ExitGameDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/ExitGameDialog");
#endif
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnConfirm()
	{
		System.Diagnostics.Process.GetCurrentProcess().Kill();
	}

	public void OnCancel()
	{
		DialogManager.Instance.CloseDialog();
	}
}
