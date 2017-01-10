using UnityEngine;
using System.Collections;

public enum EffectType
{
	Attack,
	PreSKill,
	Skill
}

public class EnemyAdditionalEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayEffect(EffectType type)
	{
		switch(type)
		{
		case EffectType.Attack:
			PlayAttackEffect();
			break;
		case EffectType.PreSKill:
			PlayPreSkillEffect();
			break;
		case EffectType.Skill:
			PlaySkillEffect();
			break;
		}
	}

	protected virtual void PlayAttackEffect()
	{

	}

	protected virtual void PlayPreSkillEffect()
	{
	}

	protected virtual void PlaySkillEffect()
	{

	}
}
