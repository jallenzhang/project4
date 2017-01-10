//#define US_VERSION
using UnityEngine;
using System.Collections;
using System;

public class UnlockSceneDialog : BaseDialog<PetDialogParam> {

	public static void Popup(int cost, Action callback)
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/UnlockChangjingDialog_us", new PetDialogParam(){Cost = cost, m_callback = callback});
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/UnlockChangjingDialog", new PetDialogParam(){Cost = cost, m_callback = callback});
#endif
	}

	public void OnConfirm()
	{
		if (SettingManager.Instance.TotalDiamond < 100)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		GameData.Instance.AddDiamond(-100);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_MAP2);
		SettingManager.Instance.SceneLocked = 0;
		if (this.Param.m_callback != null)
			this.Param.m_callback();

		DialogManager.Instance.CloseDialog();
	}

	public void OnCancel()
	{
		DialogManager.Instance.CloseDialog();
	}
}
