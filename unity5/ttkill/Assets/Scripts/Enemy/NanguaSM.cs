using UnityEngine;
using System.Collections;

public class NanguaSM : EnemyController {
	
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
		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("idle");
		yield return null;
	}
	
	protected void idle_Exit()
	{
	}
	
	protected IEnumerator walk_Enter()
	{
		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("walk");
		yield return null;
	}

	IEnumerator run_Enter()
	{
		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("walk");
		yield return null;
	}
	
	protected IEnumerator thinkAttack_Enter()
	{
		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("idle");
		
		yield return new WaitForSeconds(thinkAttackTime);
		
		ChangeState(AnimState.attack);
	}
	
	
	protected void thinkAttack_Update()
	{
		if (GetState().Equals(AnimState.take1) || GetState().Equals(AnimState.take2) ||GetState().Equals(AnimState.dead))
			StopCoroutine("thinkAttack_Enter");
	}

	void thinkAttack_Exit()
	{
		GetComponent<Rigidbody>().mass = 1;
	}
	
	protected void walk_Exit()
	{
	}
	
	protected IEnumerator attack_Enter()
	{
		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("attack");
		GetComponent<Rigidbody>().mass = 100;
		yield return new WaitForSeconds(0.5f);
		AttackHero();
		ChangeState(AnimState.idle);
	}
	
	protected void attack_Exit()
	{
		GetComponent<Rigidbody>().mass = 1;
	}
	
	protected IEnumerator take1_Enter()
	{
		if (HP <= 0)
			yield break;

		isTaked = true;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Animation>().CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead))
			ChangeState(AnimState.idle);
	}
	
	protected void take1_Exit()
	{
		isTaked = false;
		GetComponent<Rigidbody>().useGravity = true;
	}

	protected IEnumerator take2_Enter()
	{
		if (HP <= 0)
			yield break;

		isTaked = true;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Animation>().CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead))
			ChangeState(AnimState.idle);
	}
	
	protected void take2_Exit()
	{
		isTaked = false;
		GetComponent<Rigidbody>().useGravity = true;
	}
	
	protected IEnumerator dead_Enter()
	{
		StopAllLiveCoro();
		GetComponent<Animation>().CrossFade("dead");
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = false;

		yield return new WaitForSeconds(1f);
		GetComponent<Rigidbody>().useGravity = true;
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
