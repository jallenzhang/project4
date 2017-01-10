using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class TuziSM : EnemyController {
	private bool isTaked = false;
	void Awake()
	{
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected IEnumerator idle_Enter()
	{
		animation.CrossFade("idle");
		yield return null;
	}
	
	protected void idle_Exit()
	{
	}
	
	protected IEnumerator walk_Enter()
	{
		animation.CrossFade("walk");
		yield return null;
	}

	IEnumerator run_Enter()
	{
		animation.CrossFade("walk");
		yield return null;
	}

	protected IEnumerator thinkAttack_Enter()
	{
		animation.CrossFade("idle");
//		GameObject effect = (GameObject)Instantiate(Resources.Load("prefabs/Effects/shengji"));
//		effect.transform.position = transform.position;
//		ParticleSystem[] pses = effect.GetComponentsInChildren<ParticleSystem>();
//		foreach(ParticleSystem ps in pses)
//		{
//			ps.time = thinkAttackTime / 2f;
//		}

		yield return new WaitForSeconds(thinkAttackTime);
//		Destroy(effect);

		ChangeState(AnimState.attack);
	}


	protected void thinkAttack_Update()
	{
		if (!GetState().Equals(AnimState.thinkAttack) /*|| GetState().Equals(AnimState.dead)*/)
			StopCoroutine("thinkAttack_Enter");
	}

	void thinkAttack_Exit()
	{
	}
	
	protected void walk_Exit()
	{
	}
	
	protected IEnumerator attack_Enter()
	{
		animation.CrossFade("attack");
		rigidbody.mass = 100;
		yield return new WaitForSeconds(0.5f);
		AttackHero();
		ChangeState(AnimState.idle);
	}
	
	protected void attack_Exit()
	{
		rigidbody.mass = 1;
	}

	protected IEnumerator take_Enter()
	{
		isTaked = true;
		rigidbody.useGravity = false;
		animation.CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead))
			ChangeState(AnimState.idle);
	}
	
	protected void take_Exit()
	{
		isTaked = false;
		rigidbody.useGravity = true;
	}
	
	protected IEnumerator dead_Enter()
	{
		animation.CrossFade("dead");
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		collider.enabled = false;
		rigidbody.useGravity = false;

		yield return new WaitForSeconds(1f);
		rigidbody.useGravity =true;
		yield return new WaitForSeconds(2f);
		EnemyManager.Instance.Enemies.Remove(GetComponent<EnemyController>());
		EventService.Instance.GetEvent<EnemyDeadEvent> ().Publish (EnemyId);
		DestroyImmediate(gameObject);
	}
	
	protected void dead_Exit()
	{
		
	}
}
