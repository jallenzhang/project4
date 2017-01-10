//#define US_VERSION
using UnityEngine;
using System.Collections;

public class HeroUpgradeDialog : DialogBase
{
	public Transform personPos;
	public Transform weaponPos;
	public UIProgressBar progressHP;
	public UIProgressBar progressStrength;
	public UIProgressBar progressAgility;
	public UILabel labelLv;
	public UILabel labelCost;
	public GameObject upgrade_effect;
	public GameObject maxlevelObj;

	public UILabel hpValue;
	public UILabel strengthValue;
	public UILabel agilityValue;

	private static Camera WorldCamera;
	private Camera NGUICamera;
	private GameObject weapon;
	public static void Popup()
	{
//		UILayout.Instance.BottomOut();
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/renwushengji_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/renwushengji");
#endif


	}

	void Start()
	{
		Update3DUI(SettingManager.Instance.CurrentAvatarId);
		UpdateUI(SettingManager.Instance.CurrentAvatarId);
	}

	void Update3DUI(int avatarId)
	{
		GameObject person = GameObject.FindGameObjectWithTag("3DMain");
		int childCount = person.transform.childCount;
		for(int i = 0; i < childCount; i++)
		{
			if (person.transform.GetChild(i).gameObject.name.Equals("nan01"))
			{
				person.transform.GetChild(i).gameObject.SetActive(SettingManager.Instance.CurrentAvatarId == 1);
			}
			else if (person.transform.GetChild(i).gameObject.name.Equals("nv01"))
			{
				person.transform.GetChild(i).gameObject.SetActive(SettingManager.Instance.CurrentAvatarId == 2);
			}
			else
				person.transform.GetChild(i).gameObject.SetActive(true);
		}
		
		WorldCamera = person.GetComponentInChildren<Camera>();
		WorldCamera.depth = 3;


		NGUICamera = GetComponentInParent<Camera>();
		
		//weapon
		if (WorldCamera.transform.childCount > 0)
			Destroy(WorldCamera.transform.GetChild(0).gameObject);

		if (avatarId == 1)
			weapon = (GameObject)Instantiate(Resources.Load("prefabs/Weapons/bangqiugun"));
		else
			weapon = (GameObject)Instantiate(Resources.Load("prefabs/Weapons/gun_shouqiang"));
		weapon.transform.parent = WorldCamera.transform;
		Vector3 screenpos = NGUICamera.WorldToScreenPoint(weaponPos.position);
		screenpos += new Vector3(0, 0, 1);
		weapon.transform.position = WorldCamera.ScreenToWorldPoint(screenpos);
		weapon.transform.localScale = Vector3.one * 0.08f;
		if (avatarId == 1)
			weapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 135f, 0));
		else
			weapon.transform.localRotation = Quaternion.Euler(new Vector3(-70f, -160f, -35f));
		weapon.GetComponent<WeaponTrigger>().enabled = false;
		weapon.transform.FindChild("shengji").gameObject.SetActive(false);

		//person
//		GameObject hero = (GameObject)Instantiate(Resources.Load("prefabs/Heros/heroUpgrade/nan01"));
//		hero.transform.parent = WorldCamera.transform;
//		screenpos = NGUICamera.WorldToScreenPoint(personPos.position);
//		screenpos += new Vector3(0, 0, 1);
//		hero.transform.position = WorldCamera.ScreenToWorldPoint(screenpos);
//		hero.transform.localScale = Vector3.one * 0.1f;
//		hero.transform.localRotation = Quaternion.Euler(new Vector3(0, 180,0));
//		hero.transform.FindChild("weapon").gameObject.SetActive(false);
	}

	void UpdateUI(int avatarId)
	{
		int level = AvatarDB.Instance.GetAvatarLvById(avatarId);
		level = Mathf.Max(1, level);
		AvatarInfo avatarInfo = IOHelper.GetAvatarInfoById(avatarId);
		int maxLv = avatarInfo.maxlv;

		AvatarUpgradeInfo avatarUpgradeInfo = IOHelper.GetAvaterUpgradeInfoByIdAndLevel(avatarId, level);
		AvatarUpgradeInfo maxAvatarUpgradeInfo = IOHelper.GetAvaterUpgradeInfoByIdAndLevel(avatarId, maxLv);

		if (labelLv != null)
		{
			labelLv.text = level.ToString();
		}

		if (level == maxLv)
		{
			maxlevelObj.SetActive(true);
		}
		else
		{
			maxlevelObj.SetActive(false);
		}

		if (hpValue != null)
		{
			hpValue.text = avatarUpgradeInfo.hp + "/" + maxAvatarUpgradeInfo.hp;
		}

		if (progressHP != null)
		{
			progressHP.value = (float)avatarUpgradeInfo.hp / (float)maxAvatarUpgradeInfo.hp;
		}

		if (strengthValue != null)
		{
			strengthValue.text = avatarUpgradeInfo.strength + "/" + maxAvatarUpgradeInfo.strength;
		}

		if (progressStrength != null)
		{
			progressStrength.value = avatarUpgradeInfo.strength / maxAvatarUpgradeInfo.strength;
		}

		if (agilityValue != null)
		{
			agilityValue.text = avatarUpgradeInfo.agility + "/" + maxAvatarUpgradeInfo.agility;
		}

		if (progressAgility != null)
		{
			progressAgility.value = avatarUpgradeInfo.agility / maxAvatarUpgradeInfo.agility;
		}

		if (labelCost != null && level < maxLv)
		{
			int cost = 0;
//			if (AvatarDB.Instance.GetAvatarLvById(avatarId) == 0 && IOHelper.GetAvaterUpgradeInfoByIdAndLevel(avatarId, level).cost != 0)
//			{
//				cost = IOHelper.GetAvaterUpgradeInfoByIdAndLevel(avatarId, level).cost;
//			}

//			if (cost == 0)
			{
				cost = IOHelper.GetAvaterUpgradeInfoByIdAndLevel(avatarId, Mathf.Min(maxLv, level + 1)).cost;
			}

			labelCost.text = cost.ToString();
		}
	}

	void OnDestroy()
	{
		WorldCamera.depth = 1;
		Destroy(weapon);
//		int count = WorldCamera.transform.childCount-1;
//
//		while(count >= 0)
//		{
//			Destroy(WorldCamera.transform.GetChild(count).gameObject);
//			count--;
//		}

//		GameObject person = GameObject.FindGameObjectWithTag("3DPerson");
//		int childCount = person.transform.childCount;
//		for(int i = 0; i < childCount; i++)
//		{
//			person.transform.GetChild(i).gameObject.SetActive(false);
//		}

	}

	public void OnBuy()
	{

	}

	public void OnUpgrade()
	{
		if (GameData.Instance.currentGold < int.Parse(labelCost.text))
		{
			NotEnoughGoldDialog.Popup();
			return;
		}

		if (AvatarDB.Instance.GetAvatarLvById(SettingManager.Instance.CurrentAvatarId) < IOHelper.GetAvatarInfoById(SettingManager.Instance.CurrentAvatarId).maxlv)
		{
			int level = AvatarDB.Instance.GetAvatarLvById(SettingManager.Instance.CurrentAvatarId) + 1;
			AvatarDB.Instance.UpdateAvatar(SettingManager.Instance.CurrentAvatarId, level);
			GameData.Instance.AddGold(-int.Parse(labelCost.text));
			UpdateUI(SettingManager.Instance.CurrentAvatarId);
			GameObject effect = (GameObject)Instantiate(upgrade_effect);
			effect.transform.parent = WorldCamera.transform.parent;
			Vector3 screenpos = NGUICamera.WorldToScreenPoint(personPos.position);
			screenpos += new Vector3(0, 0, 1);

			effect.transform.localScale = Vector3.one * 0.1f;
#if US_VERSION
			effect.transform.position = new Vector3(0.7f, 0, -1);//WorldCamera.ScreenToWorldPoint(screenpos);
#else
			effect.transform.position = new Vector3(0.7f, 0, -1);//WorldCamera.ScreenToWorldPoint(screenpos);
#endif
			SettingManager.Instance.MaxAvatarLevel = Mathf.Max(SettingManager.Instance.MaxAvatarLevel, level);
		}
	}

	public void onLeft()
	{
		if (SettingManager.Instance.CurrentAvatarId == 1)
			return;

		SettingManager.Instance.CurrentAvatarId -= 1;
		Update3DUI(SettingManager.Instance.CurrentAvatarId);
		UpdateUI(SettingManager.Instance.CurrentAvatarId);
	}
	
	public void onRight()
	{
		if (SettingManager.Instance.CurrentAvatarId >= 2)
			return;
		
		SettingManager.Instance.CurrentAvatarId += 1;
		Update3DUI(SettingManager.Instance.CurrentAvatarId);
		UpdateUI(SettingManager.Instance.CurrentAvatarId);
	}

}
