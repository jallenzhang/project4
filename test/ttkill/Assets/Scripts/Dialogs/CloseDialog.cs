using UnityEngine;
using System.Collections;

public class CloseDialog : MonoBehaviour {

	void OnClick()
	{
		DialogManager.Instance.CloseDialog();

		if (UILayout.Instance != null)
			UILayout.Instance.BottomIn();

		if (MainArea.Instance != null)
			MainArea.Instance.Show();

	}

}
