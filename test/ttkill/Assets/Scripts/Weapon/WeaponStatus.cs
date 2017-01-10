using UnityEngine;
using System.Collections;

public class WeaponStatus : MonoBehaviour {
	public UISprite icon;
	public UISprite weaponMPProgress;
	private int weaponMP = 400;
	private static WeaponStatus instance = null;

	public static WeaponStatus Instance
	{
		get
		{
			return instance;
		}
	}
	// Use this for initialization
	void Start () {
		instance = this;
		if (weaponMPProgress != null)
			weaponMP = weaponMPProgress.width;
		SubscribeEvents();
	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<WeaponChangeEvent>().Unsubscribe(OnWeaponChange);
	}

	void SubscribeEvents()
	{
		EventService.Instance.GetEvent<WeaponChangeEvent>().Subscribe(OnWeaponChange);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeWeaponMPProgress(float progress)
	{
		if (weaponMPProgress != null)
			weaponMPProgress.width = Mathf.RoundToInt(progress * weaponMP);
	}

	public void OnWeaponChange(WeaponType type, int capacity)
	{
		icon.spriteName = GetWeaponSpriteName(type);
	}

	public static string GetWeaponSpriteName(WeaponType type)
	{
		string spriteName = string.Empty;
		switch (type)
		{
			case WeaponType.bangqiugun:
				spriteName = "abgnqiubang";
				break;
//			case WeaponType.chuizi:
//				spriteName = "chuizi";
//				break;
			case WeaponType.dao:
				spriteName = "dao";
				break;
			case WeaponType.dianju:
				spriteName = "dianju";
				break;
			case WeaponType.gun_fire:
				spriteName = "huoqiang";
				break;
			case WeaponType.gun_liudan:
				spriteName = "liudanqiang";
				break;
			case WeaponType.gun_m4:
				spriteName = "jiqiang";
				break;
			case WeaponType.gun_sandan:
				spriteName = "baoli";
				break;
			case WeaponType.gun_shouqiang:
				spriteName = "shouqiang";
				break;
			case WeaponType.gundouble_fire:
				spriteName = "huoqiang2";
				break;
			case WeaponType.gundouble_liudan:
				spriteName = "liudanqiang2";
				break;
			case WeaponType.gundouble_m4:
				spriteName = "jiqiang2";
				break;
			case WeaponType.gundouble_sandan:
				spriteName = "baoli2";
				break;
			case WeaponType.gundouble_shouqiang:
				spriteName = "shouqiang2";
				break;
			case WeaponType.gun_jiatelin:
				spriteName = "bujixiang_icon";
				break;
		}
		return spriteName;
	}


}
