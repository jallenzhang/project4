using UnityEngine;
using System.Collections;

public class GunUpgradeInfo {
	public int id {get;set;}
	public int lv {get;set;}
	public int atk {get;set;}
	public int capacity {get;set;}
	/// <summary>
	/// 当lv等于0， cost不是0的时候表示还未解锁
	/// </summary>
	/// <value>The cost.</value>
	public int cost {get;set;}
	public int type {get;set;}
	/// <summary>
	/// 击退距离
	/// </summary>
	/// <value>The probability.</value>
	public float Probability{get;set;}
}
