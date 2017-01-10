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
		GetComponent<Animation>().CrossFade("idle");
		yield return null;
	}
	
	protected void idle_Exit()
	{
	}
	
	protected IEnumerator walk_Enter()
	{
		GetComponent<Animation>().CrossFade("walk");
		yield return null;
	}

	IEnumerator run_Enter()
	{
		GetComponent<Animation>().CrossFade("walk");
		yield return null;
	}

	protected IEnumerator thinkAttack_Enter()
	{
		GetComponent<Animation>().CrossFade("idle");
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

	protected IEnumerator take_Enter()
	{
		isTaked = true;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Animation>().CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead))
			ChangeState(AnimState.idle);
	}
	
	protected void take_Exit()
	{
		isTaked = false;
		GetComponent<Rigidbody>().useGravity = true;
	}
	
	protected IEnumerator dead_Enter()
	{
		GetComponent<Animation>().CrossFade("dead");
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = false;

		yield return new WaitForSeconds(1f);
		GetComponent<Rigidbody>().useGravity =true;
		yield return new WaitForSeconds(2f);
		EnemyManager.Instance.Enemies.Remove(GetComponent<EnemyController>());
		EventService.Instance.GetEvent<EnemyDeadEvent> ().Publish (EnemyId);
		DestroyImmediate(gameObject);
	}
	
	protected void dead_Exit()
	{
		
	}
}
