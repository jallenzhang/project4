using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class EventBase
{
	private List<Action> _actions;
	
	/// <summary>
	/// 场景切换时，是否移除该事件对象的订阅方法
	/// </summary>
	public bool KeepOnLevelChanging { get; protected set; }
	
	/// <summary>
	/// 发布事件
	/// </summary>
	public void Publish()
	{
		if (_actions == null) return;
		
		foreach (var action in _actions)
		{
			action();
		}
	}
	
	/// <summary>
	/// 订阅事件
	/// </summary>
	public void Subscribe(Action action)
	{
		if (_actions == null)
		{
			_actions = new List<Action>();
		}
		
		if (!_actions.Contains(action))
		{
			_actions.Add(action);
		}
	}
	
	/// <summary>
	/// 取消订阅
	/// </summary>
	public void Unsubscribe(Action action)
	{
		if (_actions == null)
		{
			return;
		}
		
		if (_actions.Contains(action))
		{
			_actions.Remove(action);
		}
	}
	
	/// <summary>
	/// 移除所有订阅内容
	/// </summary>
	public void Clear()
	{
		if (_actions == null)
		{
			return;
		}
		
		_actions.Clear();
	}
}



/**/

public abstract class EventBase<T> : EventBase
{
	/* 基本方法和EventBase相同，仅把_actions换成List<Action<T>>*/
	
	private List<Action<T>> _actions;
	
	public void Publish(T payload)
	{
		if (_actions == null) return;
		
		foreach (var action in _actions)
		{
			action(payload);
		}
	}
	
	public void Subscribe(Action<T> action)
	{
		if (_actions == null)
		{
			_actions = new List<Action<T>>();
		}
		
		if (!_actions.Contains(action))
		{
			_actions.Add(action);
		}
	}
	
	public void Unsubscribe(Action<T> action)
	{
		if (_actions == null)
		{
			return;
		}
		
		if (_actions.Contains(action))
		{
			_actions.Remove(action);
		}
	}
	
	public new void Clear()
	{
		if (_actions == null)
		{
			return;
		}
		
		_actions.Clear();
		base.Clear();
	}
}

public abstract class EventBase<T1,T2> : EventBase
{
	/* 基本方法和EventBase相同，仅把_actions换成List<Action<T>>*/
	
	private List<Action<T1, T2>> _actions;
	
	public void Publish(T1 payload, T2 paylaod2)
	{
		if (_actions == null) return;
		
		foreach (var action in _actions)
		{
			action(payload, paylaod2);
		}
	}
	
	public void Subscribe(Action<T1, T2> action)
	{
		if (_actions == null)
		{
			_actions = new List<Action<T1, T2>>();
		}
		
		if (!_actions.Contains(action))
		{
			_actions.Add(action);
		}
	}
	
	public void Unsubscribe(Action<T1, T2> action)
	{
		if (_actions == null)
		{
			return;
		}
		
		if (_actions.Contains(action))
		{
			_actions.Remove(action);
		}
	}
	
	public new void Clear()
	{
		if (_actions == null)
		{
			return;
		}
		
		_actions.Clear();
		base.Clear();
	}
}