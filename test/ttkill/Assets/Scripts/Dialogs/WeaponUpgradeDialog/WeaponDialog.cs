//#define US_VERSION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponDialog : DialogBase {
	public GameObject upgradeEffectPrefab;
	public GameObject btnUpgrade;
	public GameObject btnActive;
	public GameObject lv;
	public GameObject maxLv;
	public UISprite   spriteActive;
	public UISprite   spriteUpgrade;
	public UILabel 	  labelActive;
	public UILabel    labelUpgrade;
	public UILabel    labelCurrentLevel;
	public UILabel    labelCurrentAttack;
	public UILabel    labelCurrentCapacity;
	public UIProgressBar progressAttack;
	public UIProgressBar progressCapacity;
	public Transform moveArea;
	
	private int currentIndex = 0;
	private WeaponType currentWeaponType = 0;
	public static void Popup()
	{
		GameObject weapon = GameObject.FindGameObjectWithTag("3DWeapon");
		int childCount = weapon.transform.childCount;
		for(int i = 0; i < childCount; i++)
		{
			weapon.transform.GetChild(i).gameObject.SetActive(true);
		}
		
		#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/wuqishengjiDlg_us");
		#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/wuqishengjiDlg");
		#endif
	}
	
	public static void Close()
	{
		UILayout.Instance.BottomIn();
		
		GameObject weapon = GameObject.FindGameObjectWithTag("3DWeapon");
		int childCount = weapon.transform.childCount;
		for(int i = 0; i < childCount; i++)
		{
			weapon.transform.GetChild(i).gameObject.SetActive(false);
		}
		
		DialogManager.Instance.CloseDialog();
	}
	
	public void CloseDialog()
	{
		Close();
		MainArea.Instance.Show();
	}
	
	void UpdateUI(WeaponType wt)
	{
		GunInfo info = IOHelper.GetGunInfoById((int)wt);
		int level = WeaponDB.Instance.GetWeaponLvById((int)wt);
		int tmpLevel = Mathf.Max(1, level);
		Debug.Log("tmpLevel " + tmpLevel + " (int)wt " + (int)wt);
		GunUpgradeInfo upgradeInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)wt, tmpLevel);
		GunUpgradeInfo nextUpgradeInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)wt, tmpLevel + 1);
		GunUpgradeInfo maxInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)wt, info.maxlv);
		
		if (level == 0 && upgradeInfo.cost != 0)
		{
			btnActive.SetActive(true);
			labelActive.text = upgradeInfo.cost.ToString();
			spriteActive.spriteName = upgradeInfo.type == 1 ? "jinbi" : "zuanshi";
			btnUpgrade.SetActive(false);
		}
		else
		{
			btnActive.SetActive(false);
			btnUpgrade.SetActive(true);
			if (nextUpgradeInfo != null)
				labelUpgrade.text = nextUpgradeInfo.cost.ToString();
			else
				labelUpgrade.text = "max";
			if (nextUpgradeInfo != null)
				spriteUpgrade.spriteName = nextUpgradeInfo.type == 1 ? "jinbi" : "zuanshi";
		}
		
		WeaponItem[] items = moveArea.GetComponentsInChildren<WeaponItem>();
		foreach(WeaponItem item in items)
		{
			if (item.type == wt)
			{
				item.UpdateUI();
				break;
			}
		}
		
		//		if (level < info.maxlv)
		//		{
		//			lv.SetActive(true);
		//			maxLv.SetActive(false);
		//			labelCurrentLevel.text = tmpLevel.ToString();
		//		}
		//		else
		//		{
		//			lv.SetActive(false);
		//			maxLv.SetActive(true);
		//		}
		//		labelCurrentAttack.text = upgradeInfo.atk.ToString();
		//		labelCurrentCapacity.text = upgradeInfo.capacity.ToString();
		//
		//		progressAttack.value = (float)upgradeInfo.atk / (float)maxInfo.atk;
		//		progressCapacity.value = (float)upgradeInfo.capacity / (float)maxInfo.capacity;
	}
	
	public void NextWeapon()
	{
		//		int mod = WeaponUpgrade.Instance.weapons.Count;
		if (currentIndex >= WeaponUpgrade.Instance.weapons.Count - 1)
		{
			return;
		}
		
		currentIndex = currentIndex + 1;
		
		TweenPosition.Begin(moveArea.gameObject, 1, new Vector3(-currentIndex * 1000f, 0, 0));
		//		currentIndex = currentIndex % mod;
		
		currentWeaponType = WeaponUpgrade.Instance.UpdateUI(currentIndex);
		UpdateUI(currentWeaponType);
	}
	
	public void PreviousWeapon()
	{
		//		int mod = WeaponUpgrade.Instance.weapons.Count;
		if (currentIndex <= 0)
			return;
		
		currentIndex = currentIndex - 1;
		TweenPosition.Begin(moveArea.gameObject, 1, new Vector3(-currentIndex * 1000f, 0, 0));
		
		currentWeaponType = WeaponUpgrade.Instance.UpdateUI(currentIndex);
		UpdateUI(currentWeaponType);
	}
	
	IEnumerator Start()
	{
		yield return new WaitForSeconds(0.2f);
		TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
		currentWeaponType = WeaponUpgrade.Instance.UpdateUI(currentIndex);
		UpdateUI(currentWeaponType);
	}
	
	private void MTABuyPoint()
	{
		switch(currentWeaponType)
		{
		case WeaponType.dianju:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DIANJU);
			break;
		case WeaponType.gundouble_fire:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DOUBLE_FIRE);
			break;
		case WeaponType.gun_fire:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_FIRE);
			break;
		case WeaponType.gundouble_liudan:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DOUBLE_LIUDAN);
			break;
		case WeaponType.gundouble_m4:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DOUBLE_M4);
			break;
		case WeaponType.gundouble_sandan:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DOUBLE_SANDAN);
			break;
		case WeaponType.gundouble_shouqiang:
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DOUBLE_SHOUQIANG);
			break;
		}
	}
	
	private void MTAUpgradePoint()
	{
		switch(currentWeaponType)
		{
		case WeaponType.dao:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_DAO);
			break;
		case WeaponType.gun_liudan:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_LIUDAN);
			break;
		case WeaponType.gun_m4:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_M4);
			break;
		case WeaponType.gun_sandan:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_SANDAN);
			break;
		case WeaponType.gun_shouqiang:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_SHOUQIANG);
			break;
		case WeaponType.dianju:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_DIANJU);
			break;
		case WeaponType.gundouble_fire:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_DOUBLE_FIRE);
			break;
		case WeaponType.gun_fire:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_FIRE);
			break;
		case WeaponType.gundouble_liudan:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_DOUBLE_LIUDAN);
			break;
		case WeaponType.gundouble_m4:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_DOUBLE_M4);
			break;
		case WeaponType.gundouble_sandan:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_DOUBLE_SANDAN);
			break;
		case WeaponType.gundouble_shouqiang:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_DOUBLE_SHOUQIANG);
			break;
		}
	}
	
	IEnumerator FireTrigger()
	{
		yield return new WaitForSeconds(0.2f);
		
	}
	
	public void OnBuy()
	{
		int level = WeaponDB.Instance.GetWeaponLvById((int)currentWeaponType);
		int tmpLevel = Mathf.Max(level, 1);
		GunUpgradeInfo nextUpgradeInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)currentWeaponType, tmpLevel);
		
		if (nextUpgradeInfo.type == 1 && nextUpgradeInfo.cost <= GameData.Instance.currentGold)
		{
			SettingManager.Instance.GunNum+=1;
			GameData.Instance.AddGold(-nextUpgradeInfo.cost);
			WeaponDB.Instance.UpdateWeapon((int)currentWeaponType, level + 1);
			UpdateUI(currentWeaponType);
			MTABuyPoint();
			GameObject effect = (GameObject)Instantiate(upgradeEffectPrefab);
			effect.transform.parent = GameObject.FindGameObjectWithTag("3DWeapon").transform;
			effect.transform.localPosition = new Vector3(0, -2, 0);
			StartCoroutine(FireTrigger());
		}
		else if (nextUpgradeInfo.type == 2 && nextUpgradeInfo.cost <= GameData.Instance.currentDiamond)
		{
			SettingManager.Instance.GunNum+=1;
			GameData.Instance.AddDiamond(-nextUpgradeInfo.cost);
			WeaponDB.Instance.UpdateWeapon((int)currentWeaponType, level + 1);
			UpdateUI(currentWeaponType);
			MTABuyPoint();
			GameObject effect = (GameObject)Instantiate(upgradeEffectPrefab);
			effect.transform.parent = GameObject.FindGameObjectWithTag("3DWeapon").transform;
			effect.transform.localPosition = new Vector3(0, -2, 0);
			StartCoroutine(FireTrigger());
		}
		else
		{
			if (nextUpgradeInfo.type == 1)
			{
				NotEnoughGoldDialog.Popup();
			}
			else if (nextUpgradeInfo.type == 2)
			{
				NotEnoughDiamondDialog.Popup();
			}
			Debug.LogWarning("resource is not enough!");
		}
	}
	
	public void OnUpgrade()
	{
		GunInfo info = IOHelper.GetGunInfoById((int)currentWeaponType);
		
		int level = WeaponDB.Instance.GetWeaponLvById((int)currentWeaponType);
		
		if (level >= info.maxlv)
			return;
		
		GunUpgradeInfo nextUpgradeInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)currentWeaponType, level + 1);
		
		if (nextUpgradeInfo.type == 1 && nextUpgradeInfo.cost <= GameData.Instance.currentGold)
		{
			SettingManager.Instance.WeaponUpgrade += 1;
			GameData.Instance.AddGold(-nextUpgradeInfo.cost);
			WeaponDB.Instance.UpdateWeapon((int)currentWeaponType, level + 1);
			SettingManager.Instance.MaxWeaponLevel = Mathf.Max(level+1, SettingManager.Instance.MaxWeaponLevel);
			UpdateUI(currentWeaponType);
			MTAUpgradePoint();
			GameObject effect = (GameObject)Instantiate(upgradeEffectPrefab);
			effect.transform.parent = GameObject.FindGameObjectWithTag("3DWeapon").transform;
			effect.transform.localPosition = new Vector3(0, -1.5f, 0);
			StartCoroutine(FireTrigger());
		}
		else if (nextUpgradeInfo.type == 2 && nextUpgradeInfo.cost <= GameData.Instance.currentDiamond)
		{
			SettingManager.Instance.MaxWeaponLevel = Mathf.Max(level+1, SettingManager.Instance.MaxWeaponLevel);
			SettingManager.Instance.WeaponUpgrade += 1;
			GameData.Instance.AddDiamond(-nextUpgradeInfo.cost);
			WeaponDB.Instance.UpdateWeapon((int)currentWeaponType, level + 1);
			UpdateUI(currentWeaponType);
			MTAUpgradePoint();
			GameObject effect = (GameObject)Instantiate(upgradeEffectPrefab);
			effect.transform.parent = GameObject.FindGameObjectWithTag("3DWeapon").transform;
			effect.transform.localPosition = new Vector3(0, -1.5f, 0);
			StartCoroutine(FireTrigger());
		}
		else
		{
			if (nextUpgradeInfo.type == 1)
			{
				NotEnoughGoldDialog.Popup();
			}
			else if (nextUpgradeInfo.type == 2)
			{
				NotEnoughDiamondDialog.Popup();
			}
			Debug.LogWarning("resource is not enough!");
		}
		
	}
}
