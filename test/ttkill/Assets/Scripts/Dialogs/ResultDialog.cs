//#define US_VERSION
using UnityEngine;
using System.Collections;

public class ResultDialog : DialogBase
{
	public UILabel labelScore;
	public UILabel labelGold;
	public UILabel labelDiamond;
	public GameObject directPassLevel;
	
	private static bool s_success = false;
	
	public static void Popup(bool success)
	{
		//		UILayout.Instance.BottomOut();
		s_success = success;
		if (success)
		{
			#if US_VERSION
			DialogManager.Instance.PopupDialog("prefabs/Dialogs/jiesuan_success_us");
			#else
			DialogManager.Instance.PopupDialog("prefabs/Dialogs/jiesuan_success");
			#endif
		}
		else
		{
			#if US_VERSION
			DialogManager.Instance.PopupDialog("prefabs/Dialogs/jiesuan_fail_us");
			#else
			DialogManager.Instance.PopupDialog("prefabs/Dialogs/jiesuan_fail");
			#endif
		}
	}
	
	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	IEnumerator FireEvent()
	{
		yield return new WaitForEndOfFrame();
		
		if (SettingManager.Instance.TutorialRetry == 1 || SettingManager.Instance.TutorialSeq <= 21)
		{
			TuorialTriggerManager.Instance.OpenTutorialDialogByIndex(101);
		}
		
		if (SettingManager.Instance.TutorialGoHome == 1 || SettingManager.Instance.TutorialSeq <= 21)
		{
			TuorialTriggerManager.Instance.OpenTutorialDialogByIndex(102);
		}
	}
	
	void Init()
	{
		Time.timeScale = 0;
		GameData.Instance.Pause = true;
		
		if (GameData.Instance.LevelType == LevelType.RushLevel)
		{
			if (GameData.Instance.CurrentLevel == SettingManager.Instance.NextLevel)
				if (directPassLevel != null)
					directPassLevel.SetActive(true);
			else
				if (directPassLevel != null)
					directPassLevel.SetActive(false);
			
			labelGold.text = (UIBattleSceneLogic.Instance.CurrentGoldValue* (1 + GameData.Instance.goldAdditionValue)).ToString();
			GameData.Instance.AddGold((int)(UIBattleSceneLogic.Instance.CurrentGoldValue*GameData.Instance.goldAdditionValue));
			
			if (SettingManager.Instance.NextLevel == GameData.Instance.CurrentLevel && s_success)
			{
				if (SettingManager.Instance.DiamondJiacheng != 1)
				{
					labelDiamond.text = "2";
					GameData.Instance.AddDiamond(2);
				}
				else
				{
					labelDiamond.text = "20";
					GameData.Instance.AddDiamond(20);
				}
				SettingManager.Instance.NextLevel+=1;
			}
			
			if (SettingManager.Instance.DiamondJiacheng != 1)
			{
				GameData.Instance.daojuDiamondAdditional = false;
			}
			else
			{
				SettingManager.Instance.DiamondJiacheng = 2;
				GameData.Instance.daojuDiamondAdditional = false;
			}
		}
		else
		{
			if (SettingManager.Instance.DiamondJiacheng == 1)
			{
				SettingManager.Instance.DiamondJiacheng = 2;
				GameData.Instance.daojuDiamondAdditional = false;
			}
			
			int totalGold = Mathf.FloorToInt(Mathf.Pow(UIBattleSceneLogic.Instance.CurrentGoldValue, 0.85f) * (1 + GameData.Instance.goldAdditionValue));
			labelGold.text = totalGold.ToString();
			GameData.Instance.AddGold(totalGold - UIBattleSceneLogic.Instance.CurrentGoldValue);
		}
		//		labelDiamond.text = UIBattleSceneLogic.Instance.CurrentDiamondValue.ToString();
		labelScore.text = UIBattleSceneLogic.Instance.CurrentScoreValue.ToString();
		
		SettingManager.Instance.HighestScore = Mathf.Max(SettingManager.Instance.HighestScore, UIBattleSceneLogic.Instance.CurrentScoreValue);
		
		StartCoroutine(FireEvent());
		
		#if UNITY_IPHONE
		// upload highest score LB
		if (UIBattleSceneLogic.Instance.CurrentScoreValue >= SettingManager.Instance.HighestScore)
			GameCenterManager.ReportScore(SettingManager.Instance.HighestScore, ConstData.LeaderBoardId);
		#endif
		
		//		GameData.Instance.AddGold(int.Parse(labelGold.text));
		//		GameData.Instance.AddDiamond(UIBattleSceneLogic.Instance.CurrentDiamondValue);
	}
	
	public void DirectPass()
	{
		if (SettingManager.Instance.TotalDiamond < 50)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}
		
		if (SettingManager.Instance.TotalTili < 1)
		{
			NotEnoughTiliDialog.Popup();
			return;	
		}
		
		GameData.Instance.AddDiamond(-50);
		MTAManager.DoEvent(MTAPoint.MINI_BUY_DIR_PASS);
		SettingManager.Instance.NextLevel+=1;
		GotoNextLevel();
	}
	
	public void GotoNextLevel()
	{
		//		if (SettingManager.Instance.TotalTili < 1)
		//		{
		//			NotEnoughTiliDialog.Popup();
		//			return;	
		//		}
		//
		//		Time.timeScale = 1.2f;
		//
		//		if (SettingManager.Instance.TotalTili == 20)
		//		{
		//			SettingManager.Instance.TiliRecoverTime = System.DateTime.Now.ToString();
		////			MainArea.Instance.StartRecoverTili();
		//		}
		//		GameData.Instance.AddTili(-1);
		
		GameData.Instance.GotoLevel(GameData.Instance.CurrentLevel + 1);
		DialogManager.Instance.CloseDialog();
		Camera.main.GetComponent<GrayscaleEffect>().enabled = false;
		GameData.Instance.LevelType = LevelType.RushLevel;
		GameData.Instance.Pause = false;
		
		GameData.Instance.goldDouble = false;
		GameData.Instance.bulletCapacity = 1f;
		Ultilities.CleanMemory();
		
		OtherDialog.Popup(Application.loadedLevelName);
		//		Application.LoadLevel(Application.loadedLevelName);
	}
	
	public void Retry()
	{
		if (SettingManager.Instance.TotalTili < 1)
		{
			NotEnoughTiliDialog.Popup();
			return;	
		}
		
		if (SettingManager.Instance.TotalTili == 20)
		{
			SettingManager.Instance.TiliRecoverTime = System.DateTime.Now.ToString();
			//			MainArea.Instance.StartRecoverTili();
		}
		GameData.Instance.AddTili(-1);
		GameData.Instance.goldDouble = false;
		GameData.Instance.bulletCapacity = 1f;
		
		Time.timeScale = 1.2f;
		GameData.Instance.Pause = false;
		GameData.Instance.CurrentWave = 0;
		if (GameData.Instance.LevelType == LevelType.RushLevel)
			GameData.Instance.GotoLevel(GameData.Instance.CurrentLevel);
		DialogManager.Instance.CloseDialog();
		Camera.main.GetComponent<GrayscaleEffect>().enabled = false;
		//		GameData.Instance.LevelType = LevelType.RushLevel;
		
		
		if (GameData.Instance.LevelType == LevelType.RushLevel)
		{
			SettingManager.Instance.AdvantageModeTime += 1;
			SettingManager.Instance.UseTiliNum += 1;
			Ultilities.CleanMemory();
			Application.LoadLevel(Application.loadedLevelName);
		}
		else
		{
			GameData.Instance.Reset();
			SettingManager.Instance.ChallegeModeTime += 1;
			SettingManager.Instance.UseTiliNum += 1;
			Ultilities.CleanMemory();
			Application.LoadLevel(Application.loadedLevelName);
		}
		
		SettingManager.Instance.TutorialRetry = 0;
	}
	
	public void GotoHome()
	{
		Time.timeScale = 1.2f;
		DialogManager.Instance.CloseDialog();
		GameData.Instance.Pause = false;
		GameData.Instance.bulletCapacity = 1f;
		GameData.Instance.goldDouble = false;
		//		WeaponManager.selectedWeapons.Clear();
		Camera.main.GetComponent<GrayscaleEffect>().enabled = false;
		//		Ultilities.CleanMemory();
		SettingManager.Instance.TutorialGoHome = 0;
		Application.LoadLevel("ui");
	}
}
