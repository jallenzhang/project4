using UnityEngine;
using System.Collections;

public class DogSM : EnemyController {
	private GameObject bossHP;
	//	private bool isTaked = false;
	void Awake()
	{
	}
	
	// Use this for initialization
	void Start () {
		if ((int)currentType >= 100)
		{
			bossHP = (GameObject)Instantiate(Resources.Load("UI/bossHP"));
			bossHP.transform.parent = UIBattleSceneLogic.Instance.NGUICamera.transform;
			bossHP.transform.localScale = Vector3.one;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (bossHP != null && bossHP.GetComponent<UIBossHP>().bossHP.value > 0)
		{
			bossHP.transform.position = Helper.WorldToNGUIPos(Camera.main, UIBattleSceneLogic.Instance.NGUICamera, transform.position + transform.up * 6);
			bossHP.GetComponent<UIBossHP>().bossHP.value = HP / maxHP;
			bossHP.GetComponent<UIBossHP>().status.text = GetState().ToString();
		}
		else
		{
			if (bossHP != null)
				Destroy(bossHP);
		}
	}
	
	protected IEnumerator idle_Enter()
	{
		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("walk");
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

		GetComponent<Animation>().CrossFade("run");
		yield return null;
	}
	
	protected IEnumerator thinkAttack_Enter()
	{
		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("walk");
		
		yield return new WaitForSeconds(thinkAttackTime);
		
		ChangeState(AnimState.attack);
	}
	
	
	protected void thinkAttack_Update()
	{
		if (GetState().Equals(AnimState.take2) || GetState().Equals(AnimState.take1) || GetState().Equals(AnimState.dead))
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
		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("attack");
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

		GetComponent<Animation>().CrossFade("take");
		yield return new WaitForSeconds(1f);
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

		GetComponent<Animation>().CrossFade("take");
		yield return new WaitForSeconds(1f);
		if (!GetState().Equals(AnimState.dead) && HP > 0)
			ChangeState(AnimState.idle);
	}
	
	protected void take2_Exit()
	{
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
