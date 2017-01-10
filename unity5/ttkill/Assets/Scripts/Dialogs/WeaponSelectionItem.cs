using UnityEngine;
using System.Collections;

public class WeaponSelectionItem : MonoBehaviour {

	public UISprite type, icon;
	public GameObject lockObj;
	public WeaponSelectionItem parent;

//	[HideInInspector]
	public WeaponSelectionDialog dlg;

	private WeaponType weaponType = WeaponType.dao;
	public WeaponType WeaponType
	{
		get
		{
			return weaponType;
		}
		set
		{
			weaponType = value;
			icon.spriteName = WeaponStatus.GetWeaponSpriteName(value);
			type.spriteName = (int)value < (int)WeaponType.gun_area ? "jin" : "yuan";

			if (dlg == null)
				dlg = NGUITools.FindInParents<WeaponSelectionDialog>(transform);

			int level = WeaponDB.Instance.GetWeaponLvById((int)weaponType);
			if (level > 0)
			{
				TutorialTrigger tt =gameObject.AddComponent<TutorialTrigger>();
				tt.needIncreaseSeq = true;
				tt.needAuto = true;
				if (weaponType == WeaponType.dao && SettingManager.Instance.TutorialSeq <= 12)
				{
					tt.index = 12;
					StartCoroutine(FireTrigger());
				}
				else if (weaponType == WeaponType.gun_shouqiang && SettingManager.Instance.TutorialSeq <= 12)
				{
					tt.index = 13;
				}
				else if (weaponType == WeaponType.gun_sandan && SettingManager.Instance.TutorialSeq <= 12)
				{
					tt.index = 14;
				}
				else if (weaponType == WeaponType.gun_liudan && SettingManager.Instance.TutorialSeq <= 12)
				{
					tt.index = 15;
				}
				else if (weaponType == WeaponType.gun_m4 && SettingManager.Instance.TutorialSeq <= 12)
				{
					tt.index = 16;
				}
			}
		}
	}

	public bool IsCloseWeapon
	{
		get
		{
			return WeaponType < WeaponType.gun_area;
		}
	}

	[HideInInspector]
	public bool selected = false;

	void Start()
	{
		selected = false;
		foreach(WeaponType item in GameData.Instance.selectedWeapons)
		{
			if (item == weaponType)
			{
				selected = true;
			}
		}

		int level = WeaponDB.Instance.GetWeaponLvById((int)weaponType);
		if (level == 0)
		{
			gameObject.GetComponent<Collider>().enabled = false;
			if (lockObj != null)
			{
				lockObj.SetActive(true);
			}
		}

		if (IsCloseWeapon)
			GetComponent<UISprite>().spriteName = "zhuangpeiwuqikuang02";
		else
			GetComponent<UISprite>().spriteName = "zhuangpeiwuqikuang01";
	}

	IEnumerator FireTrigger()
	{
		yield return new WaitForSeconds(0.2f);
		TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
	}

	public void onClick()
	{
		int level = WeaponDB.Instance.GetWeaponLvById((int)weaponType);
		
		if (level == 0)
			return;

		if (!selected && IsCloseWeapon && dlg != null && dlg.HasCloseWeaponSelected()) // 只能选择一把近战武器
		{
			return;
		}
		if (!selected && dlg.SelectedGunWeaponCount() >= 4 && !IsCloseWeapon) return;

//		if (!selected && GameData.Instance.selectedWeapons.Count == 0 && !IsCloseWeapon) return;

		selected = !selected;

		if (GetComponentInParent<TutorialDialog>() != null)
		{
			TutorialDialog td = GetComponentInParent<TutorialDialog>();
			parent = td.Param.target.GetComponent<WeaponSelectionItem>();
			parent.onClick();
			return;
		}

		if (parent != null)
		{
			parent.selected = selected;
		}
		if (dlg != null)
		{
			dlg.OnSelectedWeaponsChanged();
		}

		if (selected)
		{
			icon.color = new Color(0.3f, 0.3f, 0.3f);
		}
		else
		{
			icon.color = Color.white;
			if (parent != null)
				parent.icon.color = Color.white;
		}

	}

}
