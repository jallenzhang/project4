//#define US_VERSION
using UnityEngine;
using System.Collections;

public class NotEnoughDiamondDialog : DialogBase {
	float originalCameraDepth = 0;
	Camera camera;

	private static bool s_popupShangcheng;
	public static void Popup(bool popupShangcheng = true)
	{
		s_popupShangcheng = popupShangcheng;
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/NotEnoughDiamondDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/NotEnoughDiamondDialog");
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
		if (s_popupShangcheng)
			ShangChengDialog.Popup();
	}
	
	public void OnCancel()
	{
		camera.depth = originalCameraDepth;
		DialogManager.Instance.CloseDialog();
	}
}
