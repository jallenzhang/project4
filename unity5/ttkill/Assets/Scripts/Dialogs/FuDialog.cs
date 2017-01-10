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

	public void CloseDialog()
	{
		UILayout.Instance.BottomIn();
		DialogManager.Instance.ClearAll();
	}

	IEnumerator ShowMain()
	{
		yield return new WaitForSeconds(0.3f);
		MainArea.Instance.Show();
	}

}
