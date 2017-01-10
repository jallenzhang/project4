using UnityEngine;
using System.Collections;

public class PangziAdditionalEffect : EnemyAdditionalEffect {

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
		
		StartCoroutine(GenerateSkillWaves(4));
	}
	
	IEnumerator GenerateSkillWaves(int waveCount)
	{
		int i = 0;
		while(i < waveCount)
		{
			GameObject bullet = (GameObject)Instantiate(Resources.Load("prefabs/Effects/pangzi_wave"));
//			bullet.GetComponent<WaBullet>().Init(transform, 10f);
			bullet.transform.parent = transform;
			bullet.transform.localPosition = new Vector3(0, 3, 0);
//			bullet.transform.position = transform.position + new Vector3(0, 2, 0) + Quaternion.Euler(new Vector3(0, 360f * 2 * i / bulletCount, 0)) * new Vector3(0, 0, 3);
			bullet.transform.localRotation = Quaternion.Euler(new Vector3(0, 360f * i/ (float)waveCount, 0));
//			if (i < bulletCount / 2)
//				bullet.transform.localRotation = Quaternion.Euler(new Vector3(0, 360f * 2 * i / bulletCount, 0));
//			else
//				bullet.transform.localRotation = Quaternion.Euler(new Vector3(0, 360f * 2 * i / bulletCount + 360f / bulletCount, 0));
			i++;
			yield return new WaitForSeconds(0.05f);
		}
	}
}
