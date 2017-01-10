//#define US_VERSION

using UnityEngine;
using System.Collections;
using System;

public class PetArea
{
	public PetType type;
	public int currentLevel;
	public int Cost;
	public string currentLevelDes;
	public string nextLevelDes;
	public int onBattle;
	public PetType oldType;
	public int oldCurrentLevel;

	public void Reset()
	{
		type = PetType.none;
		oldType = PetType.none;
		currentLevel = 0;
		Cost = 0;
		currentLevelDes = string.Empty;
		nextLevelDes = string.Empty;
		onBattle = 0;
		oldCurrentLevel = 0;
	}
}

public class PetDialog : DialogBase
{
	public GameObject petSelection;
	public GameObject btnEquip_up;
	public GameObject btnChange_up;
	public GameObject btnEquip_down;
	public GameObject btnChange_down;
	public Transform TopSpawn;
	public Transform DownSpawn;

	public UILabel labelUPLevel;
	public UILabel labelDownLevel;
	public UILabel labelUpCurDes;
	public UILabel labelDownCurDes;
	public UILabel labelUpNextDes;
	public UILabel labelDownNextDes;
	public UILabel labelUpCost;
	public UILabel labelDownCost;
	public GameObject upgradeEffectPrefab;
	public GameObject lockObj;

	public GameObject upBtn;
	public GameObject downBtn;
	public GameObject upMaxLevelObj;
	public GameObject downMaxLevelObj;

	private Camera NGUICamera;
	private static Camera WorldCamera;
	private Transform upPetArea;
	private Transform downPetArea;
	private PetArea upArea;
	private PetArea downArea;

	private int petPos = 0; //0: no where;  1: up pos;  2: down pos

	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/chongwushengji_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/chongwushengji");
#endif
		GameObject pet = GameObject.FindGameObjectWithTag("3DPet");
		int childCount = pet.transform.childCount;
		for(int i = 0; i < childCount; i++)
		{
			pet.transform.GetChild(i).gameObject.SetActive(true);
		}

		WorldCamera = pet.GetComponentInChildren<Camera>();
	}

	// Use this for initialization
	void Start () {
		NGUICamera = GetComponentInParent<Camera>();
		upPetArea = WorldCamera.transform.FindChild("up");
		downPetArea = WorldCamera.transform.FindChild("down");
		upArea = new PetArea();
		upArea.Reset();
		downArea = new PetArea();
		downArea.Reset();
		UpdateUI();
		UpdateBottomArea();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UnLockPetPos()
	{
		UnlockPetPositionDialog.PopUp(80, 2, UpdateBottomArea);
	}

	void UpdateBottomArea()
	{
		if (SettingManager.Instance.PetLocked == 1)
		{
			Debug.Log("aaaaaaa");
			lockObj.SetActive(true);
			btnChange_down.collider.enabled = false;
		}
		else
		{
			lockObj.SetActive(false);
			btnChange_down.collider.enabled = true;
		}
	}

	void UpdateUI()
	{
		Array petArray = Enum.GetValues(typeof(PetType));
		int i = 0;
		foreach(var p in petArray)
		{
			if ((PetType)p == PetType.none)
				continue;
			int level = PetDB.Instance.GetPetLvById((int)p);
			int onBattle = PetDB.Instance.GetPetOnBattleById((int)p);

			if (onBattle == 1)
			{
				if (i == 0)
				{
					petPos = 1;

					OnPetSelect((PetType)p);
					i++;
				}
				else if (i==1)
				{
					petPos = 2;

					OnPetSelect((PetType)p);
					i++;
				}
			}
		}
		petPos = 0;
		EventService.Instance.GetEvent<PetSelectionEvent>().Publish();
	}

	void OnDestroy()
	{
		GameObject pet = GameObject.FindGameObjectWithTag("3DPet");
		if (pet != null)
		{
			int childCount = pet.transform.childCount;
			for(int i = 0; i < childCount; i++)
			{
				pet.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	public void MoveIn_up()
	{
//		if (petSelection.transform.position != Vector3.zero)
		{
			btnChange_up.SetActive(false);
			btnEquip_up.SetActive(true);
			petPos = 1;
//			TweenPosition.Begin(petSelection, 0.5f, Vector3.zero);
//			TweenScale.Begin(petSelection, 0.5f, Vector3.one);
//			TweenAlpha.Begin(petSelection, 0.3f, 1);
		}
	}

	public void MoveOut_up()
	{
//		if (petSelection.transform.position != new Vector3(-200, 0, 0))
		{
			btnChange_up.SetActive(true);
			btnEquip_up.SetActive(false);
			petPos = 0;
			if (upArea.oldType != PetType.none)
				PetDB.Instance.UpdatePet((int)upArea.oldType, upArea.oldCurrentLevel, 0);
			if (upArea.type != PetType.none)
				PetDB.Instance.UpdatePet((int)upArea.type, upArea.currentLevel, 1);
			EventService.Instance.GetEvent<PetSelectionEvent>().Publish();
//			TweenPosition.Begin(petSelection, 0.5f, new Vector3(165, 0, 0));
//			TweenScale.Begin(petSelection, 0.5f, Vector3.one * 0.8f);
//			TweenAlpha.Begin(petSelection, 0.3f, 0);
		}
	}

	public void MoveIn_down()
	{
//		if (petSelection.transform.position != Vector3.zero)
		{
			petPos = 2;
			btnChange_down.SetActive(false);
			btnEquip_down.SetActive(true);
//			TweenPosition.Begin(petSelection, 0.5f, Vector3.zero);
//			TweenScale.Begin(petSelection, 0.5f, Vector3.one);
//			TweenAlpha.Begin(petSelection, 0.3f, 1);
		}
	}
	
	public void MoveOut_down()
	{
//		if (petSelection.transform.position != new Vector3(-200, 0, 0))
		{
			petPos = 0;
			if (downArea.oldType != PetType.none)
				PetDB.Instance.UpdatePet((int)downArea.oldType, downArea.oldCurrentLevel, 0);

			btnEquip_down.SetActive(false);
			btnChange_down.SetActive(true);
			if (downArea.type != PetType.none)
				PetDB.Instance.UpdatePet((int)downArea.type, downArea.currentLevel, 1);
			EventService.Instance.GetEvent<PetSelectionEvent>().Publish();
//			TweenPosition.Begin(petSelection, 0.5f, new Vector3(165f, 0, 0));
//			TweenScale.Begin(petSelection, 0.5f, Vector3.one * 0.8f);
//			TweenAlpha.Begin(petSelection, 0.3f, 0);
		}
	}

	private GameObject GeneratePet(PetType type, Vector3 pos)
	{
		GameObject objPet = null;
		switch(type)
		{
		case PetType.songshu:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet_songshu"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.06f;
			break;
		case PetType.tuzi:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/Enemy/tuzi"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.06f;
			break;
		case PetType.pet3:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet03"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.06f;
			break;
		case PetType.pet4:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet04"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.06f;
			break;
		case PetType.pet5:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet05"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.06f;
			break;
		case PetType.pet6:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet06"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.06f;
			break;
		case PetType.pet7:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet07"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.06f;
			break;
		}

		return objPet;
	}

	void UpdateUpArea(PetType type)
	{
		int count = upPetArea.childCount - 1;
		while(count >= 0)
		{
			upArea.oldType = upArea.type;
			upArea.oldCurrentLevel = upArea.currentLevel;
			Destroy(upPetArea.GetChild(count).gameObject);
			count--;
		}
		upArea.currentLevel = PetDB.Instance.GetPetLvById((int)type);
		upArea.onBattle = PetDB.Instance.GetPetOnBattleById((int)type);
		upArea.type = type;
		int cost = 0;
		if (upArea.currentLevel == 0)
		{
			upArea.currentLevel = 1;
			cost = IOHelper.GetPetInfoByIdAndLevel((int)type, upArea.currentLevel).cost;
		}

		if (upArea.currentLevel >= 10)
		{
			upMaxLevelObj.SetActive(true);
			upBtn.SetActive(false);
		}
		else
		{
			upMaxLevelObj.SetActive(false);
			upBtn.SetActive(true);

			if (cost == 0)
				cost = IOHelper.GetPetInfoByIdAndLevel((int)type, upArea.currentLevel+1).cost;
			upArea.Cost = cost;
		}
		Vector3 uiPos = NGUICamera.WorldToScreenPoint(TopSpawn.position) + new Vector3(0, 0, 1);
		
		GameObject objPet = GeneratePet(type, WorldCamera.ScreenToWorldPoint(uiPos));

		if (labelUPLevel != null)
			labelUPLevel.text = "Lv." + upArea.currentLevel;

		if (labelUpCost != null)
			labelUpCost.text = upArea.Cost.ToString();

		if (labelUpCurDes != null)
			labelUpCurDes.text = IOHelper.GetPetInfoByIdAndLevel((int)type, upArea.currentLevel).desc;

		if (labelUpNextDes != null && upArea.currentLevel < 10)
			labelUpNextDes.text = IOHelper.GetPetInfoByIdAndLevel((int)type, upArea.currentLevel + 1).desc;
		else
			labelUpNextDes.text = "已到最高等级";

		objPet.transform.parent = upPetArea;
	}

	public void UpdateDownArea(PetType type)
	{
		int count = downPetArea.childCount - 1;
		while(count >= 0)
		{
			downArea.oldType = downArea.type;
			downArea.oldCurrentLevel = downArea.currentLevel;
			Destroy(downPetArea.GetChild(count).gameObject);
			count--;
		}
		
		downArea.currentLevel = PetDB.Instance.GetPetLvById((int)type);
		downArea.onBattle = PetDB.Instance.GetPetOnBattleById((int)type);
		downArea.type = type;
		int cost = 0;
		if (downArea.currentLevel == 0)
		{
			downArea.currentLevel  = 1;
			cost = IOHelper.GetPetInfoByIdAndLevel((int)type, downArea.currentLevel).cost;
		}

		if (downArea.currentLevel >= 10)
		{
			downMaxLevelObj.SetActive(true);
			downBtn.SetActive(false);
		}
		else
		{
			downMaxLevelObj.SetActive(false);
			downBtn.SetActive(true);

			if (cost == 0)
				cost = IOHelper.GetPetInfoByIdAndLevel((int)type, downArea.currentLevel+1).cost;
			downArea.Cost = cost;
		}
		Vector3 uiPos = NGUICamera.WorldToScreenPoint(DownSpawn.position) + new Vector3(0, 0, 1);

		GameObject objPet = GeneratePet(type, WorldCamera.ScreenToWorldPoint(uiPos));
		if (labelDownLevel != null)
			labelDownLevel.text = "Lv." + downArea.currentLevel;
		
		if (labelDownCost != null)
			labelDownCost.text = downArea.Cost.ToString();
		
		if (labelDownCurDes != null)
			labelDownCurDes.text = IOHelper.GetPetInfoByIdAndLevel((int)type, downArea.currentLevel).desc;
		
		if (labelDownNextDes != null && downArea.currentLevel < 10)
			labelDownNextDes.text = IOHelper.GetPetInfoByIdAndLevel((int)type, downArea.currentLevel + 1).desc;
		else
			labelDownNextDes.text = "已到最高等级";


		objPet.transform.parent = downPetArea;
	}

	public void OnPetSelect(PetType type)
	{
		if (petPos == 0)
			return;

		if (petPos == 1)
		{
			UpdateUpArea(type);
		}
		else if (petPos == 2)
		{
			UpdateDownArea(type);
		}
	}

	public void OnUpAreaUpgrade()
	{
		if (upArea.currentLevel >= 10)
			return;

		if (GameData.Instance.currentGold < upArea.Cost)
		{
			NotEnoughGoldDialog.Popup();
			return;
		}

		GameData.Instance.AddGold(-upArea.Cost);

		if (upArea.currentLevel == 1 && upArea.Cost == IOHelper.GetPetInfoByIdAndLevel((int)upArea.type,upArea.currentLevel).cost)
		{
			PetDB.Instance.UpdatePet((int)upArea.type, 1, upArea.onBattle);
		}
		else
		{
			PetDB.Instance.UpdatePet((int)upArea.type, upArea.currentLevel + 1, upArea.onBattle);
		}

		UpdateUpArea(upArea.type);

		Vector3 uiPos = NGUICamera.WorldToScreenPoint(TopSpawn.position) + new Vector3(0, 0, 1);
		uiPos = WorldCamera.ScreenToWorldPoint(uiPos);

		GameObject effect = (GameObject)Instantiate(upgradeEffectPrefab);
		effect.transform.parent = upPetArea;
		effect.transform.localPosition = new Vector3(-2.6f, 0, 11f);
	}

	public void OnDownAreaUpgrade()
	{
		if (downArea.currentLevel >= 10)
			return;

		if (GameData.Instance.currentGold < downArea.Cost)
		{
			NotEnoughGoldDialog.Popup();
			return;
		}

		GameData.Instance.AddGold(-downArea.Cost);

		if (downArea.currentLevel == 1 && downArea.Cost == IOHelper.GetPetInfoByIdAndLevel((int)downArea.type,upArea.currentLevel).cost)
		{
			PetDB.Instance.UpdatePet((int)downArea.type, 1, downArea.onBattle);
		}
		else
		{
			PetDB.Instance.UpdatePet((int)downArea.type, downArea.currentLevel + 1, downArea.onBattle);
		}
		
		UpdateDownArea(downArea.type);

		Vector3 uiPos = NGUICamera.WorldToScreenPoint(DownSpawn.position) + new Vector3(0, 0, 1);
		uiPos = WorldCamera.ScreenToWorldPoint(uiPos);
		
		GameObject effect = (GameObject)Instantiate(upgradeEffectPrefab);
		effect.transform.parent = downPetArea;
		effect.transform.localPosition = new Vector3(-2.6f, -3, 11f);
	}
}
