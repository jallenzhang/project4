//#define US_VERSION
using UnityEngine;
using System.Collections;

public class ShangChengDialog : DialogBase
{
	public UILabel AoeLabel;
	public UILabel BindongLabel;
	public UILabel JiaxueLabel;
	public UILabel JiatelinLabel;

	public UIToggledObjects tab1;
	public UIToggledObjects tab2;
	public UIToggledObjects tab3;
	public UIToggledObjects tab4;

	float originalCameraDepth = 0;
	Camera camera;
	public static void Popup()
	{
//		UILayout.Instance.BottomOut();
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/shangcheng_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/shangcheng");
#endif
	}

	// Use this for initialization
	void Start () {
		camera = NGUITools.FindCameraForLayer(gameObject.layer);
		originalCameraDepth = camera.depth;
		camera.depth = 100f;

		AoeLabel.text = SettingManager.Instance.TotalAoe.ToString();
		BindongLabel.text = SettingManager.Instance.TotalBindong.ToString();
		JiaxueLabel.text = SettingManager.Instance.TotalJiaxue.ToString();
		JiatelinLabel.text = SettingManager.Instance.TotalJiatelin.ToString();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void Close()
	{
		camera.depth = originalCameraDepth;

		DialogManager.Instance.CloseDialog();
	}

	public void OnBuyDiamond(UILabel RMBNum)
	{
		Debug.Log("RMBNum is: " + RMBNum.text);
#if US_VERSION && UNITY_IPHONE
		switch(RMBNum.text)
		{
		case "0.99":
			LoadingDialog.Popup();
			MINIPaymentManager.buyItem(MINIPaymentManager.ONE_TIER);
			break;
		case "4.99":
			LoadingDialog.Popup();
			MINIPaymentManager.buyItem(MINIPaymentManager.FIVE_TIER);
			break;
		case "9.99":
			LoadingDialog.Popup();
			MINIPaymentManager.buyItem(MINIPaymentManager.TEN_TIER);
			break;
		case "19.99":
			LoadingDialog.Popup();
			MINIPaymentManager.buyItem(MINIPaymentManager.TWENTU_TIER);
			break;
		case "29.99":
			LoadingDialog.Popup();
			MINIPaymentManager.buyItem(MINIPaymentManager.THIRTY_TIER);
			break;
		case "49.99":
			LoadingDialog.Popup();
			MINIPaymentManager.buyItem(MINIPaymentManager.FIFTY_TIER);
			break;
		}
//#elif UNITY_ANDROID
//		switch(RMBNum.text)
//		{
//		case "2":
//			LoadingDialog.Popup();
//			MINIAnySDKManager.BuyItem(MINIAnySDKManager.B20);
//			break;
//		case "4":
//			LoadingDialog.Popup();
//			MINIAnySDKManager.BuyItem(MINIAnySDKManager.B45);
//			break;
//		case "8":
//			LoadingDialog.Popup();
//			MINIAnySDKManager.BuyItem(MINIAnySDKManager.B96);
//			break;
//		case "12":
//			LoadingDialog.Popup();
//			MINIAnySDKManager.BuyItem(MINIAnySDKManager.B155);
//			break;
//		case "30":
//			LoadingDialog.Popup();
//			MINIAnySDKManager.BuyItem(MINIAnySDKManager.B400);
//			break;
//		case "60":
//			LoadingDialog.Popup();
//			MINIAnySDKManager.BuyItem(MINIAnySDKManager.B1000);
//			break;
//		}
#else
		int RMB = int.Parse(RMBNum.text);
		int diamond = 0;
		switch(RMB)
		{
		case 2:
			diamond = 20;
			break;
		case 4:
			diamond = 45;
			break;
		case 8:
			diamond = 96;
			break;
		case 12:
			diamond = 155;
			break;
		case 30:
			diamond = 400;
			break;
		case 60:
			diamond = 1000;
			break;
		}
		GameData.Instance.AddDiamond(diamond);
#endif

	}

	public void OnBuyGold(UILabel diamondNum)
	{
		int diamond = int.Parse(diamondNum.text);
//		GameData.Instance.AddGold(
		if (diamond > GameData.Instance.currentDiamond)
		{
			NotEnoughDiamondDialog.Popup(false);
			return;
		}

		switch(diamond)
		{
		case 10:
			GameData.Instance.AddGold(500);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_GOLD_500);
			break;
		case 50:
			GameData.Instance.AddGold(2500);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_GOLD_2500);
			break;
		case 100:
			GameData.Instance.AddGold(5200);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_GOLD_5200);
			break;
		case 200:
			GameData.Instance.AddGold(11000);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_GOLD_11000);
			break;
		case 500:
			GameData.Instance.AddGold(28000);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_GOLD_28000);
			break;
		case 1000:
			GameData.Instance.AddGold(88000);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_GOLD_88000);
			break;
		}

		GameData.Instance.AddDiamond(-diamond);

	}

	public void OnBuyTili(UILabel diamondNum)
	{
		int diamond = int.Parse(diamondNum.text);

		if (diamond > GameData.Instance.currentDiamond)
		{
			NotEnoughDiamondDialog.Popup(false);
			return;
		}
		
		switch(diamond)
		{
		case 1:
			GameData.Instance.AddTili(1);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_TILI_1);
			break;
		case 5:
			GameData.Instance.AddTili(5);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_TILI_5);
			break;
		case 10:
			GameData.Instance.AddTili(12);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_TILI_12);
			break;
		case 15:
			GameData.Instance.AddTili(20);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_TILI_20);
			break;
		case 30:
			GameData.Instance.AddTili(50);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_TILI_50);
			break;
		case 50:
			GameData.Instance.AddTili(120);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_TILI_120);
			break;
		}
		
		GameData.Instance.AddDiamond(-diamond);
	}

	public void OnBuyBindong()
	{
		int diamond = 25;

		if (diamond > GameData.Instance.currentDiamond)
		{
			NotEnoughDiamondDialog.Popup(false);
			return;
		}

		GameData.Instance.AddBindong(1);
		GameData.Instance.AddDiamond(-diamond);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_BINDONG);
		BindongLabel.text = SettingManager.Instance.TotalBindong.ToString();
	}

	public void OnBuyJiaxue()
	{
		int diamond = 20;
		if (diamond > GameData.Instance.currentDiamond)
		{
			NotEnoughDiamondDialog.Popup(false);
			return;
		}

		GameData.Instance.AddJiaxue(1);
		GameData.Instance.AddDiamond(-diamond);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_XUE);
		JiaxueLabel.text = SettingManager.Instance.TotalJiaxue.ToString();
	}

	public void OnBuyJiatelin()
	{
		int diamond = 25;
		if (diamond > GameData.Instance.currentDiamond)
		{
			NotEnoughDiamondDialog.Popup(false);
			return;
		}

		GameData.Instance.AddJiatelin(1);
		GameData.Instance.AddDiamond(-diamond);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_JIATELIN);
		JiatelinLabel.text = SettingManager.Instance.TotalJiatelin.ToString();
	}

	public void OnBuyAoe()
	{
		int diamond = 30;
		if (diamond > GameData.Instance.currentDiamond)
		{
			NotEnoughDiamondDialog.Popup(false);
			return;
		}

		GameData.Instance.AddAoe(1);
		GameData.Instance.AddDiamond(-diamond);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_AOE);
		AoeLabel.text = SettingManager.Instance.TotalAoe.ToString();
	}
}
