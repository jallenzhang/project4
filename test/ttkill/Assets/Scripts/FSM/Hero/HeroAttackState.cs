using UnityEngine;
using System.Collections;

public class HeroAttackState : FMS_State {

	public HeroAttackState()
	{
		ID = StateID.Hero_Attack;
	}

	public override void Enter ()
	{
//		throw new System.NotImplementedException ();
	}

	public override void DoBeforeEnter ()
	{
		base.DoBeforeEnter ();
	}

	public override void DoBeforeLeave ()
	{
		base.DoBeforeLeave ();
	}
}
