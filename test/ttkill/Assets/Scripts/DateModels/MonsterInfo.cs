using UnityEngine;
using System.Collections;

public class MonsterInfo {
	public int id {get;set;}
	public string name {get;set;}
	/// <summary>
	/// 对应的prefab
	/// </summary>
	/// <value>The imageid.</value>
	public int imageid {get;set;}

	public float atk {get;set;}

	public int skill_atk {get;set;}

	public float skill_time {get;set;}

	public float atk_range {get;set;}
	/// <summary>
	/// 攻击思考时间
	/// </summary>
	/// <value>The atk_time.</value>
	public float atk_time {get;set;}
	/// <summary>
	/// 怪物的血平均血量 * hp_Proportion  
	/// </summary>
	/// <value>The hp_ proportion.</value>
	public float hp_Proportion {get;set;}
	public float move_speed {get;set;}
	public float money {get;set;}
	public float Visual_range {get;set;}
	public int score {get;set;}
}
