using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Weapon
{
	public WeaponType ID;
	public string Name;
	public GameObject WeaponAvatar;
	public Material WeaponMaterial;
	public Mesh WeaponMesh;
	public GameObject BulletAvatar;
	public GameObject BulletAvatar2;

	[HideInInspector]
	public int currentLv;

	[HideInInspector]
	public float range;
	[HideInInspector]
	public float atk;
	[HideInInspector]
	public int capacity;
//	[HideInInspector]
	public float Frequency;
	[HideInInspector]
	public int currentCapacity;
	[HideInInspector]
	public float hitDistance;
}

public class WeaponController : MonoBehaviour {
	public WeaponType currentWeaponId = WeaponType.bangqiugun;
	public List<Weapon> Weapons = new List<Weapon>();
	public int currentCapacity;
	public GameObject leftHandWeapon;

	private static WeaponController instance = null;
	public static WeaponController Instance
	{
		get
		{
			return instance;
		}
	}
	// Use this for initialization
	void Start () {
		instance = this;
		InitWeapons();
		if (SettingManager.Instance.CurrentAvatar == 1)
			currentCapacity = GetWeaponByID(WeaponType.bangqiugun).capacity;
		else
			currentCapacity = GetWeaponByID(WeaponType.gun_shouqiang).capacity;
		EventService.Instance.GetEvent<WeaponChangeEvent> ().Subscribe (OnWeaponChange);
	}

	void InitWeapons()
	{
		foreach(Weapon weapon in Weapons)
		{
			int level = WeaponDB.Instance.GetWeaponLvById((int)weapon.ID);
			GunInfo info = IOHelper.GetGunInfoById((int)weapon.ID);
			int tmpLevel = Mathf.Max(1, level);
			if (weapon.ID == WeaponType.bangqiugun)
			{
				tmpLevel = Mathf.Max(AvatarDB.Instance.GetAvatarLvById(SettingManager.Instance.CurrentAvatar), 1);
			}
			else if (weapon.ID == WeaponType.gun_jiatelin)
			{
				tmpLevel = SettingManager.Instance.MaxAvatarLevel;
			}

			GunUpgradeInfo upgradeInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)weapon.ID, tmpLevel);
//			Debug.Log("weapon.ID " + (int)weapon.ID);
			weapon.range = info.range;
			weapon.atk = upgradeInfo.atk * (1f + transform.parent.GetComponent<HeroController>().attackAddtional);
			weapon.capacity = weapon.currentCapacity = Mathf.FloorToInt(upgradeInfo.capacity * (1f + transform.parent.GetComponent<HeroController>().capacityUpPercent) * GameData.Instance.bulletCapacity);
			weapon.hitDistance = upgradeInfo.Probability;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Weapon GetWeaponByID(WeaponType type)
	{
		Weapon ret = null;
		foreach(Weapon weapon in Weapons)
		{
			if (weapon.ID == type)
			{
				ret = weapon;
				break;
			}
		}

		return ret;
	}

	IEnumerator ThrowWeapon(float duration, GameObject oldWeapon)
	{
		oldWeapon.transform.position = oldWeapon.transform.position + new Vector3 (1, 0, 0);
		yield return new WaitForSeconds(duration);
		oldWeapon.GetComponentInChildren<BoxCollider> ().enabled = true;
	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<WeaponChangeEvent> ().Unsubscribe (OnWeaponChange);
		Weapons.Clear();
	}

	void OnWeaponChange(WeaponType weaponId, int capacity)
	{
//		if (weaponId != currentWeaponId) {
			if (currentWeaponId != WeaponType.bangqiugun && weaponId != WeaponType.bangqiugun && currentCapacity > 0)
			{
				Weapon oldWeapon = Weapons.Where(s=>s.ID == currentWeaponId).FirstOrDefault();
				if (oldWeapon == null)
				{
					Debug.LogError("can't find weapon by id is " + currentWeaponId);
					Debug.Log("Weapons.length " + Weapons.Count);
				}
				GameObject go = (GameObject)Instantiate(oldWeapon.WeaponAvatar);
				go.transform.position = new Vector3(transform.position.x, 1.4f, transform.position.z);
				go.transform.parent = GameObject.Find("Weapons").transform;
				go.transform.localScale = Vector3.one;
				go.GetComponentInChildren<BoxCollider>().enabled = false;
				go.GetComponent<WeaponTrigger>().currentCapacity = currentCapacity;

				StartCoroutine(ThrowWeapon(1, go));
			}

			currentWeaponId = weaponId;
			Weapon newWeapon = Weapons.Where(s=>s.ID == weaponId).FirstOrDefault();
			if (newWeapon != null)
			{
				GetComponent<SkinnedMeshRenderer>().material = newWeapon.WeaponMaterial;
				GetComponent<SkinnedMeshRenderer>().sharedMesh = newWeapon.WeaponMesh;
				if (currentWeaponId > WeaponType.gun_single 
				    && currentWeaponId < WeaponType.gun_double)
				{
					if (leftHandWeapon != null)
					{
						leftHandWeapon.GetComponent<SkinnedMeshRenderer>().material = newWeapon.WeaponMaterial;
						leftHandWeapon.GetComponent<SkinnedMeshRenderer>().sharedMesh = newWeapon.WeaponMesh;
					}
				}
				else
				{
					if (leftHandWeapon != null)
					{
						leftHandWeapon.GetComponent<SkinnedMeshRenderer>().material = null;
						leftHandWeapon.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
					}
				}

			}
			if (capacity != 0)
				newWeapon.currentCapacity  = capacity;
			else
				newWeapon.currentCapacity = newWeapon.capacity;

			currentCapacity = newWeapon.currentCapacity;
			WeaponStatus.Instance.ChangeWeaponMPProgress((float)currentCapacity/(float)GetWeaponByID(currentWeaponId).capacity);
//		}
	}

	public void DecreaseWeaponCapacity()
	{
		currentCapacity--;
		currentCapacity = Mathf.Max(0, currentCapacity);
		if(currentCapacity == 0)
		{
			EventService.Instance.GetEvent<WeaponChangeEvent>().Publish(WeaponType.bangqiugun, 0);

		}
		else
		{
			WeaponStatus.Instance.ChangeWeaponMPProgress((float)currentCapacity/(float)GetWeaponByID(currentWeaponId).capacity);
		}
	}
}
