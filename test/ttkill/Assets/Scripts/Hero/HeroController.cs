using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;

[RequireComponent(typeof(Rigidbody))]
public class HeroController : MonsterLove.StateMachine.StateMachineBehaviour {
	public Vector3 dir = Vector3.zero;
	public float speed = 5.0f;
	[HideInInspector]
	public float fuSpeed = 0;
	public float velocitySnapness = 5.0f;
	public float turningSmoothing = 0.3f;
	public int heroId = 1;
	public WeaponType currentWeaponId = WeaponType.bangqiugun;
	public bool isAutoAim = true;
	public float AutoAimEnemyDuration = 3f;
	//	public float frequency = 10f;
	public bool isFired = false;
	public Transform spawnPointR;
	public Transform spawnPointL;
	public float chopAngles = 60f;
	public float maxHp = 1000;
	public float Probability = 0;
	public float crit = 0;
	public GameObject followSmoke;
	public Transform bulletsArea;
	public GameObject zidanke;
	public bool automatic = false;
	
	public GameObject daoguangObj;
	
	[HideInInspector] public float hp;
	[HideInInspector] public float hpAddtional = 0;
	[HideInInspector] public float attackAddtional = 0;
	[HideInInspector] public float hitDistance;
	[HideInInspector] public bool isPlayer = true; // is player, or zhaohuan hero
	[HideInInspector] public bool isRecovered = false;
	[HideInInspector] public float recoveredPercent = 0f;
	[HideInInspector] public bool isSpeedUp = false;
	[HideInInspector] public float speedUpPercent = 0;
	[HideInInspector] public float capacityUpPercent = 0;
	
	private EnemyController aimedController = null;
	//	private float lastAimTime = 0;
	//	private JoyStick joystick_left;
	private JoyStick joystick_right;
	
	private AttackButton btnFire;
	private float lastFireTime = 0;
	// Use this for initialization
	public GameObject joystick;
	private GameObject joystickGo;
	public Vector3 RockerOriginalPos = new Vector3(0.13f, 0.1f, 0);
	private Joystick joystickLeft;
	
	private AnimState currentState = AnimState.idle;
	
	void Awake()
	{
		if (!automatic)
		{
			joystickGo = Instantiate(joystick) as GameObject;
			joystickGo.transform.localPosition = RockerOriginalPos;
			joystickLeft = joystickGo.GetComponent<Joystick>();
			
			//			joystick_left = GameObject.Find ("joystick_left").GetComponent<JoyStick>();
			btnFire = GameObject.Find ("joystick_right").GetComponent<AttackButton>();
			SubscribeEvents ();
			StartCoroutine(DropWeaponAfterDuration(2f));
			Time.timeScale = 1.2f;
			GameData.Instance.goldAdditionValue = 0;
			///pet
			System.Array petArray = System.Enum.GetValues(typeof(PetType));
			int i = 0;
			foreach(var p in petArray)
			{
				int level = PetDB.Instance.GetPetLvById((int)p);
				int onBattle = PetDB.Instance.GetPetOnBattleById((int)p);
				if (onBattle == 1)
				{
					float xoffset = 0;
					float zoffset = 0;
					switch((PetType)p)
					{
					case PetType.songshu:
						speed = speed * (1f + IOHelper.GetPetInfoByIdAndLevel((int)p, level).value);
						GameObject petSongshu = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet_songshu"));
						xoffset = Random.Range(-3f, 3f);
						zoffset = Random.Range(-3f, 3f);
						petSongshu.transform.position = transform.position + new Vector3(xoffset, 0, zoffset);
						break;
					case PetType.tuzi:
						GameObject petTuzi = (GameObject)Instantiate(Resources.Load("prefabs/pet/tuzi"));
						xoffset = Random.Range(-5f, 5f);
						zoffset = Random.Range(-5f, 5f);
						petTuzi.transform.position = transform.position + new Vector3(xoffset, 0, zoffset);
						break;
					case PetType.pet3:
						hpAddtional = IOHelper.GetPetInfoByIdAndLevel((int)p, level).value;
						GameObject pet03 = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet03"));
						xoffset = Random.Range(-5f, 5f);
						zoffset = Random.Range(-5f, 5f);
						pet03.transform.position = transform.position + new Vector3(xoffset, 0, zoffset);
						break;
					case PetType.pet4:
						isRecovered = true;
						recoveredPercent = IOHelper.GetPetInfoByIdAndLevel((int)p, level).value;
						GameObject pet04 = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet04"));
						xoffset = Random.Range(-5f, 5f);
						zoffset = Random.Range(-5f, 5f);
						pet04.transform.position = transform.position + new Vector3(xoffset, 0, zoffset);
						break;
					case PetType.pet5:
						isSpeedUp = true;
						speedUpPercent = IOHelper.GetPetInfoByIdAndLevel((int)p, level).value;
						GameObject pet05 = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet05"));
						xoffset = Random.Range(-5f, 5f);
						zoffset = Random.Range(-5f, 5f);
						pet05.transform.position = transform.position + new Vector3(xoffset, 0, zoffset);
						break;
					case PetType.pet6:
						capacityUpPercent = IOHelper.GetPetInfoByIdAndLevel((int)p, level).value;
						GameObject pet06 = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet06"));
						xoffset = Random.Range(-5f, 5f);
						zoffset = Random.Range(-5f, 5f);
						pet06.transform.position = transform.position + new Vector3(xoffset, 0, zoffset);
						break;
					case PetType.pet7:
						attackAddtional = IOHelper.GetPetInfoByIdAndLevel((int)p, level).value;
						GameObject pet07 = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet07"));
						xoffset = Random.Range(-5f, 5f);
						zoffset = Random.Range(-5f, 5f);
						pet07.transform.position = transform.position + new Vector3(xoffset, 0, zoffset);
						break;
					}
				}
				
			}
		}
		
	}
	
	IEnumerator DropWeaponAfterDuration(float duration)
	{
		if (!isPlayer) yield break;
		
		yield return new WaitForSeconds(duration);
		float x = Random.Range(-5f, 5f);
		float z = Random.Range(-5f, 5f);
		
		WeaponManager.Instance.DropWeapon(WeaponManager.RandomWeapon(), transform.position + new Vector3(x, 0, z));
	}
	
	void Start () {
		
	}
	
	void SubscribeEvents()
	{
		EventService.Instance.GetEvent<HeroStateChangeEvent> ().Subscribe (onStateChange);
		EventService.Instance.GetEvent<EnemyDeadEvent> ().Subscribe (onEnemyDead);
		EventService.Instance.GetEvent<FireChangeEvent> ().Subscribe (onFireChanged);
		EventService.Instance.GetEvent<WeaponChangeEvent> ().Subscribe (OnWeaponChange);
	}
	
	void UpdateRocker()
	{
		if (joystickGo != null)
		{
			if (Time.timeScale < 0.5f)
			{
				joystickGo.SetActive(false);
			}
			else
			{
				joystickGo.SetActive(true);
			}
		}
	}
	
	void Update()
	{
		UpdateRocker();
		
		if (!isPlayer) return;
		
		if (automatic) return;
		
		#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetKeyDown (KeyCode.F)) {
			isFired = true;
			EventService.Instance.GetEvent<FireChangeEvent>().Publish(true);
		}
		else if (Input.GetKeyUp (KeyCode.O))
		{
			isFired = false;
			EventService.Instance.GetEvent<FireChangeEvent>().Publish(false);
		}
		#else
		if (btnFire.IsPressed()) {
			if (Time.timeScale == 0)
				return;
			
			isFired = true;
			EventService.Instance.GetEvent<FireChangeEvent>().Publish(true);
		}
		else
		{
			isFired = false;		
			EventService.Instance.GetEvent<FireChangeEvent>().Publish(false);
		}
		#endif
		
		#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetKeyDown (KeyCode.A)) {
			//turn to left
			dir += Vector3.left;
		}
		
		if (Input.GetKeyDown (KeyCode.W)) {
			dir += Vector3.forward;
		}
		
		if (Input.GetKeyDown (KeyCode.S)) {
			dir += Vector3.back;
		}
		
		if (Input.GetKeyDown (KeyCode.D)) {
			dir += Vector3.right;
		}
		
		if (Input.GetKeyDown(KeyCode.T))
		{
			dir = Vector3.zero;
		}
		dir = dir.normalized;
		#else
		//		Vector2 pos = joystick_left.getPositions();
		Vector2 pos = joystickLeft.position;
		dir = new Vector3(pos.x, 0, pos.y).normalized;
		
		#endif
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GameData.Instance.Pause)
		{
			joystickLeft.ResetJoystick();
			return;
		}
		
		if (Time.timeScale < 1f)
			return;
		
		
		if (GetState().Equals(AnimState.skill))
			return;
		
		if (GetState().Equals(AnimState.daojuAttack))
			return;
		
		
		if (GetState().Equals(AnimState.dead))
			return;
		
		Vector3 targetVelocity = dir * (speed + fuSpeed);
		Vector3 deltavelocity = targetVelocity - GetComponent<Rigidbody>().velocity;
		GetComponent<Rigidbody>().AddForce(deltavelocity * velocitySnapness, ForceMode.Force);
		
		//		animation["idle"].weight = Mathf.Lerp(1, 0, Mathf.Abs(targetVelocity.magnitude));
		
		Vector3 faceDir = dir;
		if (faceDir == Vector3.zero) {			
			faceDir = transform.forward;
		}
		
		if (dir == Vector3.zero) {
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;	
		}
		
		if (isAutoAim) {
			//			if (Time.time > lastAimTime + AutoAimEnemyDuration || aimedController == null)
			{
				//				lastAimTime = Time.time;
				aimedController = EnemyManager.Instance.GetNearestEnemy(transform.position);
			}
			
			if ((isFired || automatic) && aimedController != null)
				faceDir = aimedController.transform.position - transform.position;
		}
		
		float rotationAngle =  Ultilities.AngleAroundAxis (transform.forward, faceDir, Vector3.up);
		GetComponent<Rigidbody>().angularVelocity = (Vector3.up * rotationAngle * turningSmoothing * Time.deltaTime);
		
		if (isFired || automatic)
		{
			speed = 8f;
			if (GetState().Equals(AnimState.skill))
			{
				GetComponent<Rigidbody>().angularVelocity = Vector3.zero;	
			}
			else if (Mathf.Abs(targetVelocity.magnitude) == 0)
			{
				ChangeState(AnimState.staticAttack);
			}
			else
			{
				ChangeState(AnimState.attack);
			}
			
			Weapon currentWeapon = WeaponController.Instance.GetWeaponByID(currentWeaponId);
			
			//			if (automatic)
			//				currentWeapon = GetComponent<FakeWeaponController>().currentWeapon;
			
			if (currentWeapon == null)
			{
				Debug.LogError("can't find the weapon by id " + currentWeaponId);
				return;
			}
			
			//近战
			if (currentWeaponId < WeaponType.gun_area)
			{
				if (currentWeaponId == WeaponType.dianju)
				{
					if (Time.time > lastFireTime + 1f / currentWeapon.Frequency)
					{
						lastFireTime = Time.time;
						
						Quaternion r = Quaternion.AngleAxis (chopAngles/2f, Vector3.up) * transform.rotation;
						Vector3 rPoint = r * Vector3.forward * currentWeapon.range + transform.position;
						
						Debug.DrawLine(transform.position, rPoint, Color.red, 10f);
						
						Quaternion l = Quaternion.AngleAxis(chopAngles / 2f, Vector3.down) * transform.rotation;
						Vector3 lPoint = l * Vector3.forward * currentWeapon.range + transform.position;
						Debug.DrawLine(transform.position, lPoint, Color.green, 10f);
						Ultilities.gm.audioScript.dianjuFX.play();
						
						for (int i = EnemyManager.Instance.transform.childCount - 1; i >= 0; i--)
						{
							if (EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>() != null && Helper.isINTriangle(EnemyManager.Instance.transform.GetChild(i).position, transform.position, lPoint, rPoint))
							{
								StartCoroutine(hitBack(EnemyManager.Instance.transform.GetChild(i), EnemyManager.Instance.transform.GetChild(i).position, EnemyManager.Instance.transform.GetChild(i).position + Vector3.back * (hitDistance + currentWeapon.hitDistance), 0.2f));
								
								//							EventService.Instance.GetEvent<EnemyStateChangeEvent>().Publish(AnimState.take, enemy.EnemyId);
								EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().onDamage(currentWeapon.atk, EnemyManager.Instance.transform.GetChild(i).position);
								if (!automatic)
									WeaponController.Instance.DecreaseWeaponCapacity();
								
								Ultilities.gm.audioScript.dianjuHitFX.play();
							}
						}
					}
					//					
				}
			}
		}
		else
		{
			speed = 9f;
			if (GetComponent<Rigidbody>().velocity == Vector3.zero) {
				if (!GetState().Equals(AnimState.dead))
					ChangeState(AnimState.idle);
				
			}
			else
			{
				ChangeState(AnimState.walk);
			}
		}
		
	}
	
	IEnumerator hitBack(Transform target, Vector3 from, Vector3 to, float duration)
	{
		float rate = 1f / duration;
		float tmp = 0f;
		
		if (target == null || target.GetComponent<EnemyController>() == null || target.GetComponent<EnemyController>().HP <= 0 || (int)target.GetComponent<EnemyController>().currentType > 100)
			yield break;
		
		while(tmp < 1.0f)
		{
			tmp += rate * Time.deltaTime;
			if (target == null)
				yield break;
			
			target.position = Vector3.Lerp(from ,to, tmp);
			yield return null;
		}
	}
	
	IEnumerator SpeedUpByPet()
	{
		fuSpeed = speed * speedUpPercent;
		yield return new WaitForSeconds(2);
		fuSpeed = 0;
	}
	
	public void GetHit(float damage)
	{
		if (GameData.Instance.Wudi)
			return;
		
		if (isSpeedUp && Random.Range(0, 100) < 4)
		{
			StartCoroutine(SpeedUpByPet());
		}
		
		hp -= damage;
		if (hp < 0)
		{
			hp = 0;
			ChangeState(AnimState.dead);
		}
		if (!automatic)
			UIBattleSceneLogic.Instance.SetHp(hp / maxHp);
	}
	
	private static int skillFlag = 1;
	private int skillMode = 3;
	void TryChangeToSkill()
	{
		skillFlag = skillFlag % skillMode;
		if (skillFlag == 0)
		{
			ChangeState(AnimState.skill);
		}
		skillFlag++;
	}
	
	void ChangeBack2Normal()
	{
		//		ChangeState(AnimState.attack);
	}
	
	void OnCloseInSkillHit()
	{
		Weapon currentWeapon = WeaponController.Instance.GetWeaponByID(currentWeaponId);
		//		if (automatic)
		//			currentWeapon = GetComponent<FakeWeaponController>().currentWeapon;
		
		Quaternion r = Quaternion.AngleAxis (120/2f, Vector3.up) * transform.rotation;
		Vector3 rPoint = r * Vector3.forward * currentWeapon.range + transform.position;
		
		Debug.DrawLine(transform.position, rPoint, Color.red, 10f);
		
		Quaternion l = Quaternion.AngleAxis(120 / 2f, Vector3.down) * transform.rotation;
		Vector3 lPoint = l * Vector3.forward * currentWeapon.range + transform.position;
		Debug.DrawLine(transform.position, lPoint, Color.green, 10f);
		
		for (int i = EnemyManager.Instance.Enemies.Count - 1; i >= 0; i--)
		{
			if (EnemyManager.Instance.Enemies[i] != null && Helper.isINTriangle(EnemyManager.Instance.Enemies[i].transform.position, transform.position, lPoint, rPoint))
			{
				Debug.Log("hit enemy!!!!!!!!!!");
				if (!automatic)
					WeaponController.Instance.DecreaseWeaponCapacity();
				//				Vector3 force = transform.forward * 3;
				//				EnemyManager.Instance.Enemies[i].rigidbody.AddForce(force, ForceMode.Impulse);
				//				EnemyManager.Instance.Enemies[i].transform.Translate(Vector3.back * (hitDistance + currentWeapon.hitDistance));
				StartCoroutine(hitBack(EnemyManager.Instance.Enemies[i].transform, EnemyManager.Instance.Enemies[i].transform.position, EnemyManager.Instance.Enemies[i].transform.position + Vector3.back * (hitDistance + currentWeapon.hitDistance), 0.2f));
				
				//							EventService.Instance.GetEvent<EnemyStateChangeEvent>().Publish(AnimState.take, enemy.EnemyId);
				EnemyManager.Instance.Enemies[i].onDamage(currentWeapon.atk, EnemyManager.Instance.Enemies[i].transform.position);
			}
		}
		
		GameObject effect = (GameObject)Instantiate(Resources.Load("prefabs/Effects/daoguang"));
		effect.transform.parent = transform;
		effect.transform.localRotation = Quaternion.Euler(Vector3.zero);
		effect.transform.localPosition = new Vector3(0, 2.5f, 0);
	}
	
	void OnCloseInHit()
	{
		Weapon currentWeapon = WeaponController.Instance.GetWeaponByID(currentWeaponId);
		
		//		if (automatic)
		//			currentWeapon = GetComponent<FakeWeaponController>().currentWeapon;
		
		Quaternion r = Quaternion.AngleAxis (90/2f, Vector3.up) * transform.rotation;
		if (currentWeaponId == WeaponType.gun_fire || currentWeaponId == WeaponType.gundouble_fire)
			r= Quaternion.AngleAxis (40/2f, Vector3.up) * transform.rotation;
		Vector3 rPoint = r * Vector3.forward * currentWeapon.range + transform.position;
		
		Debug.DrawLine(transform.position, rPoint, Color.red, 10f);
		
		Quaternion l = Quaternion.AngleAxis(90 / 2f, Vector3.down) * transform.rotation;
		if (currentWeaponId == WeaponType.gun_fire || currentWeaponId == WeaponType.gundouble_fire)
			l= Quaternion.AngleAxis (40/2f, Vector3.down) * transform.rotation;
		Vector3 lPoint = l * Vector3.forward * currentWeapon.range + transform.position;
		Debug.DrawLine(transform.position, lPoint, Color.green, 10f);
		
		//		if (currentWeapon.ID == WeaponType.chuizi)
		//		{
		//			GameObject effect = (GameObject)Instantiate(Resources.Load("prefabs/Effects/chuizi"));
		//			effect.transform.position = transform.position + transform.rotation * new Vector3(0, 1, 2);
		//			Ultilities.gm.audioScript.chuiziFX.play();
		//		}
		//		else 
		if (currentWeaponId == WeaponType.dao)
		{
			Ultilities.gm.audioScript.daoFX.play();
		}
		else if (currentWeaponId == WeaponType.bangqiugun)
		{
			Ultilities.gm.audioScript.bangqiugunFX.play();
		}
		else if (currentWeaponId == WeaponType.gun_fire || currentWeaponId == WeaponType.gundouble_fire)
		{
			GameObject bulletObj = null;
			if (!automatic)
				bulletObj = (GameObject)Instantiate(WeaponController.Instance.GetWeaponByID(currentWeaponId).BulletAvatar);
			else
				bulletObj = (GameObject)Instantiate(GetComponent<FakeWeaponController>().currentWeapon.BulletAvatar);
			
			//			bulletObj.transform.parent = bulletsArea;
			bulletObj.transform.position = spawnPointR.position - spawnPointR.transform.forward * 1.38f;
			bulletObj.transform.rotation = spawnPointR.rotation;
			
			bulletObj.transform.parent = bulletsArea;
			//			FireBullet fireBullet = bulletObj.GetComponent<FireBullet>();
			//			fireBullet.distance = currentWeapon.range;
			//			fireBullet.damage = currentWeapon.atk;
			//			fireBullet.InitWithSpawnPoint(spawnPointR.gameObject);
			if (!automatic)
				WeaponController.Instance.DecreaseWeaponCapacity();
			
			if (currentWeaponId == WeaponType.gundouble_fire)
			{
				GameObject bulletObjLeft = null;
				if (!automatic)
					bulletObjLeft = (GameObject)Instantiate(WeaponController.Instance.GetWeaponByID(currentWeaponId).BulletAvatar2);
				else
					bulletObjLeft = (GameObject)Instantiate(GetComponent<FakeWeaponController>().currentWeapon.BulletAvatar2);
				bulletObjLeft.transform.parent = bulletsArea;
				bulletObjLeft.transform.position = spawnPointL.position - spawnPointL.transform.forward * 1.88f;
				bulletObjLeft.transform.rotation = spawnPointL.rotation;
				//				FireBullet fireBullet2 = bulletObjLeft.GetComponent<FireBullet>();
				//				fireBullet2.distance = currentWeapon.range;
				//				fireBullet2.damage = currentWeapon.atk;
				//				fireBullet2.InitWithSpawnPoint(spawnPointL.gameObject);
				if (!automatic)
					WeaponController.Instance.DecreaseWeaponCapacity();
			}
		}
		
		for (int i = EnemyManager.Instance.transform.childCount - 1; i >= 0; i--)
		{
			if (EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>() != null && Helper.isINTriangle(EnemyManager.Instance.transform.GetChild(i).position, transform.position, lPoint, rPoint))
			{
				if (!automatic && currentWeaponId != WeaponType.gun_fire && currentWeaponId != WeaponType.gundouble_fire)
					WeaponController.Instance.DecreaseWeaponCapacity();
				
				StartCoroutine(hitBack(EnemyManager.Instance.transform.GetChild(i), EnemyManager.Instance.transform.GetChild(i).position, EnemyManager.Instance.transform.GetChild(i).position + Vector3.back * (hitDistance + currentWeapon.hitDistance), 0.2f));
				//							EventService.Instance.GetEvent<EnemyStateChangeEvent>().Publish(AnimState.take, enemy.EnemyId);
				EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().onDamage(currentWeapon.atk, EnemyManager.Instance.transform.GetChild(i).position);
				
				if (currentWeaponId == WeaponType.bangqiugun)
					Ultilities.gm.audioScript.bangqiugunHitFX.play();
				//				else if (currentWeaponId == WeaponType.chuizi)
				//					Ultilities.gm.audioScript.chuiziHitFx.play();
				else if (currentWeaponId == WeaponType.dao)
					Ultilities.gm.audioScript.daoHitFX.play();
				
			}
		}
	}
	
	void GenerateZidanke()
	{
		GameObject zidankeEffect = (GameObject)Instantiate(zidanke);
		zidankeEffect.transform.parent = transform;
		zidankeEffect.transform.localPosition = new Vector3(0, 1f, 1.24f);
	}
	
	IEnumerator ShootBullet()
	{
		Weapon currentWeapon = WeaponController.Instance.GetWeaponByID(currentWeaponId);
		
		//		if (automatic)
		//			currentWeapon = GetComponent<FakeWeaponController>().currentWeapon;
		
		if (currentWeapon == null)
		{
			Debug.LogError("can't find the weapon by id " + currentWeaponId);
			yield break;
		}
		
		if (currentWeaponId == WeaponType.dianju)
		{
			yield break;
		}
		
		//		if (Time.time > lastFireTime + 1f / currentWeapon.Frequency)
		{
			lastFireTime = Time.time;
			GameObject bulletObj = null;
			if (!automatic)
				bulletObj = (GameObject)Instantiate(WeaponController.Instance.GetWeaponByID(currentWeaponId).BulletAvatar);
			else
				bulletObj = (GameObject)Instantiate(GetComponent<FakeWeaponController>().currentWeapon.BulletAvatar);
			
			//			bulletObj.transform.parent = bulletsArea;
			bulletObj.transform.position = spawnPointR.position - spawnPointR.transform.forward * 4.38f;
			bulletObj.transform.rotation = spawnPointR.rotation;
			
			GameObject qiangkou = null;
			GameObject qiangkou2 = null;
			if (currentWeapon.ID == WeaponType.gun_sandan || currentWeapon.ID == WeaponType.gundouble_sandan)
			{
				qiangkou = (GameObject)Instantiate(Resources.Load("prefabs/Effects/gun_sandan"));
				if (currentWeapon.ID == WeaponType.gundouble_sandan)
					qiangkou2 = (GameObject)Instantiate(Resources.Load("prefabs/Effects/gun_sandan"));
			}
			else
			{
				qiangkou = (GameObject)Instantiate(Resources.Load("prefabs/bullets/qiangkou"));
				if (currentWeapon.ID >  WeaponType.gun_single && currentWeapon.ID < WeaponType.gun_double)
				{
					qiangkou2 = (GameObject)Instantiate(Resources.Load("prefabs/bullets/qiangkou2"));
				}
			}
			
			qiangkou.transform.parent = bulletsArea;
			qiangkou.transform.position = spawnPointR.transform.position + new Vector3(0.5f, 0, 0);
			if (qiangkou2 != null)
			{
				qiangkou2.transform.parent = bulletsArea;
				qiangkou2.transform.position = spawnPointL.transform.position + new Vector3(0.5f, 0, 0);
			}
			
			if (currentWeaponId == WeaponType.gun_liudan || currentWeaponId == WeaponType.gundouble_liudan)
			{
				Ultilities.gm.audioScript.liudanFX.play();
				LiudanBullet liudanBullet = bulletObj.GetComponent<LiudanBullet>();
				liudanBullet.distance = currentWeapon.range;
				liudanBullet.damage = currentWeapon.atk;
				//						liudanBullet.distance = currentWeapon.Distance;
				liudanBullet.InitWithSpawnPoint(spawnPointR.gameObject);
				if (!automatic)
					WeaponController.Instance.DecreaseWeaponCapacity();
				
				if (currentWeaponId == WeaponType.gundouble_liudan)
				{
					yield return new WaitForSeconds(0.5f);
					Ultilities.gm.audioScript.liudanFX.play();
					GameObject bulletObj2 = null;
					if (!automatic)
						bulletObj2 = (GameObject)Instantiate(WeaponController.Instance.GetWeaponByID(currentWeaponId).BulletAvatar2);
					else
						bulletObj2 = (GameObject)Instantiate(GetComponent<FakeWeaponController>().currentWeapon.BulletAvatar2);
					
					//					bulletObj2.transform.parent = bulletsArea;
					bulletObj2.transform.position = spawnPointL.position - spawnPointL.transform.forward * 2.88f;
					bulletObj2.transform.rotation = spawnPointL.rotation;
					LiudanBullet liudanBullet2 = bulletObj2.GetComponent<LiudanBullet>();
					liudanBullet2.distance = currentWeapon.range;
					liudanBullet2.damage = currentWeapon.atk;
					//						liudanBullet.distance = currentWeapon.Distance;
					liudanBullet2.InitWithSpawnPoint(spawnPointL.gameObject);
					
					if (!automatic)
						WeaponController.Instance.DecreaseWeaponCapacity();
				}
			}
			else if (currentWeaponId == WeaponType.gun_sandan || currentWeaponId == WeaponType.gundouble_sandan)
			{
				GenerateZidanke();
				Ultilities.gm.audioScript.sandanFX.play();
				GameObject zidanRightEffect = (GameObject)Instantiate(Resources.Load("prefabs/Bullets/zidan_sandan"));
				zidanRightEffect.transform.position = spawnPointR.position;
				zidanRightEffect.transform.rotation = transform.rotation;
				for (int i = 0; i < 4; i++)
				{
					float xOffset = Random.Range(-10f, 10f);
					float yOffset = Random.Range(-10f, 10f);
					
					//							float duration = Random.Range(0.01f, 0.05f);
					GameObject bulletObjRight = null;
					if (!automatic)
						bulletObjRight = (GameObject)Instantiate(WeaponController.Instance.GetWeaponByID(currentWeaponId).BulletAvatar);
					else
						bulletObjRight = (GameObject)Instantiate(GetComponent<FakeWeaponController>().currentWeapon.BulletAvatar);
					
					
					//					bulletObjRight.transform.parent = bulletsArea;
					bulletObjRight.transform.position = spawnPointR.position;
					bulletObjRight.transform.rotation = Quaternion.Euler(new Vector3(xOffset, yOffset, 0)) * spawnPointR.rotation;
					SimpleBullet rightbullet = bulletObjRight.GetComponent<SimpleBullet>();
					rightbullet.distance = currentWeapon.range;
					rightbullet.damage = currentWeapon.atk;
					rightbullet.InitWithSpawnPoint(spawnPointR.gameObject, Quaternion.Euler(new Vector3(xOffset, yOffset, 0)));
				}
				
				
				//				SimpleBullet midbullet = bulletObj.GetComponent<SimpleBullet>();
				//				midbullet.distance = currentWeapon.range;
				//				midbullet.damage = currentWeapon.atk;
				//				midbullet.InitWithSpawnPoint(spawnPointR.gameObject, Quaternion.Euler(Vector3.zero));
				//						yield return new WaitForSeconds(0.01f);
				if (!automatic)
					WeaponController.Instance.DecreaseWeaponCapacity();
				
				if (currentWeaponId == WeaponType.gundouble_sandan)
				{
					yield return new WaitForSeconds(0.3f);
					Ultilities.gm.audioScript.sandanFX.play();
					GameObject zidanLeftEffect = (GameObject)Instantiate(Resources.Load("prefabs/Bullets/zidan_sandan"));
					zidanLeftEffect.transform.position = spawnPointL.position;
					zidanLeftEffect.transform.rotation = transform.rotation;
					for (int i = 0; i < 4; i++)
					{
						float xOffset = Random.Range(-10f, 10f);
						float yOffset = Random.Range(-10f, 10f);
						//							float duration = Random.Range(0.01f, 0.05f);
						GameObject bulletObjLeft = null;
						if (!automatic)
							bulletObjLeft = (GameObject)Instantiate(WeaponController.Instance.GetWeaponByID(currentWeaponId).BulletAvatar2);
						else
							bulletObjLeft = (GameObject)Instantiate(GetComponent<FakeWeaponController>().currentWeapon.BulletAvatar2);
						//						bulletObjLeft.transform.parent = bulletsArea;
						bulletObjLeft.transform.position = spawnPointL.position;
						bulletObjLeft.transform.rotation = Quaternion.Euler(new Vector3(xOffset, yOffset, 0)) * spawnPointL.rotation;
						SimpleBullet rightbullet = bulletObjLeft.GetComponent<SimpleBullet>();
						rightbullet.distance = currentWeapon.range;
						rightbullet.damage = currentWeapon.atk;
						rightbullet.InitWithSpawnPoint(spawnPointL.gameObject, Quaternion.Euler(new Vector3(xOffset, yOffset, 0)));
					}
					
					
					//					SimpleBullet midbullet2 = bulletObj.GetComponent<SimpleBullet>();
					//					midbullet2.distance = currentWeapon.range;
					//					midbullet2.damage = currentWeapon.atk;
					//					midbullet2.InitWithSpawnPoint(spawnPointL.gameObject, Quaternion.Euler(Vector3.zero));
					//						yield return new WaitForSeconds(0.01f);
					if (!automatic)
						WeaponController.Instance.DecreaseWeaponCapacity();
				}
				
			}
			else if (currentWeaponId == WeaponType.gun_fire || currentWeaponId == WeaponType.gundouble_fire)
			{
				
			}
			else
			{
				if (currentWeaponId == WeaponType.gun_shouqiang)
				{
					Ultilities.gm.audioScript.shouqiangSoundFX.play();
				}
				else if (currentWeaponId == WeaponType.gundouble_shouqiang)
				{
					Ultilities.gm.audioScript.shouqiangSoundFX.play();
					Ultilities.gm.audioScript.shouqiangSoundFX.play();
				}
				else if (currentWeaponId == WeaponType.gun_m4 || currentWeaponId == WeaponType.gun_jiatelin)
				{
					Ultilities.gm.audioScript.jiqiangSoundFX.play();
				}
				else if (currentWeaponId == WeaponType.gundouble_m4)
				{
					Ultilities.gm.audioScript.jiqiangSoundFX.play();
					Ultilities.gm.audioScript.jiqiangSoundFX.play();
				}
				
				GenerateZidanke();
				SimpleBullet bullet = bulletObj.GetComponent<SimpleBullet>();
				bullet.distance = currentWeapon.range;
				bullet.damage = currentWeapon.atk;
				bullet.InitWithSpawnPoint(spawnPointR.gameObject,Quaternion.Euler(Vector3.zero));
				if (!automatic)
					WeaponController.Instance.DecreaseWeaponCapacity();
				
				if (currentWeaponId > WeaponType.gun_single && currentWeaponId < WeaponType.gun_double)
				{
					yield return new WaitForSeconds(0.05f);
					GameObject bulletObj2 = null;
					if (!automatic)
						bulletObj2 = (GameObject)Instantiate(WeaponController.Instance.GetWeaponByID(currentWeaponId).BulletAvatar2);
					else
						bulletObj2 = (GameObject)Instantiate(GetComponent<FakeWeaponController>().currentWeapon.BulletAvatar2);
					//					bulletObj2.transform.parent = bulletsArea;
					bulletObj2.transform.position = spawnPointL.position - spawnPointL.transform.forward * 4.88f;
					bulletObj2.transform.rotation = spawnPointL.rotation;
					SimpleBullet bullet2 = bulletObj2.GetComponent<SimpleBullet>();
					bullet2.distance = currentWeapon.range;
					bullet2.damage = currentWeapon.atk;
					bullet2.InitWithSpawnPoint(spawnPointL.gameObject,Quaternion.Euler(Vector3.zero));
					if (!automatic)
						WeaponController.Instance.DecreaseWeaponCapacity();
				}
				
			}
		}
		
		yield return null;
	}
	
	void GenerateBullet()
	{
		StartCoroutine(ShootBullet());
	}
	
	IEnumerator GenerateSandanBullets(Weapon currentWeapon, GameObject bulletObj, Transform spawnPoint)
	{
		for (int i = 0; i < 4; i++)
		{
			float xOffset = Random.Range(-15f, 15f);
			float yOffset = Random.Range(-15f, 15f);
			float duration = Random.Range(0.01f, 0.05f);
			GameObject bulletObjRight = null;
			if (!automatic)
				bulletObjRight = (GameObject)Instantiate(WeaponController.Instance.GetWeaponByID(currentWeaponId).BulletAvatar);
			else
				bulletObjRight = (GameObject)Instantiate(GetComponent<FakeWeaponController>().currentWeapon.BulletAvatar);
			bulletObjRight.transform.parent = bulletsArea;
			bulletObjRight.transform.position = spawnPoint.position;
			bulletObjRight.transform.rotation = Quaternion.Euler(new Vector3(xOffset, yOffset, 0)) * spawnPoint.rotation;
			SimpleBullet rightbullet = bulletObjRight.GetComponent<SimpleBullet>();
			rightbullet.distance = currentWeapon.range;
			rightbullet.damage = currentWeapon.atk;
			rightbullet.InitWithSpawnPoint(spawnPoint.gameObject, Quaternion.Euler(new Vector3(xOffset, yOffset, 0)));
			yield return new WaitForSeconds(duration);
		}
		
		
		SimpleBullet midbullet = bulletObj.GetComponent<SimpleBullet>();
		midbullet.distance = currentWeapon.range;
		midbullet.damage = currentWeapon.atk;
		midbullet.InitWithSpawnPoint(spawnPoint.gameObject, Quaternion.Euler(Vector3.zero));
		yield return new WaitForSeconds(0.01f);
	}
	
	private GameObject dianjuAttackEffect;
	void OnWeaponChange(WeaponType weaponId, int capacity)
	{
		int childCount = bulletsArea.childCount -1;
		Ultilities.gm.audioScript.weaponChangeFX.play();
		while(childCount >= 0)
		{
			Transform child = bulletsArea.GetChild(childCount);
			if (child != null)
				child.gameObject.SetActive(false);
			childCount--;
		}
		
		if (currentWeaponId != weaponId)
		{
			currentWeaponId = weaponId;
			//			lastFireTime -= 1f / WeaponController.Instance.GetWeaponByID(currentWeaponId).Frequency;
		}
		
		if (currentWeaponId != WeaponType.bangqiugun && currentWeaponId != WeaponType.dao)
		{
			daoguangObj.SetActive(false);
		}
		else
		{
			if (isFired)
				daoguangObj.SetActive(true);
		}
		
		if (currentWeaponId == WeaponType.bangqiugun && SettingManager.Instance.TutorialSeq <= 1)
		{
//			Time.timeScale = 0;
			TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
		}
		
		ChangeState(AnimState.thinkAttack);
	}
	
	void ChangeMovementState(AnimState state)
	{
		if (currentState != state) {
			EventService.Instance.GetEvent<HeroStateChangeEvent>().Publish(state, heroId);
		}
	}
	
	void onStateChange(AnimState state, int id)
	{
		if (heroId == id) {
			currentState = state;
			GetComponent<Animation>().CrossFade(state.ToString());
		}
	}
	
	void onEnemyDead(int deadId)
	{
		//		if (aimedController != null && aimedController.EnemyId == deadId) {
		//			aimedController = null;		
		//		}
	}
	
	void onFireChanged(bool fire)
	{
		isFired = fire;
		
		if (isFired)
		{
			if (currentWeaponId == WeaponType.bangqiugun
			    ||currentWeaponId == WeaponType.dao) 
			{
				daoguangObj.SetActive(true);
			}
			else
			{
				daoguangObj.SetActive(false);
			}
		}
		else
		{
			daoguangObj.SetActive(false);
		}
		//		if (!fire)
		//			lastFireTime -= 1f / WeaponController.Instance.GetWeaponByID(currentWeaponId).Frequency;
	}
	
	void OnDestroy()
	{
		EventService.Instance.GetEvent<HeroStateChangeEvent> ().Unsubscribe (onStateChange);
		EventService.Instance.GetEvent<EnemyDeadEvent> ().Unsubscribe (onEnemyDead);
		EventService.Instance.GetEvent<FireChangeEvent> ().Unsubscribe (onFireChanged);
		EventService.Instance.GetEvent<WeaponChangeEvent> ().Unsubscribe (OnWeaponChange);
	}
}
