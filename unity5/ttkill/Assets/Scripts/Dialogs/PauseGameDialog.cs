//#define US_VERSION
using UnityEngine;
using System.Collections;

public class PauseGameDialog : DialogBase {

	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog(CommonAsset.Load("prefabs/Dialogs/PauseGameDialog_us"));
#else
		DialogManager.Instance.PopupDialog(CommonAsset.Load("prefabs/Dialogs/PauseGameDialog"));
#endif
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLeaveBattle()
	{
		Time.timeScale = 1.2f;
		GameData.Instance.Pause = false;
		DialogManager.Instance.CloseDialog();
		GameData.Instance.goldDouble = false;
		GameData.Instance.bulletCapacity = 1f;
		GameData.Instance.daojuDiamondAdditional = false;
		if (SettingManager.Instance.DiamondJiacheng == 1)
		{
			SettingManager.Instance.DiamondJiacheng = 2;
		}
//		WeaponManager.selectedWeapons.Clear();
		Ultilities.CleanMemory();
		Application.LoadLevel("ui");
	}
	
	void OnResumeGame()
	{
		Time.timeScale = 1.2f;
		GameData.Instance.Pause = false;
		GameData.Instance.bulletCapacity = 1f;
		GameData.Instance.goldDouble = false;
		DialogManager.Instance.CloseDialog();
	}
}
