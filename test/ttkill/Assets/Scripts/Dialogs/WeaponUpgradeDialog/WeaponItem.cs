using UnityEngine;
using System.Collections;

public class WeaponItem : MonoBehaviour {
	public int index;
	public WeaponType type;
	public Transform weaponPos;

	public GameObject lv;
	public GameObject maxLv;
	public UILabel    labelCurrentLevel;
	public UILabel    labelCurrentAttack;
	public UILabel    labelCurrentCapacity;
	public UILabel    labelSpeed;
	public UIProgressBar progressAttack;
	public UIProgressBar progressCapacity;

	private Camera weaponCamera;
	private Camera uiCamera;
	private GameObject weapon;
	private Vector3 worldPos;
	// Use this for initialization
	void Start () {
		GameObject weapon3D = GameObject.FindGameObjectWithTag("3DWeapon");
		weaponCamera = weapon3D.GetComponentInChildren<Camera>();
		uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);

		weapon = (GameObject)Instantiate(WeaponUpgrade.Instance.weapons[index].WeaponPrefab);
		weapon.transform.parent = weapon3D.transform;
		weapon.transform.localScale = Vector3.one * 0.1f;

		UpdateUI();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos =uiCamera.WorldToScreenPoint(weaponPos.position);
		pos += new Vector3(0, 0, 1);
		worldPos = weaponCamera.ScreenToWorldPoint(pos);
		weapon.transform.position = worldPos;
	}

	public void UpdateUI()
	{
		GunInfo info = IOHelper.GetGunInfoById((int)type);
		int level = WeaponDB.Instance.GetWeaponLvById((int)type);
		int tmpLevel = Mathf.Max(1, level);
		Debug.Log("tmpLevel " + tmpLevel + " (int)wt " + (int)type);
		GunUpgradeInfo upgradeInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)type, tmpLevel);
		GunUpgradeInfo nextUpgradeInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)type, level + 1);
		GunUpgradeInfo maxInfo = IOHelper.GetGunUpgradeInfoByIdAndLevel((int)type, info.maxlv);

		
		if (level < info.maxlv)
		{
			lv.SetActive(true);
			maxLv.SetActive(false);
			labelCurrentLevel.text = tmpLevel.ToString();
		}
		else
		{
			lv.SetActive(false);
			maxLv.SetActive(true);
		}
		labelCurrentAttack.text = upgradeInfo.atk.ToString();
		labelCurrentCapacity.text = upgradeInfo.capacity.ToString();
		labelSpeed.text = info.speed.ToString();
		
		progressAttack.value = (float)upgradeInfo.atk / (float)maxInfo.atk;
		progressCapacity.value = (float)upgradeInfo.capacity / (float)maxInfo.capacity;
	}

	void OnDestroy()
	{
		Destroy(weapon);
	}
}
