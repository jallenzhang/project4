//#define US_VERSION
using UnityEngine;
using System.Collections;

public class NotEnoughGoldDialog : DialogBase {
	float originalCameraDepth = 0;
	Camera camera;
	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/NotEnoughGoldDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/NotEnoughGoldDialog");
#endif
	}


	// Use this for initialization
	void Start () {
		camera = NGUITools.FindCameraForLayer(gameObject.layer);
		originalCameraDepth = camera.depth;
		camera.depth = 100f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnConfirm()
	{
		OnCancel();
		ShangChengDialog.Popup();
	}
	
	public void OnCancel()
	{
		camera.depth = originalCameraDepth;
		DialogManager.Instance.CloseDialog();
	}
}
