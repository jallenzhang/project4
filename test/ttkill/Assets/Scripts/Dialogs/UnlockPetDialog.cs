//#define US_VERSION
using UnityEngine;
using System.Collections;
using System;

public class PetDialogParam : DialogParam
{
	public int Cost;
	public int pet_type;
	public Action m_callback;
}

public class UnlockPetDialog : BaseDialog<PetDialogParam> {
	public UILabel description;
	private int cost;
	private Action m_callback;
	public static void PopUp(int cost, int type, Action callback)
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/UnlockPetDialog_us", new PetDialogParam(){Cost = cost, pet_type = type, m_callback = callback});
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/UnlockPetDialog", new PetDialogParam(){Cost = cost, pet_type = type, m_callback = callback});
#endif
	}

	void MTABuyPoint()
	{
		switch((PetType)this.Param.pet_type)
		{
		case PetType.songshu:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_PET_1);
			break;
		case PetType.tuzi:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_PET_2);
			break;
		case PetType.pet3:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_PET_3);
			break;
		case PetType.pet4:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_PET_4);
			break;
		case PetType.pet5:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_PET_5);
			break;
		case PetType.pet6:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_PET_6);
			break;
		case PetType.pet7:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_PET_7);
			break;
		}
	}

	public void Start()
	{
		description.text = string.Format(description.text, this.Param.Cost);	
	}

	public void OnConfirm()
	{
		OnCancel();

		if (GameData.Instance.currentGold < this.Param.Cost)
		{
			NotEnoughGoldDialog.Popup();
			return;
		}

		PetDB.Instance.UpdatePet(this.Param.pet_type, 1, 0);
		GameData.Instance.AddGold(-this.Param.Cost);
		SettingManager.Instance.PetNum += 1;
		MTABuyPoint();
		if (this.Param.m_callback != null)
			this.Param.m_callback();
//		if (s_popupShangcheng)
//			ShangChengDialog.Popup();
	}
	
	public void OnCancel()
	{
		DialogManager.Instance.CloseDialog();
	}
}
