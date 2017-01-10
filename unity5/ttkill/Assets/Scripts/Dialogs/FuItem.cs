//#define US_VERSION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuItem : MonoBehaviour {

	public enum Type { Speed = 1, Call = 4, Restore = 2, Fury = 3 }

	public Type type;

	public UIProgressBar slider;

	public UILabel description;

	public UILabel costLabel;

	public GameObject btnMaxLevel;
	public GameObject btnUpgrade;

	int currentLevel;

	// Use this for initialization
	void Start ()
	{
		UpdateUI();

		StartCoroutine(FireTrigger());
	}

	void UpdateUI()
	{
		currentLevel = IOHelper.GetCurrentFuLv(type);
		
		var f1 = IOHelper.GetFuInfo(type, currentLevel);
		var f2 = IOHelper.GetFuInfo(type, currentLevel + 1);
		
		if (currentLevel == IOHelper.GetFuMaxLv(type))
		{
			btnMaxLevel.SetActive(true);
			btnUpgrade.SetActive(false);
		}
		else
		{
			btnMaxLevel.SetActive(false);
			btnUpgrade.SetActive(true);
		}
		
		#if US_VERSION
		description.text = (f1 == null ? string.Empty : f1.desc) + (f2 == null ? string.Empty : ("\n[10aee6]Next Level\n" + f2.desc));
		#else
		description.text = (f1 == null ? string.Empty : f1.desc) + (f2 == null ? string.Empty : ("\n[10aee6]下一等级\n" + f2.desc));
		#endif
		costLabel.text = f1.cost.ToString();
		
		slider.value = (float)currentLevel / IOHelper.GetFuMaxLv(type);
	}

	IEnumerator FireTrigger()
	{
		yield return new WaitForSeconds(0.2f);
		TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
	}

	void MTAUpgradePoint()
	{
		switch(type)
		{
		case Type.Call:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_FU_ZHAOHUAN);
			break;
		case Type.Fury:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_FU_KUANGBAO);
			break;
		case Type.Restore:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_FU_HUIFU);
			break;
		case Type.Speed:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_FU_SPEED);
			break;
		}
	}

	public void OnLvUpClick()
	{
		if (IOHelper.GetFuInfo(type, currentLevel + 1) != null)
		{
			if (IOHelper.GetFuInfo(type, currentLevel).cost > SettingManager.Instance.TotalGold)
			{
				NotEnoughGoldDialog.Popup();
				return;
			}

			GameData.Instance.AddGold(-IOHelper.GetFuInfo(type, currentLevel).cost);
			IOHelper.SetCurrentFuLv(type, currentLevel + 1);
			MTAUpgradePoint();
//			Start();
			UpdateUI();
		}
	}

}
