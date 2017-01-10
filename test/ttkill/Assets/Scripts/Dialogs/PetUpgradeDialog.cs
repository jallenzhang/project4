//#define US_VERSION
using UnityEngine;
using System.Collections;
using System;

public class PetUpgradeDialog : DialogBase {
	public UILabel labelLeftLv;
	public UILabel labelRightLv;
	public Transform LeftSpawn;
	public Transform RightSpawn;
	public GameObject lockObj;
	public GameObject btnRightChange;
	public GameObject btnRightUnlock;
	public UISprite leftFillSprite;
	public UISprite rightFillSprite;

	private static Camera WorldCamera;

	private Camera NGUICamera;
	private int petPos = 0; //0: no where;  1: left pos;  2: right pos
	private Transform leftPetArea;
	private Transform rightPetArea;

	private PetArea leftArea;
	private PetArea rightArea;

	private PetType leftType;
	private PetType rightType;

	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/PetUpgradeDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/PetUpgradeDialog");
#endif

		GameObject pet = GameObject.FindGameObjectWithTag("3DPet");
		int childCount = pet.transform.childCount;
		for(int i = 0; i < childCount; i++)
		{
			pet.transform.GetChild(i).gameObject.SetActive(true);
		}
		
		WorldCamera = pet.GetComponentInChildren<Camera>();
		WorldCamera.gameObject.SetActive(true);

	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<PetOnLeftBattleEvent>().Unsubscribe(UpdateLeft);
		EventService.Instance.GetEvent<PetOnRightBattleEvent>().Unsubscribe(UpdateRight);

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

	void Start()
	{
		NGUICamera = GetComponentInParent<Camera>();
		leftPetArea = WorldCamera.transform.FindChild("up");
		rightPetArea = WorldCamera.transform.FindChild("down");

		leftArea = new PetArea();
		leftArea.Reset();
		rightArea = new PetArea();
		rightArea.Reset();

		EventService.Instance.GetEvent<PetOnLeftBattleEvent>().Subscribe(UpdateLeft);
		EventService.Instance.GetEvent<PetOnRightBattleEvent>().Subscribe(UpdateRight);

		UpdateUI();
		UpdateBottomArea();
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

	public void OnPetSelect(PetType type)
	{
		if (petPos == 0)
			return;
		
		if (petPos == 1)
		{
			UpdateLeft(type);
		}
		else if (petPos == 2)
		{
			UpdateRight(type);
		}
	}

	void UpdateLeft(PetType type)
	{
		leftType = type;
		int count = leftPetArea.childCount - 1;
		while(count >= 0)
		{
			leftArea.oldType = leftArea.type;
			leftArea.oldCurrentLevel = leftArea.currentLevel;
			Destroy(leftPetArea.GetChild(count).gameObject);
			count--;
		}

		leftFillSprite.enabled = false;

		leftArea.currentLevel = PetDB.Instance.GetPetLvById((int)type);
		leftArea.onBattle = PetDB.Instance.GetPetOnBattleById((int)type);
		leftArea.type = type;
		int cost = 0;
		if (leftArea.currentLevel == 0)
		{
			leftArea.currentLevel = 1;
			cost = IOHelper.GetPetInfoByIdAndLevel((int)type, leftArea.currentLevel).cost;
		}

		Vector3 uiPos = NGUICamera.WorldToScreenPoint(LeftSpawn.position) + new Vector3(0, 0, 1);
		
		GameObject objPet = GeneratePet(type, WorldCamera.ScreenToWorldPoint(uiPos));

		objPet.transform.parent = leftPetArea;

		if (labelLeftLv != null)
			labelLeftLv.text = "Lv." + leftArea.currentLevel;
	}

	void UpdateRight(PetType type)
	{
		rightType = type;
		int count = rightPetArea.childCount - 1;
		while(count >= 0)
		{
			rightArea.oldType = rightArea.type;
			rightArea.oldCurrentLevel = rightArea.currentLevel;
			Destroy(rightPetArea.GetChild(count).gameObject);
			count--;
		}

		rightFillSprite.enabled = false;

		rightArea.currentLevel = PetDB.Instance.GetPetLvById((int)type);
		rightArea.onBattle = PetDB.Instance.GetPetOnBattleById((int)type);
		rightArea.type = type;
		int cost = 0;
		if (rightArea.currentLevel == 0)
		{
			rightArea.currentLevel = 1;
			cost = IOHelper.GetPetInfoByIdAndLevel((int)type, rightArea.currentLevel).cost;
		}
		
		Vector3 uiPos = NGUICamera.WorldToScreenPoint(RightSpawn.position) + new Vector3(0, 0, 1);
		
		GameObject objPet = GeneratePet(type, WorldCamera.ScreenToWorldPoint(uiPos));
		
		objPet.transform.parent = rightPetArea;
		
		if (labelRightLv != null)
			labelRightLv.text = "Lv." + rightArea.currentLevel;
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
			objPet.transform.localScale = Vector3.one * 0.1f;
			break;
		case PetType.tuzi:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/Enemy/tuzi"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.1f;
			break;
		case PetType.pet3:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet03"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.1f;
			break;
		case PetType.pet4:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet04"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.1f;
			break;
		case PetType.pet5:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet05"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.1f;
			break;
		case PetType.pet6:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet06"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.1f;
			break;
		case PetType.pet7:
			objPet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet07"));
			objPet.GetComponent<Rigidbody>().isKinematic = true;
			//			objPet.transform.parent = downPetArea;
			objPet.transform.position = pos;
			objPet.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			objPet.transform.localScale = Vector3.one * 0.1f;
			break;
		}

		if (objPet.GetComponent<PinkPetSM>() != null)
			objPet.GetComponent<PinkPetSM>().enabled = false;
		return objPet;
	}

	public void UnLockPetPos()
	{
		UnlockPetPositionDialog.PopUp(80, 2, UpdateBottomArea);
	}

	void UpdateBottomArea()
	{
		Debug.Log("SettingManager.Instance.PetLocked " + SettingManager.Instance.PetLocked);
		if (SettingManager.Instance.PetLocked == 1)
		{
			lockObj.SetActive(true);
			btnRightChange.SetActive(false);
			btnRightUnlock.SetActive(true);
		}
		else
		{
			lockObj.SetActive(false);
			btnRightChange.SetActive(true);
			btnRightUnlock.SetActive(false);
		}
	}

	public void OnChangeLeft()
	{
		PetListDialog.Popup(true, leftType);
	}

	public void OnChangeRight()
	{
		PetListDialog.Popup(false, rightType);
	}

	public void CloseDialog()
	{
		DialogManager.Instance.CloseDialog();
	}
}
