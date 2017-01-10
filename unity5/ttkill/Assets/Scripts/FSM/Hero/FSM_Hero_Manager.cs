using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSM_Hero_Manager : MonoBehaviour {
	private FMS_State currentState;
	private StateID currentStateID;
	private List<FMS_State> states = new List<FMS_State>();
	private GameObject player;

	private static FSM_Hero_Manager instance = null;
	public static FSM_Hero_Manager Instance
	{
		get
		{
//			if (instance == null)
//			{
//				GameObject n = new GameObject();
//				n.name = "FSM_HERO";
//				instance = n.AddComponent<FSM_Hero_Manager>() as FSM_Hero_Manager;
//			}

			return instance;
		}
	}

	public FMS_State CurrentState
	{
		get
		{
			return currentState;
		}
	}

	public StateID CurrentStateID
	{
		get
		{
			return currentStateID;
		}
	}

	public void UpdateFunction()
	{
		currentState.Enter();
	}

	public void ChangeState(Translate tr)
	{
		if (tr == Translate.NullTrans)
			Debug.LogError("can't find the state");
		
		StateID statusID = currentState.GetOutState(tr);
		
		foreach(FMS_State state in states)
		{
			if (statusID == state.ID)
			{
				currentState.DoBeforeLeave();
				currentState = state;
				currentState.DoBeforeEnter();
				break;
			}
		}
	}

	public void MakeFSMMachine()
	{
		HeroAttackState attackState = new HeroAttackState();
		attackState.addDictionary(Translate.Translate_Hero_Attack, StateID.Hero_Attack);

		HeroDieState dieState = new HeroDieState();
		dieState.addDictionary(Translate.Translate_Hero_Die, StateID.Hero_Die);

		HeroWalkState walkState = new HeroWalkState();
		walkState.addDictionary(Translate.Translate_Hero_Walk, StateID.Hero_Walk);

		HeroIdleState idleState = new HeroIdleState();
		idleState.addDictionary(Translate.Translate_Hero_Idle, StateID.Hero_Idle);

		AddFSMState(idleState);
		AddFSMState(walkState);
		AddFSMState(dieState);
		AddFSMState(attackState);
	}

	private void AddFSMState(FMS_State s)
	{
		if (s == null)
		{
			Debug.LogError(" Null reference is not allowed");
		}
		
		if (states.Count == 0)
		{
			states.Add(s);                   //设置默认状态（important）;
			currentState = s;
			currentStateID = s.ID;
			return;
		}
		foreach (FMS_State state in states)
		{
			if (state == s)
			{
				Debug.LogError(s.ID.ToString() + "has already been added");
				return;
			}
		}
		states.Add(s);
	}

	public void deleteFSMState(StateID id)
	{
		if (id == StateID.NullState)
		{
			
			Debug.LogError("NullStateID is not allowed for a real state");
			
			return;
		}
		
		foreach (FMS_State state in states)
		{
			
			if (state.ID == id)
			{
				states.Remove(state);
				return;
			}
			
		}
	}

	void Awake()
	{
	}

	void Start()
	{
		instance = this;
	}

}
