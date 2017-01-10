using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WeaponUpgradeIndex
{
	public int Index;
	public GameObject WeaponPrefab;
//	public Mesh WeaponMesh;
	public WeaponType WeaponType;
}

public class WeaponUpgrade : MonoBehaviour {
	public List<WeaponUpgradeIndex> weapons = new List<WeaponUpgradeIndex>();

	private static WeaponUpgrade instance = null;
	public static WeaponUpgrade Instance
	{
		get
		{
			return instance;
		}
	}

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	public WeaponType UpdateUI (int index) {
//		MeshFilter mf = GetComponent<MeshFilter>();
//		mf.mesh = weapons[index].WeaponMesh;

		return weapons[index].WeaponType;
	}
}
