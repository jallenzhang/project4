using UnityEngine;
using System.Collections;

public class LoadingDialog : DialogBase {

	public static void Popup()
	{
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/LoadingDialog");
	}

	public static void CloseDialog()
	{
		LoadingDialog dialog =DialogManager.Instance.transform.GetComponentInChildren<LoadingDialog>();
		if (dialog != null)
			dialog.Close();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Close()
	{
		DialogManager.Instance.CloseDialog();
	}
}
