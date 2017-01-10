using System.Collections.Generic;
using UnityEngine;

public abstract class FMS_State    //定一个抽象类
{
	private StateID id;            //定一个状态ID作为变量来标识
	public StateID ID
	{
		set { id = value; }
		get { return id; }
	}
	
	private Dictionary<Translate, StateID> map = new Dictionary<Translate, StateID>();    //在某一状态下，事件引起了触发进入另一个状态
	// 于是我们定义了一个字典，存储的便是触发的类型，以及对应要进入的状态
	public void addDictionary(Translate tr, StateID id1)  //向字典里添加
	{
		if (tr == Translate.NullTrans)
		{
			Debug.LogError("Null Trans is not allower to adding into");
			return;
		}
		
		if (ID == StateID.NullState)
		{
			Debug.LogError("Null State id not ~~~");
			return;
			
		}
		if (map.ContainsKey(tr))              //NPC  任何时候都只能出于一种状态，所以一旦定义了一个触发的枚举类型，对应的只能是接下来的一种状态
		{
			Debug.LogError(id1.ToString() + "is already added to");
			return;
		}
		
		map.Add(tr, id1);
	}
	
	
	
	public void deleateDictionary(Translate tr) //删除字典里存储的一个状态
	{
		if (tr == Translate.NullTrans)
		{
			Debug.LogError("TransNull is not allowed to delate");
			return;
		}
		if (map.ContainsKey(tr))
		{
			
			map.Remove(tr);
			return;
		}
		Debug.LogError(tr.ToString() + "are not exist");
	}
	
	public StateID GetOutState(Translate tr)  //由状态的触发枚举类型返回一个对应的状态类型
	{
		if (map.ContainsKey(tr))
		{
			// Debug.Log("Translate " + tr + "State" + map[tr]);
			return map[tr];
			
			
		}
		return StateID.NullState;
	}
	
	public virtual void DoBeforeEnter() { }    //虚方法
	public virtual void DoBeforeLeave() { }
	
	public abstract void Enter(); //  抽象方法
	
}