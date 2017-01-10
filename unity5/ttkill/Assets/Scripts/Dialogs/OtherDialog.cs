//#define US_VERSION
using UnityEngine;
using System.Collections;

public class OtherDialog : DialogBase {
	public UISprite btnGoldDouble;
	public UISprite btnDiamondAdditional;
	public UISprite btnBulletCapacity;

	public UILabel labelAOE;
	public UILabel labelBindong;
	public UILabel labelJiaxue;
	public UILabel labelJiatelin;

#if US_VERSION
	private string disableSpriteName = "anniu_hui";
	private string enableSpriteName = "anniu";
#else
	private string disableSpriteName = "jinbigoumaianniu_hui";
	private string enableSpriteName = "jinbigoumaianniu";
#endif
	private static string s_name = string.Empty;
	public static void Popup(string name)
	{
		s_name = name;
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/OtherDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/OtherDialog");
#endif

	}

	void Start()
	{
		if (GameData.Instance.goldDouble && btnGoldDouble != null)
		{
			btnGoldDouble.gameObject.GetComponent<Collider>().enabled = false;
			btnGoldDouble.spriteName = disableSpriteName;
		}
		else
		{
			btnGoldDouble.gameObject.GetComponent<Collider>().enabled = true;
			btnGoldDouble.spriteName = enableSpriteName;
		}

		if (SettingManager.Instance.DiamondJiacheng != 1 && btnGoldDouble != null)
		{
			if (GameData.Instance.daojuDiamondAdditional)
			{
				btnDiamondAdditional.gameObject.GetComponent<Collider>().enabled = false;
				btnDiamondAdditional.spriteName = disableSpriteName;
			}
			else
			{
				btnDiamondAdditional.gameObject.GetComponent<Collider>().enabled = true;
				btnDiamondAdditional.spriteName = enableSpriteName;
			}
		}
		else
		{
			btnDiamondAdditional.gameObject.GetComponent<Collider>().enabled = false;
			btnDiamondAdditional.spriteName = disableSpriteName;
		}

		if (GameData.Instance.bulletCapacity == 1.5f && btnBulletCapacity != null)
		{
			btnBulletCapacity.gameObject.GetComponent<Collider>().enabled = false;
			btnBulletCapacity.spriteName = disableSpriteName;
		}
		else
		{
			btnBulletCapacity.gameObject.GetComponent<Collider>().enabled = true;
			btnBulletCapacity.spriteName = enableSpriteName;
		}

		labelJiaxue.text = SettingManager.Instance.TotalJiaxue.ToString();
		labelBindong.text = SettingManager.Instance.TotalBindong.ToString();
		labelJiatelin.text = SettingManager.Instance.TotalJiatelin.ToString();
		labelAOE.text = SettingManager.Instance.TotalAoe.ToString();
	}

	public void onGoldDouble()
	{
		if (SettingManager.Instance.TotalDiamond < 10)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		GameData.Instance.AddDiamond(-5);
		GameData.Instance.goldDouble = true;

		btnGoldDouble.gameObject.GetComponent<Collider>().enabled = false;
		btnGoldDouble.spriteName = disableSpriteName;
		MTAManager.DoEvent(MTAPoint.MINI_BUY_GOLD_DOUBLE);
#if US_VERSION
		DialogManager.Instance.PopupFadeOutMessage("Buy double gold success!");
#else
		DialogManager.Instance.PopupFadeOutMessage("购买金币加倍成功");
#endif
	}

	public void onDiamondAddtional()
	{
		if (SettingManager.Instance.TotalDiamond < 10)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		GameData.Instance.AddDiamond(-10);
		if (SettingManager.Instance.DiamondJiacheng == 0)
		{
			SettingManager.Instance.DiamondJiacheng = 1;
		}

		GameData.Instance.daojuDiamondAdditional = true;
		btnDiamondAdditional.gameObject.GetComponent<Collider>().enabled = false;
		btnDiamondAdditional.spriteName = disableSpriteName;
		MTAManager.DoEvent(MTAPoint.MINI_BUY_DIAMOND_PERCENT_20);
#if US_VERSION
		DialogManager.Instance.PopupFadeOutMessage("Buy this item success!");
#else
		DialogManager.Instance.PopupFadeOutMessage("购买有概率获得20钻石成功");
#endif
	}

	public void onAddBulletCapcity()
	{
		if (SettingManager.Instance.TotalDiamond < 20)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		GameData.Instance.AddDiamond(-20);
		GameData.Instance.bulletCapacity = 1.5f;

		btnBulletCapacity.gameObject.GetComponent<Collider>().enabled = false;
		btnBulletCapacity.spriteName = disableSpriteName;
		MTAManager.DoEvent(MTAPoint.MINI_BUY_BULLET_CAPCITY);
#if US_VERSION
		DialogManager.Instance.PopupFadeOutMessage("Buy this item success");
#else
		DialogManager.Instance.PopupFadeOutMessage("购买装弹量增加50%成功");
#endif
	}

	public void onAddHPProp()
	{
		if (SettingManager.Instance.TotalDiamond < 20)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		GameData.Instance.AddDiamond(-20);
		GameData.Instance.AddJiaxue(1);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_XUE);
		labelJiaxue.text = SettingManager.Instance.TotalJiaxue.ToString();
#if US_VERSION
		DialogManager.Instance.PopupFadeOutMessage("Buy this item success");
#else
		DialogManager.Instance.PopupFadeOutMessage("购买恢复100点生命值成功");
#endif
	}

	public void onAddBindongProp()
	{
		if (SettingManager.Instance.TotalDiamond < 25)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}
		
		GameData.Instance.AddDiamond(-25);
		GameData.Instance.AddBindong(1);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_BINDONG);
		labelBindong.text = SettingManager.Instance.TotalBindong.ToString();

#if US_VERSION
		DialogManager.Instance.PopupFadeOutMessage("Buy this item success");
#else
		DialogManager.Instance.PopupFadeOutMessage("购买冰冻成功");
#endif
	}

	public void onAddJiatelinPorp()
	{
		if (SettingManager.Instance.TotalDiamond < 25)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}
		
		GameData.Instance.AddDiamond(-25);
		GameData.Instance.AddJiatelin(1);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_JIATELIN);
		labelJiatelin.text = SettingManager.Instance.TotalJiatelin.ToString();

#if US_VERSION
		DialogManager.Instance.PopupFadeOutMessage("Buy this item success");
#else
		DialogManager.Instance.PopupFadeOutMessage("购买加特林成功");
#endif
	}

	public void onAddAOEPorp()
	{
		if (SettingManager.Instance.TotalDiamond < 30)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}
		
		GameData.Instance.AddDiamond(-30);
		GameData.Instance.AddAoe(1);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_AOE);
		labelAOE.text = SettingManager.Instance.TotalAoe.ToString();

#if US_VERSION
		DialogManager.Instance.PopupFadeOutMessage("Buy this item success");
#else
		DialogManager.Instance.PopupFadeOutMessage("购买AOE成功");
#endif
	}

	public void CloseDialog()
	{
		DialogManager.Instance.CloseDialog();
	}

	public void onEnterFight()
	{
		
		if (SettingManager.Instance.TotalTili < 1)
		{
			NotEnoughTiliDialog.Popup();
			return;
		}

		Time.timeScale = 1.2f;

		if (SettingManager.Instance.TotalTili == 20)
		{
			SettingManager.Instance.TiliRecoverTime = System.DateTime.Now.ToString();
			//			MainArea.Instance.StartRecoverTili();
		}
		GameData.Instance.AddTili(-1);
		
		GameData.Instance.CurrentWave = 0;
		if (s_name.Contains("wujin"))
		{
			SettingManager.Instance.ChallegeModeTime += 1;
			SettingManager.Instance.UseTiliNum += 1;
			GameData.Instance.LevelType = LevelType.InfiniteLevel;
			int r = Random.Range(0, 2);
#if US_VERSION
			if (r == 0)
			{
				LoadingScene.Load("wujin_us");
			}
			else if (r == 1)
				LoadingScene.Load("wujin02_us");
#else
			if (r == 0)
			{
				LoadingScene.Load("wujin");
			}
			else if (r == 1)
				LoadingScene.Load("wujin02");
#endif
			//			else if (r == 2)
			//				LoadingScene.Load("wujin03");
		}
		else if (s_name.Contains("changjing"))
		{
			SettingManager.Instance.AdvantageModeTime += 1;
			SettingManager.Instance.UseTiliNum += 1;
			//			int r = Random.Range(0, 2);
			GameData.Instance.LevelType = LevelType.RushLevel;
#if US_VERSION
			if (SettingManager.Instance.SceneSelection == 1)
				LoadingScene.Load("changjing01_us");
			else if (SettingManager.Instance.SceneSelection == 2)
				LoadingScene.Load("changjing02_us");
#else
			if (SettingManager.Instance.SceneSelection == 1)
				LoadingScene.Load("changjing01");
			else if (SettingManager.Instance.SceneSelection == 2)
				LoadingScene.Load("changjing02");
#endif
		}
	}
}
