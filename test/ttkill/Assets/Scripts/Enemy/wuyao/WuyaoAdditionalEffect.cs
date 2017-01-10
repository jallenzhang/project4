using UnityEngine;
using System.Collections;

public class WuyaoAdditionalEffect : EnemyAdditionalEffect {
	public GameObject attackEffectPrefab;
	public GameObject preSkillEffectPrefab;
	public GameObject skillEffectPrefab;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	protected override void PlayAttackEffect()
	{
		//		if (attackEffectPrefab != null)
		//		{
		//			GameObject effect = (GameObject)Instantiate(attackEffectPrefab);
		//			effect.transform.parent = transform;
		//			effect.transform.position =  transform.rotation * new Vector3(0, 0, 4f) + transform.position;
		//		}
	}
	
	protected override void PlayPreSkillEffect()
	{
		if (preSkillEffectPrefab != null)
		{
			GameObject effect = (GameObject)Instantiate(preSkillEffectPrefab);
			effect.transform.parent = transform;
			effect.transform.position = transform.position + new Vector3(0, 0.5f, 0);
		}

		GameObject ball = (GameObject)Instantiate(Resources.Load("prefabs/Effects/wuyao_ball"));
		ball.transform.parent = transform;
		ball.transform.position = transform.position + new Vector3(0, 5, 0) + transform.rotation * new Vector3(0, 0, 4);
	}
	
	protected override void PlaySkillEffect()
	{
		if (skillEffectPrefab != null)
		{
			GameObject effect = (GameObject)Instantiate(skillEffectPrefab);
			effect.transform.parent = transform;
			effect.transform.position = transform.position + new Vector3(0, 0.3f, 0);
			effect.transform.rotation = transform.rotation;
		}
		
		StartCoroutine(GenerateSkillHoles(20));
	}
	
	IEnumerator GenerateSkillHoles(int waveCount)
	{
		int i = 0;
		while(i < waveCount)
		{
			GameObject bullet = (GameObject)Instantiate(Resources.Load("prefabs/Effects/boss_wuyao_skill01"));
			bullet.GetComponent<WuyaoHole>().skill_damage = GetComponent<EnemyController>().skill_atk;
			bullet.transform.parent = transform.parent;
			float x = Random.Range(-24f, 22f);
			float z = Random.Range(-26f, 20f);
			bullet.transform.position = new Vector3(x, 0.6f, z);
			i++;
			yield return new WaitForSeconds(0.2f);
		}
	}
}
