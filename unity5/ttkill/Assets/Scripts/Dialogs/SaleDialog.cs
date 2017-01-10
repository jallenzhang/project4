//#define US_VERSION
using UnityEngine;
using System.Collections;

public class SaleDialog : DialogBase {
	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/SALEDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/SALEDialog");
#endif
	}

	public void OnBuy(GameObject item)
	{
		int index = int.Parse(item.name);
#if US_VERSION
		switch(index)
		{
		case 3:
			if (SettingManager.Instance.TotalDiamond < 70)
			{
				NotEnoughDiamondDialog.Popup();
				return;
			}

			GameData.Instance.AddDiamond(-70);
			GameData.Instance.AddBindong(2);
			GameData.Instance.AddJiaxue(3);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_SALE_TYPE_3);
			DialogManager.Instance.PopupFadeOutMessage("Buy this item success!", Vector3.zero, 3);
			break;
		case 4:
			//3 AOE + 3 jiatelin    8yuan
			if (SettingManager.Instance.TotalDiamond < 80)
			{
				NotEnoughDiamondDialog.Popup();
				return;
			}

			GameData.Instance.AddDiamond(-80);
			GameData.Instance.AddAoe(3);
			GameData.Instance.AddJiatelin(3);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_SALE_TYPE_4);
			DialogManager.Instance.PopupFadeOutMessage("Buy this item success!", Vector3.zero, 3);
			break;
		case 5:
			//3 jiaxue + 2000 gold    6yuan
			if (SettingManager.Instance.TotalDiamond < 60)
			{
				NotEnoughDiamondDialog.Popup();
				return;
			}

			GameData.Instance.AddDiamond(-60);
			GameData.Instance.AddJiaxue(3);
			GameData.Instance.AddGold(2000);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_SALE_TYPE_5);
			DialogManager.Instance.PopupFadeOutMessage("Buy this item success!", Vector3.zero, 3);
			break;
		case 6:
			//2 AOE + 4000 gold    8yuan
			if (SettingManager.Instance.TotalDiamond < 80)
			{
				NotEnoughDiamondDialog.Popup();
				return;
			}

			GameData.Instance.AddDiamond(-80);
			GameData.Instance.AddAoe(2);
			GameData.Instance.AddGold(4000);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_SALE_TYPE_6);
			DialogManager.Instance.PopupFadeOutMessage("Buy this item success!", Vector3.zero, 3);
			break;
		}
#else
		switch(index)
		{
		case 1:
			//50 diamonds + 2500 gold    6yuan
			GameData.Instance.AddGold(2500);
			GameData.Instance.AddDiamond(50);
			DialogManager.Instance.PopupFadeOutMessage("2500金币和50钻石购买成功", Vector3.zero, 3);
			break;
		case 2:
			//100 diamonds + 5000 gold    10yuan
			GameData.Instance.AddGold(5000);
			GameData.Instance.AddDiamond(100);
			DialogManager.Instance.PopupFadeOutMessage("5000金币和100钻石购买成功", Vector3.zero, 3);
			break;
		case 3:
			//2 bindong + 3 jiaxue    7yuan
			GameData.Instance.AddBindong(2);
			GameData.Instance.AddJiaxue(3);
			DialogManager.Instance.PopupFadeOutMessage("冰霜星星*2和急救箱*3购买成功", Vector3.zero, 3);
			break;
		case 4:
			//3 AOE + 3 jiatelin    8yuan
			GameData.Instance.AddAoe(3);
			GameData.Instance.AddJiatelin(3);
			DialogManager.Instance.PopupFadeOutMessage("原子弹*3和加特林*3购买成功", Vector3.zero, 3);
			break;
		case 5:
			//3 jiaxue + 2000 gold    6yuan
			GameData.Instance.AddJiaxue(3);
			GameData.Instance.AddGold(2000);
			DialogManager.Instance.PopupFadeOutMessage("急救箱*3和2000金币购买成功", Vector3.zero, 3);
			break;
		case 6:
			//2 AOE + 4000 gold    8yuan
			GameData.Instance.AddAoe(2);
			GameData.Instance.AddGold(4000);
			DialogManager.Instance.PopupFadeOutMessage("原子弹*2和4000金币购买成功", Vector3.zero, 3);
			break;
		}
#endif

	}

	public void CloseDialog()
	{
		if (UILayout.Instance != null)
			UILayout.Instance.BottomIn();
		
		if (MainArea.Instance != null)
			MainArea.Instance.Show();

		DialogManager.Instance.CloseDialog();
	}
}
