//#define US_VERSION
using UnityEngine;
using System.Collections;
using System;

public class UnlockPetPositionDialog : BaseDialog<PetDialogParam> {
	Camera camera;
	float originalCameraDepth;
	public static void PopUp(int cost, int type, Action callback)
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/UnlockPetPos_us", new PetDialogParam(){Cost = cost, pet_type = type, m_callback = callback});
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/UnlockPetPos", new PetDialogParam(){Cost = cost, pet_type = type, m_callback = callback});
#endif
	}

	void Start()
	{
		camera = NGUITools.FindCameraForLayer(gameObject.layer);
		originalCameraDepth = camera.depth;
		camera.depth = 88;
	}

	public void OnConfirm()
	{
		OnCancel();
		
		if (GameData.Instance.currentDiamond < this.Param.Cost)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		GameData.Instance.AddDiamond(-this.Param.Cost);
		SettingManager.Instance.PetLocked = 0;
		MTAManager.DoEvent(MTAPoint.MINI_BUY_PET_SOLT);
		if (this.Param.m_callback != null)
			this.Param.m_callback();
	}
	
	public void OnCancel()
	{
		camera.depth = originalCameraDepth;
		DialogManager.Instance.CloseDialog();
	}
}
