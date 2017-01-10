//#define US_VERSION

using UnityEngine;
using System.Collections;

public class FuhuoDialog : DialogBase {
	public UILabel descrip;
	private static int s_count;
	public static void PopUp(int count)
	{
		s_count = count;
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/FuhuoDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/FuhuoDialog");
#endif
	}

	int diamondValue = 0;
	void Start()
	{
		Time.timeScale = 0;
		GameData.Instance.Pause = true;
		if (s_count == 1)
			diamondValue = 10;
		else if (s_count == 2)
			diamondValue = 20;
		else if (s_count == 3)
			diamondValue = 30;

		descrip.text = string.Format(descrip.text, diamondValue);
	}

	public void OnConfirm()
	{
		if (diamondValue > SettingManager.Instance.TotalDiamond)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		GameData.Instance.AddDiamond(-diamondValue);
		if (s_count == 1)
			MTAManager.DoEvent(MTAPoint.MINI_BUY_FUHUO_1);
		else if (s_count == 2)
			MTAManager.DoEvent(MTAPoint.MINI_BUY_FUHUO_2);
		else if (s_count == 3)
			MTAManager.DoEvent(MTAPoint.MINI_BUY_FUHUO_3);

		Time.timeScale = 1.2f;
		GameData.Instance.Pause = false;
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent<HeroController>().hp = player.GetComponent<HeroController>().maxHp;
	    UIBattleSceneLogic.Instance.SetHp(player.GetComponent<HeroController>().hp / player.GetComponent<HeroController>().maxHp);
//		player.GetComponent<HeroController>().ChangeState(AnimState.idle);
		UIBattleSceneLogic.Instance.PlayAoeEffect();

		DialogManager.Instance.CloseDialog();
	}
	
	public void OnCancel()
	{
		DialogManager.Instance.CloseDialog();
		Camera.main.GetComponent<GrayscaleEffect>().enabled = true;
		ResultDialog.Popup(false);
	}
}
