//#define US_VERSION

using UnityEngine;
using System.Collections;

public class FuDialog : DialogBase
{

	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/fushengji_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/fushengji");
#endif
	}

	// Use this for initialization
	void Start()
	{
	}

	public void Close()
	{
		DialogManager.Instance.CloseDialog();
		if (UILayout.Instance != null)
			UILayout.Instance.BottomIn();
		
		if (MainArea.Instance != null)
			MainArea.Instance.Show();
	}
}
