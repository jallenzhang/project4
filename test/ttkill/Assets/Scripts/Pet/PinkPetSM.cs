using UnityEngine;
using System.Collections;

public class PinkPetSM : PetController {

	// Use this for initialization
	void Start () {
		Initialize<AnimState> ();
		ChangeState (AnimState.idle);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator idle_Enter ()
	{
		animation.CrossFade ("idle");
		yield return new WaitForSeconds(3f);
		randomPetPos = GenerateRandomPetMovePos();
		ChangeState(AnimState.walk);
	}

	void walk_Enter()
	{
		animation.CrossFade("walk");
	}
}
