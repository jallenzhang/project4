using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {
	private static WeaponManager instance = null;
	public static WeaponManager Instance
	{
		get
		{
			return instance;
		}
	}

//	public static List<WeaponType> selectedWeapons = new List<WeaponType>();

	// Use this for initialization
	void Start () {
//		DontDestroyOnLoad(gameObject);
//		Debug.Log("aaaaaaaaaaaaa " + gameObject.name);
		instance = this;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static WeaponType RandomWeapon()
	{
		if (GameData.Instance.selectedWeapons.Count <= 0) return WeaponType.gun_shouqiang;
		return GameData.Instance.selectedWeapons[Random.Range(0, GameData.Instance.selectedWeapons.Count)];
	}

	public GameObject GenerateWeapon(WeaponType type, Vector3 pos)
	{
		string weaponPath = string.Empty;
		switch(type)
		{
//		case WeaponType.chuizi:
//			weaponPath = "prefabs/Weapons/chuizi";
//			break;
		case WeaponType.dao:
			weaponPath = "prefabs/Weapons/dao";
			break;
		case WeaponType.dianju:
			weaponPath = "prefabs/Weapons/dianju";
			break;
		case WeaponType.gun_fire:
			weaponPath = "prefabs/Weapons/gun_fire";
			break;
		case WeaponType.gun_liudan:
			weaponPath = "prefabs/Weapons/gun_jiqiang";
			break;
		case WeaponType.gun_m4:
			weaponPath = "prefabs/Weapons/gun_m4";
			break;
		case WeaponType.gun_sandan:
			weaponPath = "prefabs/Weapons/gun_sandan";
			break;
		case WeaponType.gun_shouqiang:
			weaponPath = "prefabs/Weapons/gun_shouqiang";
			break;
		case WeaponType.gundouble_fire:
			weaponPath = "prefabs/Weapons/gundouble_fire";
			break;
		case WeaponType.gundouble_liudan:
			weaponPath = "prefabs/Weapons/gundouble_liudan";
			break;
		case WeaponType.gundouble_m4:
			weaponPath = "prefabs/Weapons/gundouble_m4";
			break;
		case WeaponType.gundouble_sandan:
			weaponPath = "prefabs/Weapons/gundouble_sandan";
			break;
		case WeaponType.gundouble_shouqiang:
			weaponPath = "prefabs/Weapons/gundouble_shouqiang";
			break;
		case WeaponType.gun_jiatelin:
			weaponPath = "prefabs/Weapons/gun_jiatelin";
			break;
		}

		Debug.Log("weaponPath " + type);

		if (type == WeaponType.gun_m4 || type == WeaponType.gundouble_m4)
			SettingManager.Instance.JiqiangGot_Shuaxin += 1;

		GameObject go = (GameObject)Instantiate(Resources.Load(weaponPath));
		go.transform.parent = transform;
		go.transform.position = pos;

		return go;
	}

	public void DropWeapon(WeaponType type, Vector3 pos)
	{
		GameObject go = GenerateWeapon(type, pos);
		go.transform.parent = transform;
		go.transform.position = new Vector3(pos.x, 10, pos.z);

		TweenPosition tp = TweenPosition.Begin(go, 0.5f, new Vector3(pos.x, 1.4f, pos.z));
		tp.method = UITweener.Method.BounceIn;
	}
}
