using UnityEngine;
using System.Collections;

public class GunInfo {
	public int id {get;set;}
	public string name {get;set;}
	public int maxlv {get;set;}
	public int speed {get;set;}
	/// <summary>
	/// 攻击范围
	/// </summary>
	/// <value>The range.</value>
	public float range {get;set;}
}

public class WeaponData
{
	public int id {get;set;}
	public int currentLv {get;set;}
	public GunInfo info {get;set;}

}
