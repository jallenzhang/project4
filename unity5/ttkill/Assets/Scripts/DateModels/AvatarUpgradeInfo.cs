using UnityEngine;
using System.Collections;

public class AvatarUpgradeInfo {
	public int id {get;set;}
	public int lv {get;set;}
	public int hp {get;set;}
	/// <summary>
	/// 力量属性
	/// </summary>
	/// <value>The strength.</value>
	public float strength {get;set;}
	/// <summary>
	/// 眩晕时间
	/// </summary>
	/// <value>The time.</value>
	public float time {get;set;}
	/// <summary>
	/// 击退距离
	/// </summary>
	/// <value>The distance.</value>
	public float distance {get;set;}
	/// <summary>
	/// 敏捷
	/// </summary>
	/// <value>The agility.</value>
	public float agility {get;set;}
	/// <summary>
	/// (暴击伤害 + 1) * gun's atk
	/// </summary>
	/// <value>The crit.</value>
	public float crit {get;set;}
	/// <summary>
	/// 暴击率
	/// </summary>
	/// <value>The probability.</value>
	public float Probability {get;set;}
	public int cost {get;set;}
	public int type {get;set;}
}
