using UnityEngine;
using System.Collections;

public class BossPangziSM : EnemyController {
	private bool isFirst = true;
	private GameObject bossHP;
	//	private bool isTaked = false;
	void Awake()
	{
	}
	
	// Use this for initialization
	void Start () {
		bossHP = (GameObject)Instantiate(Resources.Load("UI/bossHP"));
		bossHP.transform.parent = UIBattleSceneLogic.Instance.NGUICamera.transform;
		bossHP.transform.localScale = Vector3.one;
		StartCoroutine(GotoNextSkillRound());
	}
	
	// Update is called once per frame
	void Update () {
		if (bossHP != null && bossHP.GetComponent<UIBossHP>().bossHP.value > 0)
		{
			bossHP.transform.position = Helper.WorldToNGUIPos(Camera.main, UIBattleSceneLogic.Instance.NGUICamera, transform.position + transform.up * 8);
			bossHP.GetComponent<UIBossHP>().bossHP.value = HP / maxHP;
			bossHP.GetComponent<UIBossHP>().status.text = GetState().ToString();
		}
		else
		{
			Destroy(bossHP);
		}
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

		if (GetState().Equals(AnimState.skill))
			yield break;

		GetComponent<Animation>().CrossFade("walk");
	}

	IEnumerator run_Enter()
	{
		if (HP <= 0)
			yield break;

		if (GetState().Equals(AnimState.skill))
			yield break;

		GetComponent<Animation>().CrossFade("walk");
	}

	protected IEnumerator thinkAttack_Enter()
	{
		if (GetState().Equals(AnimState.skill))
			yield break;

		if (HP <= 0)
			yield break;

//		if (isFirst)
//		{
//			isFirst = false;
//			StartCoroutine(GotoNextSkillRound());
//		}
		GetComponent<Animation>().CrossFade("idle");
//		GameObject effect = (GameObject)Instantiate(Resources.Load("prefabs/Effects/shengji"));
//		effect.transform.position = transform.position;
//		ParticleSystem[] pses = effect.GetComponentsInChildren<ParticleSystem>();
//		foreach(ParticleSystem ps in pses)
//		{
//			ps.time = thinkAttackTime / 2f;
//		}
		
		yield return new WaitForSeconds(thinkAttackTime);

		if (!GetState().Equals(AnimState.thinkAttack))
			yield break;

		ChangeState(AnimState.attack);
	}
	
	
	protected void thinkAttack_Update()
	{
		if (!GetState().Equals(AnimState.thinkAttack))
			StopCoroutine("thinkAttack_Enter");
	}
	
	protected void walk_Exit()
	{
	}
	
	protected IEnumerator attack_Enter()
	{
		if (GetState().Equals(AnimState.skill))
			yield break;

		if (HP <= 0)
			yield break;

		GetComponent<Animation>().CrossFade("attack");
		yield return new WaitForSeconds(1.6f);
		AttackHero();
		if (!GetState().Equals(AnimState.dead) && !GetState().Equals(AnimState.skill))
			ChangeState(AnimState.idle);
//		ChangeState(AnimState.skill);
//		yield return null;
	}
	
	protected void attack_Exit()
	{
	}

	protected IEnumerator skill_Enter()
	{
		if (HP <= 0)
			yield break;

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

		GetComponent<Animation>().CrossFade("skillprep", 0.1f, PlayMode.StopAll);
		GetComponent<PangziAdditionalEffect>().PlayEffect(EffectType.PreSKill);
		yield return new WaitForSeconds(2.0f);
		if (HP <= 0)
			yield break;

		if (!GetState().Equals(AnimState.skill))
			yield break;

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

		GetComponent<Animation>().CrossFade("skill");
		GetComponent<PangziAdditionalEffect>().PlayEffect(EffectType.Skill);
		yield return new WaitForSeconds(5.0f);
		if (HP <= 0)
			yield break;

		ChangeState(AnimState.idle);
	}

	void skill_Exit()
	{
//		StartCoroutine(GotoNextSkillRound());
	}

	IEnumerator GotoNextSkillRound()
	{
		yield return new WaitForSeconds(skill_time);
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

		ChangeState(AnimState.skill);
		yield return new WaitForSeconds(7f);
		StartCoroutine(GotoNextSkillRound());
	}
	protected IEnumerator take1_Enter()
	{
		if (HP <= 0)
			yield break;

		if (GetState().Equals(AnimState.skill))
			yield break;

		GetComponent<Animation>().CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead) && !GetState().Equals(AnimState.skill))
			ChangeState(AnimState.idle);
	}

	protected IEnumerator take2_Enter()
	{
		if (HP <= 0)
			yield break;

		if (GetState().Equals(AnimState.skill))
			yield break;

		GetComponent<Animation>().CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead) && !GetState().Equals(AnimState.skill))
			ChangeState(AnimState.idle);
	}
	
	protected IEnumerator dead_Enter()
	{
		StopAllLiveCoro();
		GetComponent<Animation>().CrossFade("dead", 0.1f, PlayMode.StopAll);
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
