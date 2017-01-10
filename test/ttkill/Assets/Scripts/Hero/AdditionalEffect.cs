using UnityEngine;
using System.Collections;

public enum FuType
{
	huifu,
	jisu,
	kuangbao,
	zhaohuan,
}

/*
 *1.增加XX点移动速度。 你会发现value中和描述中的值不对应。
 *那是因为描述中为了好看，所以将那个值*10，实际上吃符就是在原有的移动速度上增加“value”相应的值
 *2.恢复XX点血量，这个很简单，不超过血量上限就可以了
 *3。增加100%的攻击力，其实就是双倍攻击。 这个符的升级 增加的是这个符的存在时间，这个也应该比较简单
 *4.召唤一个分身，这个符的变量是 分身拥有XX点生命值。
（1）吃到这个符的时候，在符的原地留下一个玩家的残影，
（2）这个残影保留用户吃到这个符时候所携带的武器且可以攻击（不会移动）
（3）这个残影继承了主体30%的攻击力，且子弹不会用完
（4）当有残影在战场上的时候，怪物的目标不是玩家本人，而是这个残影。  直到这个残影消失
（5）残影消失的条件：要么被怪打死，要么过了10秒后自动消失
 */
public class AdditionalEffect : MonoBehaviour {
	public GameObject huifuPrefab;
	public GameObject jisuPrefab;
	public GameObject kuangbaoPrefab;
	public GameObject zhaohuanPrefab;

	public GameObject fake01;

	// Use this for initialization
	void Start () {
		EventService.Instance.GetEvent<FuEvent>().Subscribe(PlayEffect);
	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<FuEvent>().Unsubscribe(PlayEffect);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlayEffect(FuType type)
	{
		switch(type)
		{
		case FuType.huifu:
			StartCoroutine(Huifu());
			break;
		case FuType.jisu:
			StartCoroutine(Jisu());
			break;
		case FuType.kuangbao:
			StartCoroutine(Kuangbao());
			break;
		case FuType.zhaohuan:
			GameObject zhaohuan = (GameObject)Instantiate(zhaohuanPrefab);
			zhaohuan.transform.parent = transform;
			zhaohuan.transform.position = transform.position + new Vector3(0, 2.5f, 0);
			StartCoroutine(Zhaohuan());
			break;
		}
	}

	IEnumerator Huifu()
	{
		var hc = GetComponent<HeroController>();
		if (hc != null)
		{
			GameObject fu = (GameObject)Instantiate(huifuPrefab);
			fu.transform.parent = transform;
			fu.transform.position = transform.position + new Vector3(0, 0.2f, 0);
			Ultilities.gm.audioScript.recoverFX.play();
			int lv = IOHelper.GetCurrentFuLv(FuItem.Type.Restore);
			var fuInfo = IOHelper.GetFuInfo(FuItem.Type.Restore, lv);
			float hpValue = (fuInfo == null ? 500 : fuInfo.value);
			hc.hp += hpValue;

			///add HP animation
			GameObject addHP = (GameObject)Instantiate(Resources.Load("UI/labelAddHP"));
			addHP.GetComponent<UILabel>().text = "+" + ((int)hpValue).ToString();
			addHP.transform.parent = UIBattleSceneLogic.Instance.NGUICamera.transform;
			addHP.transform.localScale = Vector3.one;
			addHP.transform.position = Helper.WorldToNGUIPos(Camera.main, UIBattleSceneLogic.Instance.NGUICamera, transform.position + transform.up*4);
			TweenScale.Begin(addHP, 0.6f, Vector3.one * 2);

			if (hc.hp > hc.maxHp) hc.hp = hc.maxHp;
			UIBattleSceneLogic.Instance.SetHp(hc.hp / hc.maxHp);
			yield return new WaitForSeconds(1);
			Destroy(fu);
		}
		yield break;
	}

	IEnumerator Jisu()
	{
		var hc = GetComponent<HeroController>();
		if (hc != null)
		{
//			GameObject jisu = (GameObject)Instantiate(jisuPrefab);
//			jisu.transform.parent = transform;
//			jisu.transform.position = transform.position + new Vector3(0, 0.2f, 0);

			int lv = IOHelper.GetCurrentFuLv(FuItem.Type.Speed);
			var fu = IOHelper.GetFuInfo(FuItem.Type.Speed, lv);
			hc.fuSpeed = fu == null ? 5 : fu.value;
			GetComponent<BakeMeshTest>().StartBake();
			yield return new WaitForSeconds(10);
			GetComponent<BakeMeshTest>().StopBake();
			hc.fuSpeed = 0;
//			Destroy(jisu);
		}
	}

	IEnumerator Kuangbao()
	{
//		GameObject kuangbao = (GameObject)Instantiate(kuangbaoPrefab);
//		kuangbao.transform.parent = transform;
//		kuangbao.transform.position = transform.position + new Vector3(0, 0.2f, 0);
		transform.localScale = Vector3.one * 1.2f;
		EnemyController.damageTimes = 2;
		int lv = IOHelper.GetCurrentFuLv(FuItem.Type.Fury);
		var fu = IOHelper.GetFuInfo(FuItem.Type.Fury, lv);
//		Debug.Log("yield return new WaitForSeconds(fu == null ? 5 : fu.value); " + fu.value);
		yield return new WaitForSeconds(fu == null ? 5 : fu.value);
		transform.localScale = Vector3.one;
		EnemyController.damageTimes = 1;
//		Destroy(kuangbao);
	}

	IEnumerator Zhaohuan()
	{
		GameObject go = Instantiate(fake01) as GameObject;
		go.transform.position = transform.position + new Vector3(2f, 0, 2f);
		go.AddComponent<Zhaohuan>();
		yield break;
	}
}


public class Zhaohuan : MonoBehaviour
{
	IEnumerator Start()
	{
		Destroy(GetComponent<AdditionalEffect>());
		EventService.Instance.GetEvent<FakeEvent>().Publish(true);
		rigidbody.constraints |= RigidbodyConstraints.FreezePosition;

		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<FakeEvent>().Publish(false);
	}
}