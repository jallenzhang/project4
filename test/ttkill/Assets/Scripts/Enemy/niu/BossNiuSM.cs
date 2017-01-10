using UnityEngine;
using System.Collections;

public class BossNiuSM : EnemyController {

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
			bossHP.transform.position = Helper.WorldToNGUIPos(Camera.main, UIBattleSceneLogic.Instance.NGUICamera, transform.position + transform.forward *3 + transform.up * 8);
			bossHP.GetComponent<UIBossHP>().bossHP.value = HP / maxHP;
			bossHP.GetComponent<UIBossHP>().status.text = GetState().ToString();
		}
		else
		{
			Destroy(bossHP);
		}
	}
	
	protected void idle_Enter()
	{
		animation.CrossFade("idle");
	}
	
	protected void idle_Exit()
	{
	}
	
	IEnumerator walk_Enter()
	{
		if (GetState().Equals(AnimState.thinkAttack))
			yield break;

		if (GetState().Equals(AnimState.skill))
			yield break;

		animation.CrossFade("walk", 0.1f, PlayMode.StopAll);
		yield return null;
	}

	IEnumerator run_Enter()
	{
		if (GetState().Equals(AnimState.thinkAttack))
			yield break;

		if (GetState().Equals(AnimState.skill))
			yield break;

		animation.CrossFade("walk", 0.1f, PlayMode.StopAll);
		yield return null;
	}
	
	protected IEnumerator thinkAttack_Enter()
	{
		if (GetState().Equals(AnimState.skill))
		    yield break;

		animation.CrossFade("idle", 0.1f, PlayMode.StopAll);
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
//		if (!GetState().Equals(AnimState.thinkAttack))
//			StopCoroutine("thinkAttack_Enter");
	}
	
	protected void walk_Exit()
	{
	}
	
	protected IEnumerator attack_Enter()
	{
		if (GetState().Equals(AnimState.skill))
			yield break;

		animation.CrossFade("attack", 0.1f, PlayMode.StopAll);
		yield return new WaitForSeconds(1f);
		AttackHero();
		yield return new WaitForSeconds(1f);
		if (!GetState().Equals(AnimState.dead) && !GetState().Equals(AnimState.skill))
			ChangeState(AnimState.idle);

//		ChangeState(AnimState.skill);
//		yield return null;
	}

	protected void attack_Exit()
	{
//		StartCoroutine(skill_Exit());
	}

	public IEnumerator GotoNextSkillRound()
	{
		yield return new WaitForSeconds(skill_time);
		ChangeState(AnimState.skill);
		yield return new WaitForSeconds(6f);
		StartCoroutine(GotoNextSkillRound());
	}

	protected IEnumerator skill_Enter()
	{
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.velocity = Vector3.zero;
		animation.CrossFade("skillprep", 0.1f, PlayMode.StopAll);
		GetComponent<NiuAdditionalEffect>().PlayEffect(EffectType.PreSKill);
		yield return new WaitForSeconds(2f);
		if (!GetState().Equals(AnimState.skill))
			yield break;
		animation.CrossFade("skill", 0.1f, PlayMode.StopAll);
		GetComponent<NiuAdditionalEffect>().PlayEffect(EffectType.Skill);
		StartCoroutine(ContinousMove(4f, faceDir));
		yield return new WaitForSeconds(4f);
		rigidbody.velocity = Vector3.zero;
	}

	IEnumerator ContinousMove(float duration, Vector3 dir){
		float rate = 1f / duration;
		float tmp = 0f;
		bool hurt =false;
		while (tmp < 1.0f)
		{
			tmp += Time.deltaTime * rate;
//			Vector3 targetVelocity = dir * 12;
//			Vector3 deltavelocity = targetVelocity - rigidbody.velocity;
//			rigidbody.AddForce (deltavelocity * velocitySnapness, ForceMode.Force);
			transform.Translate(Vector3.forward * Time.deltaTime * speed * 2f, Space.Self);
			float distance = Vector3.Distance (Player.transform.position, transform.position);
			if (distance <= AttackRange && !hurt)
			{
				AttackHero(skill_atk);
				hurt = true;
			}

			if (!GetState().Equals(AnimState.skill))
				yield break;

			yield return null;
		}

		ChangeState(AnimState.idle);
	}
	
	protected IEnumerator take1_Enter()
	{
		//		isTaked = true;
		if (GetState().Equals(AnimState.skill))
		    yield break;
		animation.CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead) && !GetState().Equals(AnimState.skill))
			ChangeState(AnimState.idle);
	}
	
	protected void take1_Exit()
	{
		//		isTaked = false;
	}

	protected IEnumerator take2_Enter()
	{
		//		isTaked = true;
		if (GetState().Equals(AnimState.skill))
			yield break;

		animation.CrossFade("take");
		yield return new WaitForSeconds(0.5f);
		if (!GetState().Equals(AnimState.dead) && !GetState().Equals(AnimState.skill))
			ChangeState(AnimState.idle);
	}
	
	protected void take2_Exit()
	{
		//		isTaked = false;
	}
	
	protected IEnumerator dead_Enter()
	{
		animation.CrossFade("dead");
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		collider.enabled = false;
		rigidbody.useGravity = false;

		yield return new WaitForSeconds(1f);
		rigidbody.useGravity = true;
		yield return new WaitForSeconds(1f);
		EnemyManager.Instance.Enemies.Remove(GetComponent<EnemyController>());
		EventService.Instance.GetEvent<EnemyDeadEvent> ().Publish (EnemyId);
		DestroyImmediate(gameObject);
	}
	
	protected void dead_Exit()
	{
		
	}
}
