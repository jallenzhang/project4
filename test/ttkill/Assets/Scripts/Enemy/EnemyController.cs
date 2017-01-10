using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using PathologicalGames;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : StateMachineBehaviour {
	[HideInInspector]
	public int EnemyId = 0;
	[HideInInspector]
	public float HP = 10;
	[HideInInspector]
	public float maxHP = 10;
	[HideInInspector]
	public float atk;
	[HideInInspector]
	public float skill_atk;

	public static float damageTimes = 1;

	public float ViewRange = 50f;
	/// <summary>
	/// 怪物攻击距离
	/// </summary>
	[HideInInspector]
	public float AttackRange = 2f;
	/// <summary>
	/// 怪物旋转速度
	/// </summary>
	public float turningSmoothing = 0.3f;
	/// <summary>
	/// 怪物速度
	/// </summary>
	[HideInInspector]
	public float speed = 1.0f;
	/// <summary>
	/// 怪物加速度
	/// </summary>
	public float velocitySnapness = 30.0f;

	public float thinkAttackTime = 1f;

	public float skill_time = 0;

	[HideInInspector]
	public GameObject Player;

	protected Vector3 faceDir;

	public EnemyType currentType;

	private bool bFreezed = false;

	public int Drop = 0; //0:nothing; 1:gold; 2:weapon

	private int score = 0;

	private List<Vector3> paths = new List<Vector3>();
	private Vector3 randomPos = Vector3.zero;
	private bool onceTake = true;
//	private float averageHP = 10;
	private NavMeshAgent nma;

	void Start()
	{
//		Physics.IgnoreCollision(Player.collider, collider);
	}

	public void InitData(int id)
	{
//		StartCoroutine(Init());
		MonsterInfo monster = IOHelper.GetMonsterInfo(id);
		currentType = (EnemyType)id;
		AttackRange = monster.atk_range;
		speed = monster.move_speed;
		maxHP = HP = monster.hp_Proportion * GameData.Instance.EnemyAverageHP;
//		if (Drop != 0)
//			maxHP = HP = GameData.Instance.EnemyAverageHP * 10;
		atk = monster.atk;
		skill_atk = monster.skill_atk;
		thinkAttackTime = monster.atk_time;
		skill_time = monster.skill_time;
		ViewRange = monster.Visual_range;
		score = monster.score;
		nma = GetComponent<NavMeshAgent>();
		Init();
	}

	void FixedUpdate()
	{
		if (Time.timeScale == 0)
			return;

		if (Player == null)
			return;

		if (bFreezed)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			return;
		}
	
		if (transform.position.y > 0.8f)
			return;

		var state = GetState();
		
		if (state == null) return;
		
		if (state.Equals(AnimState.dead))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			return;
		}

		if (state.Equals(AnimState.attack) && currentType >= EnemyType.boss_niu)
		{
			return;
		}

		if (state.Equals(AnimState.take1))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			return;
		}

		if (state.Equals(AnimState.take2))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			return;
		}

		if (state.Equals(AnimState.skill))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			return;
		}

		if (state.Equals(AnimState.thinkAttack))
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			return;
		}

		if (HP < 0)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			return;
		}

//		if (state.Equals(AnimState.attack))
//			return;
		
		float distance = Vector3.Distance (Player.transform.position, transform.position);
		faceDir = (Player.transform.position - transform.position).normalized;
		float rotationAngle = Ultilities.AngleAroundAxis (transform.forward, faceDir, Vector3.up);
		rigidbody.angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
		
		if (distance <= ViewRange)// && Drop == 0)
		{
			paths.Clear();
			randomPos = Vector3.zero;
			if (distance <= AttackRange - 0.3f)
			{
				if (!GetState().Equals(AnimState.attack))
				{
					rigidbody.velocity = Vector3.zero;

					ChangeState(AnimState.thinkAttack);
				}
			}
			else if (distance <= ViewRange * 0.6f)
			{
//				nma.speed = speed;
//				nma.SetDestination(Player.transform.position);
				if (!GetState().Equals(AnimState.thinkAttack))
				{
					Vector3 targetVelocity = faceDir * speed;
					Vector3 deltavelocity = targetVelocity - rigidbody.velocity;
					rigidbody.AddForce (deltavelocity * velocitySnapness, ForceMode.Force);
					ChangeState(AnimState.run);
				}
			}
			else if (HP > 0)
			{
//				nma.speed = speed * 0.6f;
//				nma.SetDestination(Player.transform.position);
				Vector3 targetVelocity = faceDir * speed * 0.6f;
				Vector3 deltavelocity = targetVelocity - rigidbody.velocity;
				rigidbody.AddForce (deltavelocity * velocitySnapness, ForceMode.Force);
				ChangeState(AnimState.walk);
			}
		}
		else
		{
//			if (paths.Count == 0)
//				GenerateNewPos();
//
//			if (Vector3.Distance(randomPos, transform.position) < 0.2f || randomPos == Vector3.zero)
//			{
//				randomPos = paths[Random.Range(0, 4)];
//			}
//			faceDir = (randomPos - transform.position).normalized;
//
//			Vector3 targetVelocity = faceDir * speed * 0.2f;
//			Vector3 deltavelocity = targetVelocity - rigidbody.velocity;
//			rigidbody.AddForce (deltavelocity * velocitySnapness, ForceMode.Force);
//			ChangeState(AnimState.walk);
		}
	}

	Vector3 burnNewPos()
	{
		Vector3 pathPos = Vector3.zero;
		
		float angle = Random.Range(0f, 360f);
		pathPos = transform.position + Quaternion.Euler(new Vector3(0, angle, 0)) * transform.localRotation * new Vector3(0, 0, 5);

		return pathPos;
	}

	void GenerateNewPos()
	{
		paths.Clear();
		for (int i = 0; i < 4; i++)
		{
			Vector3 pathPos = burnNewPos();
		    
			while(!EnemyManager.Instance.ValidatePoint(pathPos))
			{
				pathPos = burnNewPos();
			}

			paths.Add(pathPos);
		}
	}

	public void Init()
	{


		Player = (GameObject)GameObject.FindGameObjectWithTag ("Player");

		if (GameObject.FindGameObjectWithTag ("Fake") != null)
			Player = (GameObject)GameObject.FindGameObjectWithTag ("Fake");

		EventService.Instance.GetEvent<FakeEvent>().Subscribe(FakeEventRaise);
		Initialize<AnimState>();
//		yield return new WaitForSeconds(1f);
		ChangeState(AnimState.thinkAttack);
//		yield return null;

//		GenerateNewPos();

	}

	void FakeEventRaise(bool show)
	{
		if (show)
			Player = (GameObject)GameObject.FindGameObjectWithTag ("Fake");
		else
			Player = (GameObject)GameObject.FindGameObjectWithTag("Player");
	}

	void DropGold()
	{
		GameObject effect = (GameObject)Instantiate(Resources.Load("prefabs/Effects/drop_gold"));
//		Transform effect = PoolManager.Pools["Items"].Spawn("drop_gold");
//		effect.transform.parent = transform.parent;
		effect.transform.position = transform.position;
		
		if (Drop == 1)
		{
			for (int i = 0; i < 2; i++)
			{
				GameObject gold = (GameObject)Instantiate(Resources.Load("prefabs/gold/jinbi_large"));
//				Transform gold = PoolManager.Pools["Items"].Spawn("jinbi_large");
				gold.transform.position = new Vector3(transform.position.x, 0.8f, transform.position.z) + new Vector3(0.5f * i, 0, 0);
				gold.transform.localScale = Vector3.one * 0.3f;
			}
		}
	}

	public void onDamage(float damage, Vector3 hitPos)
	{
		if (HP < 0)
			return;

		if (GetState().Equals(AnimState.skill))
			return;

		if (Player == null)
		{
			Debug.Log("Player is null ");
			return;
		}
		else
		{
			if ( Player.GetComponent<HeroController>() == null)
			{
				Debug.Log("player name is: " + Player.name);
			}
		}

		damage = damage + damage * SettingManager.Instance.WeaponAddtional * 0.2f;

		damage *= damageTimes;

		float r = Random.Range(0, 100f);
		if (r < Player.GetComponent<HeroController>().Probability)
		{
			damage += damage * Player.GetComponent<HeroController>().crit;

//			GameObject duang = Instantiate(Resources.Load("prefabs/Effects/duang")) as GameObject;
//			duang.transform.parent = transform;
			SpawnPool spawnPool = PoolManager.Pools["Items"];
			Transform duang = spawnPool.Spawn("duang");

			duang.position = new Vector3(hitPos.x, 1, hitPos.z);
			duang.localScale = Vector3.one;
		}

		HP -= damage;

		///decrease HP animation
		GameObject minusHP = (GameObject)Instantiate(Resources.Load("UI/labelDropHP"));
		minusHP.GetComponent<UILabel>().text = "-" + ((int)damage).ToString();
		minusHP.transform.parent = UIBattleSceneLogic.Instance.NGUICamera.transform;
		minusHP.transform.localScale = Vector3.one;
		minusHP.transform.position = Helper.WorldToNGUIPos(Camera.main, UIBattleSceneLogic.Instance.NGUICamera, transform.position + transform.up*2);
		TweenScale.Begin(minusHP, 0.3f, Vector3.one * 2);

		//create effect
//		GameObject effectflash = Instantiate(Resources.Load("prefabs/Effects/Blood")) as GameObject;
		Transform effectflash = PoolManager.Pools["Items"].Spawn("Blood");
//		effectflash.transform.parent = transform;
		effectflash.position = new Vector3(hitPos.x, 1, hitPos.z);
		effectflash.localScale = Vector3.one;


		if (HP <= 0 && !GetState().Equals(AnimState.dead)) {
			ChangeState(AnimState.dead);
			transform.FindChild("yinying").gameObject.SetActive(false);
//			nma.Stop();
			rigidbody.velocity = Vector3.zero;

			//pet feature
			if (Player.GetComponent<HeroController>().isRecovered && Random.Range(0, 100) < 4)
			{
				Player.GetComponent<HeroController>().hp 
					= Mathf.Min(Player.GetComponent<HeroController>().hp + Player.GetComponent<HeroController>().maxHp * Player.GetComponent<HeroController>().recoveredPercent, 
					            Player.GetComponent<HeroController>().maxHp);
				UIBattleSceneLogic.Instance.SetHp(Player.GetComponent<HeroController>().hp / Player.GetComponent<HeroController>().maxHp);
			}

			GameObject bloodEffect = (GameObject)Instantiate(Resources.Load("prefabs/xue"));
			bloodEffect.transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z) ;

			int r1 = Random.Range(0, 100);
			if (((int)currentType < 100 && r1 < 15 && Drop == 0) || Drop == 1)
			{
				DropGold();
			}
			else if ((int)currentType < 100 &&  Drop == 2)
			{
				if (WeaponManager.Instance.transform.childCount <5)
				{
					float x = Random.Range(-5f, 5f);
					float z = Random.Range(-5f, 5f);
					Vector3 pos = new Vector3(x, 0.8f, z) + transform.position;
					WeaponManager.Instance.GenerateWeapon(WeaponManager.RandomWeapon(), pos);
				}
				else
				{
					DropGold();
				}
			}
			else if ((int)currentType >= 100)
			{
				GameObject effect = (GameObject)Instantiate(Resources.Load("prefabs/Effects/drop_gold"));
//				Transform effect = PoolManager.Pools["Items"].Spawn("drop_gold");
//				effect.transform.parent = transform.parent;
				effect.transform.position = transform.position;

				if ((int)currentType >= (int)EnemyType.boss_niu)
				{
					SettingManager.Instance.KillBossTime += 1;
					SettingManager.Instance.KillBoss_Shuaxin += 1;
					SettingManager.Instance.TotalBossKill += 1;
				}

				MonsterInfo monster = IOHelper.GetMonsterInfo((int)currentType);
				int count = (int)monster.money / 10; //

				if (EnemyManager.Instance.mode == PlayModes.OnlyBoss 
				    && GameData.Instance.LevelType == LevelType.RushLevel
				    && currentType >= EnemyType.boss_niu)
				{
					count *= 3;
				}

				for (int i = 0; i < count; i++)
				{
					GameObject gold = (GameObject)Instantiate(Resources.Load("prefabs/gold/jinbi_large"));
//					Transform gold = PoolManager.Pools["Items"].Spawn("jinbi_large");
					gold.transform.position = transform.position + Quaternion.Euler(new Vector3(0,360 * i/count,0)) * transform.rotation * Vector3.forward * 3;
					gold.transform.position = new Vector3(gold.transform.position.x, 0.1f, gold.transform.position.z);
					gold.transform.localScale = Vector3.one * 0.3f;
				}
			}

			if (GameData.Instance.LevelType == LevelType.InfiniteLevel)
				UIBattleSceneLogic.Instance.AddScore(score);
			EnemyManager.Instance.RemoveEnemyById(EnemyId);
			EventService.Instance.GetEvent<EnemyDeadEvent> ().Publish(EnemyId);
		}
		else if (HP > 0)
		{
			onceTake = !onceTake;
			if (onceTake)
			{
				if ( !GetState().Equals(AnimState.skill))
					ChangeState(AnimState.take1);
			}
			else
			{
				if ( !GetState().Equals(AnimState.skill))
					ChangeState(AnimState.take2);
			}
		}
	}

	/// <summary>
	/// Generators the gold.
	/// </summary>
	/// <returns>The gold.</returns>
	/// <param name="pos">Position.</param>
	/// <param name="type">1:little gold 2:middle gold 3.large gold</param>
	IEnumerator GeneratorGold(Vector3 pos, int type)
	{
		yield return new WaitForSeconds(4);
		string goldPath = "prefabs/gold/jinbi_little";
		if (type == 2)
			goldPath = "prefabs/gold/jinbi_middle";
		else if (type ==3)
			goldPath = "prefabs/gold/jinbi_large";

		GameObject gold = (GameObject)Instantiate(Resources.Load(goldPath));
		gold.transform.position = pos;
		gold.transform.localScale = Vector3.one;
	}

	public void AttackHero()
	{
		float distance = Vector3.Distance (Player.transform.position, transform.position);
//		Debug.Log("distance is:  " + distance + " AttackRange " + AttackRange);
		if (distance <= AttackRange)
			AttackHero(atk);
	}

	public void AttackHero(float damage)
	{
		if (Player != null)
		{
			HeroController hc = Player.GetComponent<HeroController>();
			if (hc != null)
			{
				hc.GetHit(damage);
			}
		}
	}

	void ExploreGolds()
	{
		int goldNum = Random.Range(4, 30);
		Vector3 explosionPoint = new Vector3(transform.position.x, -1, transform.position.z);

		for (int i = 0; i < goldNum; i++)
		{
			float x = Random.Range(-0.5f, 0.5f);
			float z = Random.Range(-0.5f, 0.5f);
			float power = Random.Range(5, 15);

			Vector3 goldPos = transform.position + new Vector3(x, -transform.position.y, z);
			Vector3 dir = goldPos - explosionPoint; //new Vector3(transform.position.x, 1, transform.position.z);
			GameObject gold = (GameObject)Instantiate(Resources.Load("prefabs/jinbi"));
			gold.transform.position = goldPos;
			gold.transform.localScale = Vector3.one;

			gold.rigidbody.AddForce(dir * power, ForceMode.Impulse);
		}
	}

	private Coroutine cor = null;
	public void StartFreezeEnemy(float duration)
	{
		if (cor != null)
			StopCoroutine(cor);

		if ((int)currentType <= 100 && NGUITools.GetActive(gameObject))
			cor = StartCoroutine(FreezeEnemy(duration));
		else
		{
			Debug.Log("currentType is: " + currentType + "  ");
		}
	}

	IEnumerator FreezeEnemy(float duration)
	{
		bFreezed = true;
//		StartCoroutine(IceFreeze(duration));
		rigidbody.velocity = Vector3.zero;
//		rigidbody.angularVelocity = Vector3.zero;
//		nma.Stop();
		Material[] oldMat = transform.GetComponentInChildren<SkinnedMeshRenderer>().materials;
		transform.GetComponentInChildren<SkinnedMeshRenderer>().materials = EnemyManager.Instance.IceMaterial;

		GameObject bingGas = (GameObject)Instantiate(Resources.Load("prefabs/Effects/daoju_bingdong02"));
		bingGas.transform.parent = transform;
		bingGas.transform.localPosition = new Vector3(0, 6, 0);
		bingGas.transform.localScale = Vector3.one;
//		transform.GetComponent<Animation>().Stop();
		if (animation["idle"] != null)
			animation["idle"].speed = 0;
		else
			animation["walk"].speed = 0;
		yield return new WaitForSeconds(duration);
		transform.GetComponentInChildren<SkinnedMeshRenderer>().materials = oldMat;
//		transform.GetComponent<Animation>().Play();
//		nma.SetDestination(Player.transform.position);
		bFreezed = false;
		if (animation["idle"] != null)
			animation["idle"].speed = 1;
		else
			animation["walk"].speed = 1;
		cor = null;

		Destroy(bingGas);
	}

	IEnumerator IceFreeze(float duration)
	{
		GetComponentInChildren<FreezeBehaviour>().isFrozen = true;
		yield return new WaitForSeconds(duration / 2f);
		GetComponentInChildren<FreezeBehaviour>().isFrozen = false;
	}
}
