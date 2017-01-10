using UnityEngine;
using System.Collections;

public class PetUpgradeItem : MonoBehaviour {
	public PetType type;
	public UILabel currentDes;
	public UILabel nextDes;
	public GameObject btnUpgrade;
	public GameObject btnBuy;
	public GameObject btnBattle;
	public GameObject alreadyBattle;
	public GameObject maxLevelObj;
	public UILabel labelLv;
	public UILabel labelCost;

	private int currentLevel;
	private bool onBattle;
	int cost = 0;
	// Use this for initialization
	void Start () {
		EventService.Instance.GetEvent<PetOnBattleEvent>().Subscribe(UpdateUI);
		UpdateUI();
	}

	void UpdateUI()
	{
		currentLevel = PetDB.Instance.GetPetLvById((int)type);
		onBattle = PetDB.Instance.GetPetOnBattleById((int)type) != 0;

		if (onBattle)
		{
			alreadyBattle.SetActive(true);
			btnBattle.SetActive(false);
		}
		else
		{
			alreadyBattle.SetActive(false);
			btnBattle.SetActive(true);
		}


		if (currentLevel == 0)
		{
			currentLevel  = 1;
			cost = IOHelper.GetPetInfoByIdAndLevel((int)type, currentLevel).cost;
			btnBuy.SetActive(true);
			btnBattle.collider.enabled = false;
			btnUpgrade.SetActive(false);
		}
		else
		{
			btnBuy.SetActive(false);
			btnUpgrade.SetActive(true);
			btnBattle.collider.enabled = true;
		}
		
		if (currentLevel >= 10)
		{
			maxLevelObj.SetActive(true);
			btnUpgrade.SetActive(false);
			btnBuy.SetActive(false);
		}
		else
		{
			maxLevelObj.SetActive(false);
			
//			if (cost == 0)
				cost = IOHelper.GetPetInfoByIdAndLevel((int)type, currentLevel+1).cost;
		}

		if (labelLv != null)
			labelLv.text = currentLevel.ToString();

		if (labelCost != null)
			labelCost.text = cost.ToString();

		if (currentDes != null)
			currentDes.text = IOHelper.GetPetInfoByIdAndLevel((int)type, currentLevel).desc;
		
		if (nextDes != null && currentLevel < 10)
			nextDes.text = IOHelper.GetPetInfoByIdAndLevel((int)type, currentLevel + 1).desc;
		else
			nextDes.text = "已到最高等级";
	}

	void MTAUpgradePoint()
	{
		switch(type)
		{
		case PetType.songshu:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_PET_1);
			break;
		case PetType.tuzi:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_PET_2);
			break;
		case PetType.pet3:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_PET_3);
			break;
		case PetType.pet4:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_PET_4);
			break;
		case PetType.pet5:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_PET_5);
			break;
		case PetType.pet6:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_PET_6);
			break;
		case PetType.pet7:
			MTAManager.DoEvent(MTAPoint.MINI_UPGRADE_PET_7);
			break;
		}
	}

	void MTAUsePoint()
	{
		switch(type)
		{
		case PetType.songshu:
			MTAManager.DoEvent(MTAPoint.MINI_USE_PET_1);
			break;
		case PetType.tuzi:
			MTAManager.DoEvent(MTAPoint.MINI_USE_PET_2);
			break;
		case PetType.pet3:
			MTAManager.DoEvent(MTAPoint.MINI_USE_PET_3);
			break;
		case PetType.pet4:
			MTAManager.DoEvent(MTAPoint.MINI_USE_PET_4);
			break;
		case PetType.pet5:
			MTAManager.DoEvent(MTAPoint.MINI_USE_PET_5);
			break;
		case PetType.pet6:
			MTAManager.DoEvent(MTAPoint.MINI_USE_PET_6);
			break;
		case PetType.pet7:
			MTAManager.DoEvent(MTAPoint.MINI_USE_PET_7);
			break;
		}
	}

	public void onUpgrade()
	{
		if (currentLevel >= 10)
			return;
		
		if (GameData.Instance.currentGold < cost)
		{
			NotEnoughGoldDialog.Popup();
			return;
		}
		
		GameData.Instance.AddGold(-cost);
		MTAUpgradePoint();
		if (currentLevel == 1 && cost == IOHelper.GetPetInfoByIdAndLevel((int)type,currentLevel).cost)
		{
			PetDB.Instance.UpdatePet((int)type, 1, onBattle ? 1 : 0);
		}
		else
		{
			PetDB.Instance.UpdatePet((int)type, currentLevel + 1, onBattle ? 1 : 0);
		}
		
//		UpdateUpArea(upArea.type);
		UpdateUI();
		
//		Vector3 uiPos = NGUICamera.WorldToScreenPoint(TopSpawn.position) + new Vector3(0, 0, 1);
//		uiPos = WorldCamera.ScreenToWorldPoint(uiPos);
//		
//		GameObject effect = (GameObject)Instantiate(upgradeEffectPrefab);
//		effect.transform.parent = upPetArea;
	}

	public void onBuy()
	{
		PetInfo info = IOHelper.GetPetInfoByIdAndLevel((int)type,1);
		if (info != null)
			UnlockPetDialog.PopUp(info.cost, (int)type, UpdateUI);
	}

	public void OnBattle()
	{
		onBattle = true;
		PetDB.Instance.UpdatePet((int)PetListDialog.oldType, PetDB.Instance.GetPetLvById((int)PetListDialog.oldType), 0);
		PetDB.Instance.UpdatePet((int)type, currentLevel, onBattle ? 1 : 0);
		PetListDialog.oldType = type;
		UpdateUI();
		MTAUsePoint();
		GetComponentInParent<PetListDialog>().onBattleChange();
		EventService.Instance.GetEvent<PetOnBattleEvent>().Publish();

	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<PetOnBattleEvent>().Unsubscribe(UpdateUI);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
