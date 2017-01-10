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

	public UILabel lableHP;
	public UILabel labelStrength;
	public UILabel labelAgility;

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
		Update3DUI(SettingManager.Instance.CurrentAvatar);
		UpdateUI(SettingManager.Instance.CurrentAvatar);
	}

	void Update3DUI(int avatarId)
	{
		NGUICamera = GetComponentInParent<Camera>();

		GameObject person = GameObject.FindGameObjectWithTag("3DMain");
		int childCount = person.transform.childCount;
		for(int i = 0; i < childCount; i++)
		{
			if (person.transform.GetChild(i).gameObject.name.Equals("nan01"))
			{
				if (SettingManager.Instance.CurrentAvatar == 1)
					person.transform.GetChild(i).gameObject.SetActive(true);
				else
					person.transform.GetChild(i).gameObject.SetActive(false);
			}
			else if (person.transform.GetChild(i).gameObject.name.Equals("nv01"))
			{
				if (SettingManager.Instance.CurrentAvatar == 2)
					person.transform.GetChild(i).gameObject.SetActive(true);
				else
					person.transform.GetChild(i).gameObject.SetActive(false);
			}
			else
			{
				person.transform.GetChild(i).gameObject.SetActive(true);
			}
		}

		WorldCamera = person.GetComponentInChildren<Camera>();
		WorldCamera.depth = 3;

		if (WorldCamera.transform.childCount > 0)
			Destroy(WorldCamera.transform.GetChild(0).gameObject);

		//weapon
		if (SettingManager.Instance.CurrentAvatar == 1)
			weapon = (GameObject)Instantiate(Resources.Load("prefabs/Weapons/bangqiugun"));
		else
			weapon = (GameObject)Instantiate(Resources.Load("prefabs/Weapons/gun_shouqiang"));
		weapon.transform.parent = WorldCamera.transform;
		Vector3 screenpos = NGUICamera.WorldToScreenPoint(weaponPos.position);
		screenpos += new Vector3(0, 0, 1);
		weapon.transform.position = WorldCamera.ScreenToWorldPoint(screenpos);
		weapon.transform.localScale = Vector3.one * 0.1f;
		if (SettingManager.Instance.CurrentAvatar == 1)
			weapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 135f, 0));
		else
			weapon.transform.localRotation = Quaternion.Euler(new Vector3(-70, 120f, -110));
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

		if (lableHP != null)
		{
			lableHP.text = avatarUpgradeInfo.hp + "/" + maxAvatarUpgradeInfo.hp;
		}

		if (progressHP != null)
		{
			progressHP.value = (float)avatarUpgradeInfo.hp / (float)maxAvatarUpgradeInfo.hp;
		}

		if (labelStrength != null)
		{
			labelStrength.text = avatarUpgradeInfo.strength + "/" + maxAvatarUpgradeInfo.strength;
		}

		if (progressStrength != null)
		{
			progressStrength.value = avatarUpgradeInfo.strength / maxAvatarUpgradeInfo.strength;
		}

		if (labelAgility != null)
		{
			labelAgility.text = avatarUpgradeInfo.agility + "/" + maxAvatarUpgradeInfo.agility;
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

		if (AvatarDB.Instance.GetAvatarLvById(SettingManager.Instance.CurrentAvatar) < IOHelper.GetAvatarInfoById(SettingManager.Instance.CurrentAvatar).maxlv)
		{
			int level = AvatarDB.Instance.GetAvatarLvById(SettingManager.Instance.CurrentAvatar) + 1;
			AvatarDB.Instance.UpdateAvatar(SettingManager.Instance.CurrentAvatar, level);
			GameData.Instance.AddGold(-int.Parse(labelCost.text));
			UpdateUI(SettingManager.Instance.CurrentAvatar);
			GameObject effect = (GameObject)Instantiate(upgrade_effect);
			effect.transform.parent = WorldCamera.transform.parent;
			Vector3 screenpos = NGUICamera.WorldToScreenPoint(personPos.position);
			screenpos += new Vector3(0, 0, 1);

			effect.transform.localScale = Vector3.one * 0.1f;
			effect.transform.position = new Vector3(0.7f, 0, -1);//WorldCamera.ScreenToWorldPoint(screenpos);
			SettingManager.Instance.MaxAvatarLevel = Mathf.Max(SettingManager.Instance.MaxAvatarLevel, level);
		}
	}

	public void onLeft()
	{
		if (SettingManager.Instance.CurrentAvatar == 1)
			return;

		SettingManager.Instance.CurrentAvatar = 1;
		Update3DUI(SettingManager.Instance.CurrentAvatar);
		UpdateUI(SettingManager.Instance.CurrentAvatar);
	}

	public void onRight()
	{
		if (SettingManager.Instance.CurrentAvatar == 2)
			return;
		
		SettingManager.Instance.CurrentAvatar = 2;
		Update3DUI(SettingManager.Instance.CurrentAvatar);
		UpdateUI(SettingManager.Instance.CurrentAvatar);
	}

}
