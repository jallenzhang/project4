using UnityEngine;
using System.Collections;

public class NiuAdditionalEffect : EnemyAdditionalEffect {
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
		if (attackEffectPrefab != null)
		{
			GameObject effect = (GameObject)Instantiate(attackEffectPrefab);
			effect.transform.parent = transform;
			effect.transform.position =  transform.rotation * new Vector3(0, 0, 4f) + transform.position;
		}
	}
	
	protected override void PlayPreSkillEffect()
	{
		if (preSkillEffectPrefab != null)
		{
			GameObject effect = (GameObject)Instantiate(preSkillEffectPrefab);
			effect.transform.parent = transform;
			effect.transform.position = transform.position;
		}
	}
	
	protected override void PlaySkillEffect()
	{
		if (skillEffectPrefab != null)
		{
			GameObject effect = (GameObject)Instantiate(skillEffectPrefab);
			effect.transform.parent = transform;
			effect.transform.position = transform.position ;//+ transform.rotation * new Vector3(0, 0, 3); 
			effect.transform.rotation = transform.rotation;

		}
	}
}
