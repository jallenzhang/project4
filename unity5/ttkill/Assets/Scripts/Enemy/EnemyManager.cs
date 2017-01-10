//#define US_VERSION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathologicalGames;

public enum EnemyType
{
	hua=1,
	ma,
	mogu,
	ji,
	pangxie,
	dog,
	bee,
	miniJi,
	hua_lan,
	ma_zong,
	mogu_lan,
	mogu_zi,
	ji_white,
	pangxie_lan,
	dog_lan,
	bee_lv,
	empty,


	bigJi = 101,
	bigMa,
	bigGou,
	bigBee,

	boss_niu = 1001,
	boss_pangzi,
	boss_wa,
	boss_wuyao,
}

public enum PlayModes
{
	Normal = 1,
	OnlyBoss,
	Bosses,
	TimeLimited,
}

public class EnemyManager : MonoBehaviour {

	public List<GameObject> forbiddenAreas;

	public GameObject BurnMonsterEffect0;
	public GameObject BurnMonsterEffect1;
	public GameObject BurnMonsterEffect2;

	public GameObject nan;
	public GameObject nv;

	public Material[]       IceMaterial;
	private int BossHPRate = 3;

	private float lastStageTime = 0f;
	private List<GameObject> deadAreas;

	private static EnemyManager instance = null;
	private bool hasBorned = false;

	public static EnemyManager Instance
	{
		get
		{
			return instance;
		}
	}

	[HideInInspector]
	public List<EnemyController> Enemies = new List<EnemyController>();

	private int currentEnemyId = 0;
	private string enmeyPath = "prefabs/Enemy/";
	private int totalEnemies = 0;
	private float wildEnemyDuration = 0f;
	[HideInInspector]
	public PlayModes mode = PlayModes.Normal;
	private bool isStop = true;

#if US_VERSION
	private string strNormalPlaymode = "Kill all of the monsters";
	private string strOnlyBossPlaymode = "Kill the BOSS";
	private string strBossesPlayMode = "Kill all of the BOSSES";
	private string strTimeLimitedPlayMode = "KILL all of the monster in 5 mins";
#else
	private string strNormalPlaymode = "杀死所有的怪";
	private string strOnlyBossPlaymode = "杀死BOSS";
	private string strBossesPlayMode = "杀死所有的BOSS";
	private string strTimeLimitedPlayMode = "300秒内杀死所有的怪";
#endif

	void Start()
	{
		instance = this;
		forbiddenAreas = GameObject.FindGameObjectsWithTag("forbiddenArea").ToList();
		deadAreas = GameObject.FindGameObjectsWithTag("deadArea").ToList();

		totalEnemies = Ultilities.TotalEnemiesInLevel();
		wildEnemyDuration = ConstData.EnemyWaveTimes[ConstData.EnemyWaveTimes.Count - 1] / (totalEnemies * 0.1f);
//		Debug.Log("totalEnemies " + totalEnemies);
		SubscribeEvents();

		if (GameData.Instance.LevelType == LevelType.RushLevel)
		{
			mode =(PlayModes)(IOHelper.GetStageInfoByStageId(GameData.Instance.CurrentLevel).type);
			switch(mode)
			{
			case PlayModes.Normal:
				UIBattleSceneLogic.Instance.WaveChange();
				StartCoroutine(WaveGenerator());
				StartCoroutine(GenerateWildEnemy());
				StartCoroutine(GenerateWeaponEnemy());
				StartCoroutine(GenerateGoldEnemy());
				StartCoroutine(RemoveEnemyInDeadArea());
				StartCoroutine(PopupTargetDescription(strNormalPlaymode));
				break;
			case PlayModes.OnlyBoss:

				string bossids = IOHelper.GetStageInfoByStageId(GameData.Instance.CurrentLevel).boss_id;
				string[] ids = bossids.Split(';');
				int id = int.Parse(ids[0]);
				GameObject player = (GameObject)GameObject.FindGameObjectWithTag ("Player");
				Vector3 playerPos = player.transform.position;
				GenerateEnemy(playerPos, (EnemyType)id);
				GameData.Instance.CurrentWave = ConstData.MaxWaves;
				UIBattleSceneLogic.Instance.WaveChange();
//				StartCoroutine(GenerateWildEnemy());		
				StartCoroutine(GenerateWeaponEnemy());
				StartCoroutine(GenerateGoldEnemy());
				StartCoroutine(RemoveEnemyInDeadArea());
				StartCoroutine(PopupTargetDescription(strOnlyBossPlaymode));
				break;
			case PlayModes.Bosses:
				StartCoroutine(WaveGenerator());
				StartCoroutine(GenerateWildEnemy());
				StartCoroutine(GenerateWeaponEnemy());
				StartCoroutine(GenerateGoldEnemy());
				StartCoroutine(RemoveEnemyInDeadArea());
				StartCoroutine(PopupTargetDescription(strBossesPlayMode));
				break;
			case PlayModes.TimeLimited:
				StartCoroutine(WaveGenerator());
				StartCoroutine(GenerateWildEnemy());
				StartCoroutine(GenerateWeaponEnemy());
				StartCoroutine(GenerateGoldEnemy());
				StartCoroutine(RemoveEnemyInDeadArea());
				StartCoroutine(DoCountDown(300));
				StartCoroutine(PopupTargetDescription(strTimeLimitedPlayMode));
				break;
			}
		}
		else
		{
			UIBattleSceneLogic.Instance.WaveChange();
			StartCoroutine(WaveGenerator());
			StartCoroutine(GenerateWildEnemy());
			StartCoroutine(GenerateWeaponEnemy());
			StartCoroutine(GenerateGoldEnemy());
			StartCoroutine(RemoveEnemyInDeadArea());
		}

		GameObject target;
		if (SettingManager.Instance.CurrentAvatarId == 1)
		{
			target = nan;
		}
		else
		{
			target = nv;
		}

		Physics.IgnoreLayerCollision(gameObject.layer, target.layer, true);
	}

	IEnumerator PopupTargetDescription(string description)
	{
		yield return new WaitForSeconds(1f);
		DialogManager.Instance.PopupFadeOutMessage(description, 4f);
	}

	/// <summary>
	///  倒计时
	/// </summary>
	/// <param name="seconds">总时间</param>
	/// <returns></returns>
	public IEnumerator DoCountDown(int seconds)
	{
		while (isStop)
		{
			if (seconds - Time.timeSinceLevelLoad < 0)
			{
				StopCoroutine("DoCountDown");
				ResultDialog.Popup(false);
				yield return isStop = false;
				break;
			}

			UIBattleSceneLogic.Instance.TimeChange(Ultilities.ConvertSecondToTime((int)(seconds - Time.timeSinceLevelLoad)));
			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator GenerateWeaponEnemy()
	{
		float randomTime = Random.Range(32f, 35f);
		yield return new WaitForSeconds(randomTime);
		GameObject player = (GameObject)GameObject.FindGameObjectWithTag ("Player");
		Vector3 playerPos = player.transform.position;

		int r = Random.Range(1, (int)EnemyType.empty * 2);
		
		if (r >= (int)EnemyType.empty)
			r = r % 2 == 0 ? (int)EnemyType.bee : (int)EnemyType.bee_lv;

		GenerateEnemy(playerPos, (EnemyType)r, 2);
//		if (GameData.Instance.CurrentWave < ConstData.MaxWaves && GameData.Instance.LevelType == LevelType.RushLevel)
			StartCoroutine(GenerateWeaponEnemy());
//		else if (IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).wave > GameData.Instance.CurrentWave && GameData.Instance.LevelType == LevelType.InfiniteLevel)
//			StartCoroutine(GenerateWeaponEnemy());

	}

	IEnumerator WaveGenerator()
	{
		if ((GameData.Instance.CurrentWave < ConstData.MaxWaves && GameData.Instance.LevelType == LevelType.RushLevel))
//		    || (GameData.Instance.CurrentWave 
		{
			float nextTime = ConstData.EnemyWaveTimes[GameData.Instance.CurrentWave];
			if (transform.childCount < 10)
			{
//				if (SettingManager.Instance.TutorialSeq == 2 && transform.childCount > 7)
//					TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
				
				if ((Time.timeSinceLevelLoad - lastStageTime) > nextTime)
				{
					GenerateWave(GameData.Instance.CurrentWave + 1);
					//here wait the enemies down, then change the wave count; because of the game over condition
					hasBorned = false;
//					Ultilities.CleanMemory();
					GameData.Instance.ChangeCurrentWave(true);
					UIBattleSceneLogic.Instance.WaveChange();
				}
			}
			else
			{
//				if (SettingManager.Instance.TutorialSeq == 3)
//					TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
				if ((Time.timeSinceLevelLoad - lastStageTime) > nextTime)
				{
					lastStageTime += 5f;
				}
			}

			yield return new WaitForSeconds(1);
			StartCoroutine(WaveGenerator());
		}
		else if (GameData.Instance.LevelType == LevelType.InfiniteLevel && GameData.Instance.CurrentWave < IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).wave)
		{
			float nextTime = ConstData.EnemyWaveTimes[GameData.Instance.CurrentWave];
			if (transform.childCount < 10)
			{
				if (Time.timeSinceLevelLoad - lastStageTime > nextTime)
				{
					GenerateWave(GameData.Instance.CurrentWave + 1);
					hasBorned = false;
//					Ultilities.CleanMemory();
					GameData.Instance.ChangeCurrentWave(true);
					UIBattleSceneLogic.Instance.WaveChange();
				}
			}
			else
			{
				if ((Time.timeSinceLevelLoad - lastStageTime) > nextTime)
				{
					lastStageTime += 5f;
				}
			}

			yield return new WaitForSeconds(1);
			StartCoroutine(WaveGenerator());
		}

		if ((GameData.Instance.CurrentWave == ConstData.MaxWaves && GameData.Instance.LevelType == LevelType.RushLevel)
		    || (GameData.Instance.LevelType == LevelType.InfiniteLevel && GameData.Instance.CurrentWave >= IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).wave))
		{
			StopCoroutine("GenerateWildEnemy");
//			StopCoroutine("GenerateWeaponEnemy");
//			StopCoroutine("GenerateGoldEnemy");
		}

	}

	void SubscribeEvents()
	{
		EventService.Instance.GetEvent<EnemyDeadEvent> ().Subscribe (RemoveEnemyById);
	}

	void UnSubscribeEvents()
	{
		EventService.Instance.GetEvent<EnemyDeadEvent> ().Unsubscribe (RemoveEnemyById);
	}

	public EnemyController GetNearestEnemy(Vector3 senderPos)
	{
		EnemyController ret = null;

		if (transform.childCount == 0)
			return ret;
		float distance = int.MaxValue;
		if (transform.GetChild(0).GetComponent<EnemyController>() != null && transform.GetChild(0).GetComponent<EnemyController>().HP > 0) {
			ret = transform.GetChild(0).GetComponent<EnemyController>();
			distance = Vector3.Distance (senderPos, ret.gameObject.transform.position);		
		}

		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i) != null 
			    && transform.GetChild(i).GetComponent<EnemyController>() != null && 
			    transform.GetChild(i).GetComponent<EnemyController>().HP > 0 &&
			    !transform.GetChild(i).GetComponent<EnemyController>().GetState().Equals(AnimState.dead))
			{
				float currentDis = Vector3.Distance (senderPos, transform.GetChild(i).position);
				if (distance > currentDis)
				{
					ret = transform.GetChild(i).GetComponent<EnemyController>();
					distance = currentDis;
				}
			}
		}

		return ret;
	}

	private void CheckWin()
	{
		if (GameData.Instance.CurrentWave == ConstData.MaxWaves && GameData.Instance.LevelType == LevelType.RushLevel)
		{
			Debug.Log("Enemies.Count " + transform.childCount );
			if (transform.childCount == 0 && hasBorned)//|| transform.childCount == 0)
			{
				Debug.Log("you win!!!!!");
				isStop = false;
				StopCoroutine("GenerateWeaponEnemy");
				StopCoroutine("WaveGenerator");
				//				GameData.Instance.CurrentWave = 0;
				StartCoroutine(PopUpResultDialog(true));
			}
			//			Application.LoadLevel("ui");
		}
		else if (GameData.Instance.LevelType == LevelType.InfiniteLevel)
		{
			if (GameData.Instance.CurrentWave == IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).wave)
			{
				Debug.Log("Infinite Enemies.Count " + transform.childCount);
				if (transform.childCount < 5 && hasBorned)//|| transform.childCount == 0)
				{
					//					GameData.Instance.CurrentWave = 0;
					GameData.Instance.ChangeCurrentStage(true);
					StartCoroutine(GotoNextInfiniteStage());
				}
			}
		}
	}

	public void  RemoveEnemyById(int id)
	{
		EnemyController controller = Enemies.Where (s => s.EnemyId == id).FirstOrDefault ();
		if (controller != null)
		{
			Enemies.Remove (controller);
			controller = null;
		}

		CheckWin();
	}

	IEnumerator GotoNextInfiniteStage()
	{
		yield return new WaitForSeconds(6f);
		UIBattleSceneLogic.Instance.showNextWaveTip();
		yield return new WaitForSeconds(2f);
		UIBattleSceneLogic.Instance.WaveChange();
		lastStageTime = Time.timeSinceLevelLoad;
		StartCoroutine(WaveGenerator());
		StartCoroutine(GenerateWildEnemy());
		StartCoroutine(GenerateWeaponEnemy());
		StartCoroutine(GenerateGoldEnemy());
	}

	IEnumerator PopUpResultDialog(bool success)
	{
		yield return new WaitForSeconds(4f);
		ResultDialog.Popup(success);
	}

	IEnumerator GenerateWildEnemy()
	{
		yield return new WaitForSeconds(wildEnemyDuration);
		GameObject player = (GameObject)GameObject.FindGameObjectWithTag ("Player");
		Vector3 playerPos = player.transform.position;
		
		int r = Random.Range(1, (int)EnemyType.empty * 2);

		if (r >= (int)EnemyType.empty)
			r = r % 2 == 0 ? (int)EnemyType.bee : (int)EnemyType.bee_lv;

		int bingo = Random.Range(0, 5);
		bool bDrop = false;
		if (bingo == 1)
			bDrop = true;

		GenerateEnemy(playerPos, (EnemyType)r, bDrop ? 1 : 0);

		if (GameData.Instance.CurrentWave < ConstData.MaxWaves && GameData.Instance.LevelType == LevelType.RushLevel)
			StartCoroutine(GenerateWildEnemy());
		else if (IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).wave > GameData.Instance.CurrentWave && GameData.Instance.LevelType == LevelType.InfiniteLevel)
			StartCoroutine(GenerateWildEnemy());
	}

	IEnumerator GenerateGoldEnemy()
	{
		yield return new WaitForSeconds(15);
		GameObject player = (GameObject)GameObject.FindGameObjectWithTag ("Player");
		Vector3 playerPos = player.transform.position;
		
		int r = Random.Range(1, (int)EnemyType.empty);
		
//		if (r >= (int)EnemyType.empty)
//			r = r % 2 == 0 ? (int)EnemyType.bee : (int)EnemyType.bee_lv;

		
		GenerateEnemy(playerPos, (EnemyType)r, 1);


//		if (GameData.Instance.CurrentWave < ConstData.MaxWaves && GameData.Instance.LevelType == LevelType.RushLevel)
			StartCoroutine(GenerateGoldEnemy());
//		else if (IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).wave > GameData.Instance.CurrentWave && GameData.Instance.LevelType == LevelType.InfiniteLevel)
//			StartCoroutine(GenerateGoldEnemy());

	}

	IEnumerator RemoveEnemyInDeadArea()
	{
		yield return new WaitForSeconds(2);
		int count = transform.childCount;

		if (count > 0)
		{
			count -= 1;
			while(count>=0)
			{
				Transform tr = transform.GetChild(count);
				if (!InSafeArea(tr.position))
				{
					if (tr.GetComponent<EnemyController>() != null)
						RemoveEnemyById(tr.GetComponent<EnemyController>().EnemyId);
				}
				count--;
			}
		}
		else
		{
			CheckWin();
		}
		StartCoroutine(RemoveEnemyInDeadArea());
	}

	public EnemyController AddEnemyAtPoint(Vector3 pos, EnemyType type, int bDrop = 0)
	{
		string path = enmeyPath + ((EnemyType)type).ToString();
//		Debug.Log("path " + path);
		GameObject enemy = (GameObject)Instantiate (Resources.Load(path));

		if (bDrop != 0)
		{
			GameObject effect = (GameObject)Instantiate(Resources.Load("prefabs/Effects/guai_diaoluo"));
			effect.transform.parent = enemy.transform;
			effect.transform.localPosition = new Vector3(0, 1.5f, 0);
		}

		enemy.GetComponent<EnemyController> ().EnemyId = currentEnemyId++;
		enemy.GetComponent<EnemyController> ().Drop = bDrop;
		enemy.GetComponent<EnemyController>().InitData((int)type);
		if (PlayModes.OnlyBoss == mode && enemy.GetComponent<EnemyController>().currentType >= EnemyType.boss_niu)
			enemy.GetComponent<EnemyController>().maxHP = enemy.GetComponent<EnemyController>().HP *= BossHPRate;
		enemy.name = currentEnemyId.ToString ();
		enemy.transform.parent = transform;
		if (enemy.GetComponent<Rigidbody>() == null)
			enemy.AddComponent<Rigidbody> ();
		enemy.transform.position = pos;
//		enemy.transform.LookAt (GameObject.FindGameObjectWithTag("Player").transform.position);

		Enemies.Add (enemy.GetComponent<EnemyController>());

		return enemy.GetComponent<EnemyController>();
	}

	private static int total = 0;
	public void GenerateWave(int wave)
	{
		GameObject player = (GameObject)GameObject.FindGameObjectWithTag ("Player");
		Vector3 playerPos = player.transform.position;

		int r = Random.Range(1, (int)EnemyType.empty);
		while(((EnemyType)r).ToString().Contains("empty")
		      || (EnemyType)r == EnemyType.bee
		      || (EnemyType)r == EnemyType.bee_lv
		      || (EnemyType)r == EnemyType.miniJi)
		{
			r = Random.Range(1, (int)EnemyType.empty);
		}

		if (mode == PlayModes.Bosses && GameData.Instance.LevelType == LevelType.RushLevel)
		{
			r = Random.Range((int)EnemyType.bigJi, (int)EnemyType.bigBee + 1);
		}

		int enemyCount = 0;
		if (GameData.Instance.LevelType == LevelType.RushLevel)
		{
			enemyCount = Mathf.FloorToInt(15 + wave + GameData.Instance.CurrentLevel / 12);
			if (GameData.Instance.CurrentLevel <= 20)
			{
				enemyCount = Mathf.FloorToInt( enemyCount / (2f - GameData.Instance.CurrentLevel * 0.05f));
			}
		}
		else
		{
			enemyCount = Mathf.FloorToInt(15 + wave + IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).level / 12f);
		}

		if (mode == PlayModes.Bosses && GameData.Instance.LevelType == LevelType.RushLevel)
		{
			GenerateEnemy(playerPos, (EnemyType)r);
			enemyCount = 1;
		}
		else
		{
			for (int i = 0; i < enemyCount; i++) { //enemyCount
				GenerateEnemy(playerPos, (EnemyType)r);
			}

			if (SettingManager.Instance.TutorialSeq < 4)
			{
				if (wave == 4)
					StartCoroutine(FireTrigger(2));
				else if (wave == 8)
				{
					StartCoroutine(FireTrigger(3));
				}
			}
		}

//		string msg = string.Format("第{0}波，增加{1}个怪", wave, enemyCount);
//
//		DialogManager.Instance.PopupFadeOutMessage(msg);
		total += enemyCount;

		string bossWave = string.Empty;
		if (GameData.Instance.LevelType == LevelType.RushLevel)
			bossWave = IOHelper.GetStageInfoByStageId(GameData.Instance.CurrentLevel).wave;
		else
		{
			bossWave = IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).appear_wave;
		}
		if ( bossWave != "0")
		{
			string[] waves = bossWave.Split(';'); 
			int i = 0;
			foreach(string w in waves)
			{
				int iw = int.Parse(w);
				if (iw == wave)
				{
					string bossids = string.Empty;
					if (GameData.Instance.LevelType == LevelType.RushLevel)
						bossids = IOHelper.GetStageInfoByStageId(GameData.Instance.CurrentLevel).boss_id;
					else
						bossids = IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).bossid;
					string[] ids = bossids.Split(';');
					int id = int.Parse(ids[i]);
					GenerateEnemy(playerPos, (EnemyType)id);
				}
				i++;
			}
		}

		Debug.Log("Total enemies are " + total);
	}

	IEnumerator FireTrigger(float index)
	{
		yield return new WaitForSeconds(3.5f);
		if (SettingManager.Instance.TutorialSeq == index)
		{
			Time.timeScale = 0;
			TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
		}
	}

//	public void GenerateBossWave()
//	{
//		StopCoroutine("GenerateWildEnemy");
////		StopCoroutine("GenerateWeaponEnemy");
//		StopCoroutine("GenerateGoldEnemy");
//	}

	void GenerateEnemy(Vector3 playerPos, EnemyType type, int bDrop = 0)
	{
		float xoffset = Random.Range(-15.0f, 15.0f);
		float zoffset = Random.Range(-15.0f, 15.0f);
		
		Vector3 generatePos = playerPos + new Vector3(xoffset, 0, zoffset);
		if (ValidatePoint(generatePos))
		{
//			int r = Random.Range(1, System.Enum.GetValues(typeof(EnemyType)).Length + 1);
//			while(((EnemyType)r).ToString().Contains("empty"))
//			{
//				r = Random.Range(1, System.Enum.GetValues(typeof(EnemyType)).Length + 1);
//			}
			StartCoroutine(GotoAddEnemyAtPoint(generatePos, type, bDrop));
		}
		else
		{
			GenerateEnemy(playerPos, type, bDrop);
		}
	}

	IEnumerator GotoAddEnemyAtPoint(Vector3 pos, EnemyType type, int bDrop = 0)
	{
//		type = EnemyType.pangxie;
		float r = Random.Range(0f, 2f);
		yield return new WaitForSeconds(r);

//		GameObject effect0 = (GameObject)Instantiate(BurnMonsterEffect0);
		SpawnPool spawnPool = PoolManager.Pools["Items"];
		Transform effect0 = spawnPool.Spawn(BurnMonsterEffect0.name);
		effect0.position = pos + new Vector3(0, 0.5f, 0);
		yield return new WaitForSeconds(2f);

//		GameObject effect1 = (GameObject)Instantiate(BurnMonsterEffect1);
//		effect1.transform.position = pos;
//		yield return new WaitForSeconds(0.5f);
//		GameObject effect2 = (GameObject)Instantiate(BurnMonsterEffect2);
		Transform effect2 = spawnPool.Spawn(BurnMonsterEffect2.name);
		effect2.transform.position = pos;
		yield return new WaitForSeconds(0.5f);
		AddEnemyAtPoint(pos, type, bDrop);
		hasBorned = true;
	}

	public bool InSafeArea(Vector3 point)
	{
		//make sure y axis is 0;
		Vector3 adjustPoint = point - new Vector3(0, point.y, 0);
		foreach(GameObject area in deadAreas)
		{
			MeshFilter mf = area.GetComponent<MeshFilter>();
			float width = mf.mesh.bounds.size.x * area.transform.localScale.x;
			float length = mf.mesh.bounds.size.z * area.transform.localScale.z;
			
			Vector3 point0 = area.transform.position + area.transform.localRotation * new Vector3(-width/2, 0, length/2);
			point0 -= new Vector3(0, point0.y, 0);
			Vector3 point1 = area.transform.position + area.transform.localRotation * new Vector3(width/2, 0, length/2);
			point1 -= new Vector3(0, point1.y, 0);
			Vector3 point2 = area.transform.position + area.transform.localRotation * new Vector3(width/2, 0, -length/2);
			point2 -= new Vector3(0, point2.y, 0);
			Vector3 point3 = area.transform.position + area.transform.localRotation * new Vector3(-width/2, 0, -length/2);
			point3 -= new Vector3(0, point3.y, 0);
			
			if (Helper.isINRect(adjustPoint, point0, point1, point2, point3))
			{
				return false;
			}
		}
		
		return true;
	}

	List<MeshFilter> mfs = new List<MeshFilter>();
	public bool ValidatePoint(Vector3 point)
	{
		//make sure y axis is 0;
		try
		{
			Vector3 adjustPoint = point - new Vector3(0, point.y, 0);

			if (mfs.Count == 0)
			{

				foreach(GameObject area in forbiddenAreas)
				{
					MeshFilter mf = area.GetComponent<MeshFilter>();
					mfs.Add(mf);
				}
			}
			int i = 0;
			foreach(MeshFilter mf in mfs)
			{
				float width = mf.mesh.bounds.size.x * forbiddenAreas[i].transform.localScale.x;
				float length = mf.mesh.bounds.size.z * forbiddenAreas[i].transform.localScale.z;

				Vector3 point0 = forbiddenAreas[i].transform.position + forbiddenAreas[i].transform.localRotation * new Vector3(-width/2, 0, length/2);
				point0 -= new Vector3(0, point0.y, 0);
				Vector3 point1 = forbiddenAreas[i].transform.position + forbiddenAreas[i].transform.localRotation * new Vector3(width/2, 0, length/2);
				point1 -= new Vector3(0, point1.y, 0);
				Vector3 point2 = forbiddenAreas[i].transform.position + forbiddenAreas[i].transform.localRotation * new Vector3(width/2, 0, -length/2);
				point2 -= new Vector3(0, point2.y, 0);
				Vector3 point3 = forbiddenAreas[i].transform.position + forbiddenAreas[i].transform.localRotation * new Vector3(-width/2, 0, -length/2);
				point3 -= new Vector3(0, point3.y, 0);
				i++;
				if (Helper.isINRect(adjustPoint, point0, point1, point2, point3))
				{
					return false;
				}
			}
		}
		catch (System.StackOverflowException ex)
		{
			return false;
		}

		return true;
	}

	void OnDestroy()
	{
		Debug.Log("what the hell !!!!!!!!!!!!!!!!!!!!!!");
		StopCoroutine("WaveGenerator");
		StopCoroutine("RemoveEnemyInDeadArea");
		mfs.Clear();
		UnSubscribeEvents();
		GameData.Instance.Reset();
	}

	void OnDrawGizmos ()
	{
		foreach(GameObject area in forbiddenAreas)
		{
			MeshFilter mf = area.GetComponent<MeshFilter>();
			float width = mf.mesh.bounds.size.x * area.transform.localScale.x;
			float length = mf.mesh.bounds.size.z * area.transform.localScale.z;

			Vector3 point0 = area.transform.position + area.transform.localRotation * new Vector3(-width/2, 0, length/2);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere (point0, 1);
//			point0 -= new Vector3(0, point0.y, 0);
			Vector3 point1 = area.transform.position + area.transform.localRotation * new Vector3(width/2, 0, length/2);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere (point1, 1);
//			point1 -= new Vector3(0, point1.y, 0);
			Vector3 point2 = area.transform.position + area.transform.localRotation * new Vector3(width/2, 0, -length/2);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere (point2, 1);
//			point2 -= new Vector3(0, point2.y, 0);
			Vector3 point3 = area.transform.position + area.transform.localRotation * new Vector3(-width/2, 0, -length/2);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere (point3, 1);
//			point3 -= new Vector3(0, point3.y, 0);

		}
	}
}
