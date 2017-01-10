using UnityEngine;
using System.Collections;

public class HuaSM : EnemyController {

	//	private bool isTaked = false;
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
		if (HP <= 0)
			yield break;

		animation.CrossFade("walk");
		yield return null;
	}
	
	protected void idle_Exit()
	{
	}
	
	protected IEnumerator walk_Enter()
	{
		if (HP <= 0)
			yield break;

		animation.CrossFade("walk");
		yield return null;
	}

	IEnumerator run_Enter()
	{
		if (HP <= 0)
			yield break;

		animation.CrossFade("run");
		yield return null;
	}
	
	protected IEnumerator thinkAttack_Enter()
	{
		if (HP <= 0)
			yield break;

		animation.CrossFade("walk");
		
		yield return new WaitForSeconds(thinkAttackTime);
		
		ChangeState(AnimState.attack);
	}

	void thinkAttack_Exit()
	{
	}
	
	
	protected void thinkAttack_Update()
	{
		if (GetState().Equals(AnimState.take1) || GetState().Equals(AnimState.take2) || GetState().Equals(AnimState.dead))
			StopCoroutine("thinkAttack_Enter");
	}
	
	protected void walk_Exit()
	{
	}
	
	protected IEnumerator attack_Enter()
	{
		animation.CrossFade("attack");
		yield return new WaitForSeconds(1.6f);
		AttackHero();
		ChangeState(AnimState.idle);
	}
	
	protected void attack_Exit()
	{
	}
	
	protected IEnumerator take1_Enter()
	{
		if (HP <= 0)
			yield break;

		animation.CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead) && HP > 0)
			ChangeState(AnimState.idle);
	}
	
	protected void take1_Exit()
	{
	}

	protected IEnumerator take2_Enter()
	{
		if (HP <= 0)
			yield break;

		animation.CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead) && HP > 0)
			ChangeState(AnimState.idle);
	}
	
	protected void take2_Exit()
	{
	}
	
	protected IEnumerator dead_Enter()
	{
		StopAllLiveCoro();
		animation.CrossFade("dead");
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		collider.enabled = false;
		rigidbody.useGravity = false;

		yield return new WaitForSeconds(1f);
		rigidbody.useGravity = true;
		yield return new WaitForSeconds(2f);
		EnemyManager.Instance.Enemies.Remove(GetComponent<EnemyController>());
		EventService.Instance.GetEvent<EnemyDeadEvent> ().Publish (EnemyId);
		DestroyImmediate(gameObject);
	}
	
	protected void dead_Exit()
	{
		
	}

	void StopAllLiveCoro()
	{
		StopCoroutine("idle_Enter");
		StopCoroutine("walk_Enter");
		StopCoroutine("run_Enter");
		StopCoroutine("thinkAttack_Enter");
		StopCoroutine("attack_Enter");
		StopCoroutine("take1_Enter");
		StopCoroutine("take2_Enter");
	}
}
