using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData {
	public int currentGold = 1000;
	public int currentDiamond = 1000;
	public int currentTili = 20;

	public int currentBindongCount = 0;
	public int currentAoeCount = 0;
	public int currentJiaxueCount = 0;
	public int currentJiatelinCount = 0;

	public float goldAdditionValue = 0;
	public int DeadTime = 0;
	public bool Pause = false;
	public bool Wudi = false;
	public float bulletCapacity = 1f;
	public List<WeaponType> selectedWeapons = new List<WeaponType>();
	public bool goldDouble = false;
	public bool daojuDiamondAdditional = false;
	private static GameData instance = null;

	public static GameData Instance
	{
		get
		{
			if (instance == null)
				instance = new GameData();

			return instance;
		}
	}

	private GameData()
	{
		currentWave = 0;
		currentStage = 1;
		currentGold = SettingManager.Instance.TotalGold;
		currentDiamond = SettingManager.Instance.TotalDiamond;
		currentTili = SettingManager.Instance.TotalTili;
		
		currentJiatelinCount = SettingManager.Instance.TotalJiatelin;
		currentJiaxueCount = SettingManager.Instance.TotalJiaxue;
		currentBindongCount = SettingManager.Instance.TotalBindong;
		currentAoeCount = SettingManager.Instance.TotalAoe;
	}

	public void Init()
	{
		Reset();
	}

	#region Properties
	private int currentWave;
	public int CurrentWave
	{
		get
		{
			return currentWave;
		}
		set
		{
			currentWave = value;
		}
	}

	private int currentStage = 1;
	public int CurrentStage
	{
		get
		{
			return currentStage;
		}
	}

	private int currentLevel = 1;
	public int CurrentLevel
	{
		get
		{
			return currentLevel = SettingManager.Instance.CurrentLevel;
		}
	}
	
	public int EnemyAverageHP
	{
		get
		{
			if (GameData.instance.LevelType == LevelType.RushLevel)
				return Mathf.FloorToInt(8f * (1000f + currentLevel * IOHelper.GetStageInfoByStageId(currentLevel).difficult * 10) / (15) / 1.4f);
			else// if (GameData.instance.LevelType == LevelType.InfiniteLevel)
				return Mathf.FloorToInt(8f * (1000f + IOHelper.GetStageInfiniteInfoByStageId(currentStage).level * 10) / (15) / 1.4f);
		}
	}

	public LevelType LevelType
	{
		get;
		set;
	}
	#endregion

	#region public methods
	public void Reset()
	{
		currentWave = 0;
		currentStage = 1;
		currentGold = SettingManager.Instance.TotalGold;
		currentDiamond = SettingManager.Instance.TotalDiamond;
		currentTili = SettingManager.Instance.TotalTili;

		currentJiatelinCount = SettingManager.Instance.TotalJiatelin;
		currentJiaxueCount = SettingManager.Instance.TotalJiaxue;
		currentBindongCount = SettingManager.Instance.TotalBindong;
		currentAoeCount = SettingManager.Instance.TotalAoe;

		if (UIBattleSceneLogic.Instance != null)
			UIBattleSceneLogic.Instance.Reset();
	}

	public void ChangeCurrentWave(bool increased)
	{
		currentWave += increased ? 1 : -1;
	}

	public void ChangeCurrentStage(bool increased)
	{
		currentWave = 0;
		currentStage += increased ? 1: -1;
	}

	public void GotoLevel(int level)
	{
		currentLevel = level;
		SettingManager.Instance.CurrentLevel = currentLevel;
		Reset();
	}

	public void AddGold(int amount)
	{
		if (amount < 0)
		{
			SettingManager.Instance.CostGoldNum_Shuaxin -= amount;
			currentGold += amount;
		}
		else
		{
			if (goldDouble)
				amount *= 2;
			else
				amount = amount + (int)(amount * SettingManager.Instance.GoldAddtional * 0.5f);
			SettingManager.Instance.GoldGot_Shuaxin += amount;
			SettingManager.Instance.TotalGoldGot += amount;
			currentGold += amount;
		}

		SettingManager.Instance.TotalGold = currentGold;
		EventService.Instance.GetEvent<GoldChangeEvent>().Publish(currentGold);
	}

	public void AddDiamond(int amount)
	{
		currentDiamond += amount;
		SettingManager.Instance.TotalDiamond = currentDiamond;
		EventService.Instance.GetEvent<DiamondChangeEvent>().Publish(currentDiamond);
	}

	public void AddTili(int tili)
	{
		currentTili += tili;
		SettingManager.Instance.TotalTili = currentTili;
		EventService.Instance.GetEvent<TiliChangeEvent>().Publish(currentTili);
	}

	public void AddAoe(int aoe)
	{
		currentAoeCount += aoe;
		SettingManager.Instance.TotalAoe = currentAoeCount;
	}

	public void AddBindong(int bindong)
	{
		currentBindongCount += bindong;
		SettingManager.Instance.TotalBindong = currentBindongCount;
	}

	public void AddJiatelin(int jiatelin)
	{
		currentJiatelinCount += jiatelin;
		SettingManager.Instance.TotalJiatelin = currentJiatelinCount;
	}

	public void AddJiaxue(int jiaxue)
	{
		currentJiaxueCount += jiaxue;
		SettingManager.Instance.TotalJiaxue = currentJiaxueCount;
	}
	#endregion
}
