using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBattleSceneLogic : MonoBehaviour {
	
	public static UIBattleSceneLogic Instance;
	
	public UIProgressBar hp;
	public UILabel coinValue;
	public float speed = 3;
	public Camera PlayerCamera;
	public Camera NGUICamera;
	public UILabel waveValue;
	public GameObject weaponTip;
	public GameObject nextWaveComming;
	public Transform rightJoyPos;
	
	public UISprite zhiliaoMaskObj;
	public UISprite bindongMaskObj;
	public UISprite gunMaskObj;
	public UISprite aoeMaskObj;
	public FreezeBehaviour freezeBehaviour;
	public UILabel timeRemained;
	
	public UILabel aoeLabel;
	public UILabel bindongLabel;
	public UILabel jiaxueLabel;
	public UILabel jiatelinLabel;
	
	public GameObject MiddleGraphicObject;
	
	int currentValue = 0;
	int currentDiamondValue = 0;
	int currentScoreValue = 0;
	
	//	int nextValue = 0;
	private GameObject m_player;
	private GameObject player
	{
		get
		{
			if (m_player == null)
				m_player = GameObject.FindGameObjectWithTag("Player");
			return m_player;
		}
		set
		{
			m_player = value;
		}
	}
	
	public int CurrentGoldValue
	{
		get
		{
			return currentValue;
		}
	}
	
	public int CurrentDiamondValue
	{
		get
		{
			return currentDiamondValue;
		}
	}
	
	public int CurrentScoreValue
	{
		get
		{
			return currentScoreValue;
		}
	}
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		
		aoeLabel.text = SettingManager.Instance.TotalAoe.ToString();
		bindongLabel.text = SettingManager.Instance.TotalBindong.ToString();
		jiaxueLabel.text = SettingManager.Instance.TotalJiaxue.ToString();
		jiatelinLabel.text = SettingManager.Instance.TotalJiatelin.ToString();
		
		if (MiddleGraphicObject != null)
		{
			if (Application.loadedLevelName.Contains("changjing01"))
			{
				if ( SettingManager.Instance.Graphic == 1)
					MiddleGraphicObject.SetActive(false);
				else
				{
					MiddleGraphicObject.SetActive(true);
				}
			}
		}
		
		//		GameObject joystick_right = GameObject.Find("joystick_right");
		//		Vector3 screenPos = NGUICamera.WorldToScreenPoint(rightJoyPos.position);
		//		joystick_right.transform.position = new Vector3((screenPos.x  + joystick_right.GetComponent<GUITexture>().pixelInset.size.x )/ Screen.width, (screenPos.y)/ Screen.height, 0);
		//		Debug.Log("joystick_right.transform.position " + joystick_right.transform.position);
	}
	
	
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnDestroy()
	{
		
		currentValue = 0;
		//		nextValue = 0;
	}
	
	public void showNextWaveTip()
	{
		GameObject tip = (GameObject)Instantiate(nextWaveComming);
		tip.transform.parent = NGUICamera.transform;
		tip.transform.localScale = Vector3.one;
		tip.transform.position = Vector3.zero;
	}
	
	public void TimeChange(string textTime)
	{
		timeRemained.text = textTime;
	}
	
	public void WaveChange()
	{
		if (GameData.Instance.LevelType == LevelType.RushLevel)
			waveValue.text = GameData.Instance.CurrentWave + "/" + ConstData.MaxWaves;
		else 
			waveValue.text = GameData.Instance.CurrentWave + "/" + IOHelper.GetStageInfiniteInfoByStageId(GameData.Instance.CurrentStage).wave;
	}
	
	public void SetHp(float percent)
	{
		this.hp.value = percent;
		if (percent > 0.3f)
			UIBloodArea.Instance.HideNoMoreBlood();
		else if (percent > 0)
		{
			if (SettingManager.Instance.TutorialAddBlood == 1)
				TuorialTriggerManager.Instance.OpenTutorialDialogByIndex(100); //100 means tutorial add blood
			
			UIBloodArea.Instance.ShowNoMoreBlood();
		}
		else
			UIBloodArea.Instance.HideNoMoreBlood();
		
	}
	
	void OnPause()
	{
		Time.timeScale = 0;
		GameData.Instance.Pause = true;
		//		DialogManager.Instance.PopupDialog(CommonAsset.Load("prefabs/Dialogs/PauseGameDialog"));
		PauseGameDialog.Popup();
	}
	
	public void Reset()
	{
		currentValue = 0;
		currentDiamondValue = 0;
		currentScoreValue = 0;
		GameData.Instance.DeadTime = 0;
		GameData.Instance.Wudi = false;
	}
	
	public void AddGold(int goldValue)
	{
		StartCoroutine(ScrollNumber(0.5f, (float)currentValue, (float)currentValue + goldValue));
		currentValue += goldValue;
		GameData.Instance.AddGold(goldValue);
	}
	
	public void AddScore(int scoreValue)
	{
		currentScoreValue += scoreValue;
	}
	
	public void AddDiamond(int diamondValue)
	{
		currentDiamondValue += diamondValue;
	}
	
	IEnumerator ScrollNumber(float duration, float from, float to)
	{
		float rate = 1f / duration;
		float t = 0;
		
		while(true)
		{
			t += Time.deltaTime * rate;
			coinValue.text = ((int)Mathf.Lerp(from, to, t)).ToString();
			
			if (t > 1)
				break;
			yield return null;
		}
	}
	
	IEnumerator zhiliaoCD(float duraion)
	{
		zhiliaoFreezed = true;
		float rate = 1f / duraion;
		float tmp = 0f;
		while(tmp < 1f)
		{
			tmp += Time.deltaTime * rate;
			zhiliaoMaskObj.fillAmount = Mathf.Lerp(1f, 0f, tmp);
			yield return null;
		}
		//		yield return new WaitForSeconds(duraion);
		zhiliaoFreezed = false;
	}
	
	private bool zhiliaoFreezed = false;
	public void OnZhiliaoProp()
	{
		if (zhiliaoFreezed)
			return;
		
		if (SettingManager.Instance.TotalJiaxue <= 0)
			return;
		
		SettingManager.Instance.UseDaojuTime += 1;
		SettingManager.Instance.DaojuCost_Shuaxin += 1;
		float duration = IOHelper.GetItemInfoById(4).cd_time;
		StartCoroutine(zhiliaoCD(duration));
		MTAManager.DoEvent(MTAPoint.MINI_USE_XUE);
		
		float tmp = player.GetComponent<HeroController>().hp;
		tmp += 100;
		
		GameObject addHP = (GameObject)Instantiate(Resources.Load("UI/labelAddHP"));
		addHP.GetComponent<UILabel>().text = "+100";
		addHP.transform.parent = UIBattleSceneLogic.Instance.NGUICamera.transform;
		addHP.transform.localScale = Vector3.one;
		addHP.transform.position = Helper.WorldToNGUIPos(Camera.main, UIBattleSceneLogic.Instance.NGUICamera, player.transform.position + player.transform.up*4);
		TweenScale.Begin(addHP, 0.6f, Vector3.one * 2);
		
		GameObject fu = (GameObject)Instantiate(player.GetComponent<AdditionalEffect>().huifuPrefab);
		fu.transform.parent = player.transform;
		fu.transform.position = player.transform.position + new Vector3(0, 0.2f, 0);
		Ultilities.gm.audioScript.recoverFX.play();
		player.GetComponent<HeroController>().hp = Mathf.Min(tmp, player.GetComponent<HeroController>().maxHp);
		SetHp(player.GetComponent<HeroController>().hp / player.GetComponent<HeroController>().maxHp);
		
		GameData.Instance.AddJiaxue(-1);
		jiaxueLabel.text = SettingManager.Instance.TotalJiaxue.ToString();
		SettingManager.Instance.TutorialAddBlood = 0;
	}
	
	IEnumerator bindongCD(float duraion)
	{
		bindongFreezed = true;
		float rate = 1f / duraion;
		float tmp = 0f;
		while(tmp < 1f)
		{
			tmp += Time.deltaTime * rate;
			bindongMaskObj.fillAmount = Mathf.Lerp(1f, 0f, tmp);
			yield return null;
		}
		//		yield return new WaitForSeconds(duraion);
		bindongFreezed = false;
	}
	
	private bool bindongFreezed = false;
	public void OnBindongProp()
	{
		if (bindongFreezed)
			return;
		
		if (SettingManager.Instance.TotalBindong <= 0)
			return;
		
		SettingManager.Instance.UseDaojuTime += 1;
		SettingManager.Instance.DaojuCost_Shuaxin += 1;
		float duration = IOHelper.GetItemInfoById(2).cd_time;
		StartCoroutine(bindongCD(duration));
		
		StartCoroutine(PlayBindongEffect());
		
		GameData.Instance.AddBindong(-1);
		MTAManager.DoEvent(MTAPoint.MINI_USE_BINDONG);
		bindongLabel.text = SettingManager.Instance.TotalBindong.ToString();
	}
	
	IEnumerator DropIceBall()
	{
		Transform spawns = GameObject.Find("BindongSpawns").transform;
		for(int i = 0;  i < 50; i++)
		{
			{
				GameObject ball = (GameObject)Instantiate(Resources.Load("prefabs/Effects/bindongObj"));
				ball.transform.parent = spawns;
				float offset = Random.Range(5f, 40f);
				float offset2 = Random.Range(0f, 40);
				ball.transform.position = new Vector3(player.transform.position.x - offset, spawns.position.y, player.transform.position.z + offset2);
				ball.transform.localScale = Vector3.one;
				yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
			}
		}
	}
	
	IEnumerator PlayBindongEffect()
	{
		GameData.Instance.Wudi = true;
		StartCoroutine(IceFreeze());
		StartCoroutine(DropIceBall());
		yield return new WaitForSeconds(1.2f);
		GameData.Instance.Wudi = false;
		for (int i = EnemyManager.Instance.transform.childCount - 1; i >= 0; i--)
		{
			if (EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>() != null)
			{
				EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().StartFreezeEnemy(10f);
				EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().onDamage(
					Mathf.Min(50f, EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().HP), Vector3.zero);//.HP -= Mathf.Min(50f, enemy.HP);
			}
		}
	}
	
	IEnumerator IceFreeze()
	{
		//		freezeBehaviour.isFrozen = true;
		yield return new WaitForSeconds(5f);
		//		freezeBehaviour.isFrozen = false;
	}
	
	IEnumerator dropGunCD(float duraion)
	{
		gunFreezed = true;
		float rate = 1f / duraion;
		float tmp = 0f;
		while(tmp < 1f)
		{
			tmp += Time.deltaTime * rate;
			gunMaskObj.fillAmount = Mathf.Lerp(1f, 0f, tmp);
			yield return null;
		}
		//		yield return new WaitForSeconds(duraion);
		gunFreezed = false;
	}
	
	private bool gunFreezed = false;
	public void OnDropGunPorp()
	{
		if (gunFreezed)
			return;
		
		if (SettingManager.Instance.TotalJiatelin <= 0)
			return;
		
		SettingManager.Instance.UseDaojuTime += 1;
		SettingManager.Instance.DaojuCost_Shuaxin += 1;
		float duration = IOHelper.GetItemInfoById(4).cd_time;
		StartCoroutine(dropGunCD(duration));
		
		StartCoroutine(PlayDropGunEffect());
		
		GameData.Instance.AddJiatelin(-1);
		MTAManager.DoEvent(MTAPoint.MINI_USE_JIATELIN);
		jiatelinLabel.text = SettingManager.Instance.TotalJiatelin.ToString();
	}
	
	IEnumerator PlayDropGunEffect()
	{
		GameData.Instance.Wudi = true;
		player.GetComponent<HeroController>().ChangeState(AnimState.daojuAttack);
		PlayerCamera.gameObject.SetActive(true);
		GameObject effect1 = (GameObject)Instantiate(Resources.Load("prefabs/Effects/daoju_jiatelin"));
		effect1.transform.parent = player.transform;
		effect1.transform.position = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
		yield return new WaitForSeconds(2f);
		WeaponType weaponId = WeaponType.gun_jiatelin;
		EventService.Instance.GetEvent<WeaponChangeEvent>().Publish(weaponId, 0);
		PlayerCamera.gameObject.SetActive(false);
		GameData.Instance.Wudi = false;
		
	}
	
	IEnumerator aoeCD(float duration)
	{
		aoeFreezed = true;
		float rate = 1f / duration;
		float tmp = 0f;
		while(tmp < 1f)
		{
			tmp += Time.deltaTime * rate;
			aoeMaskObj.fillAmount = Mathf.Lerp(1f, 0f, tmp);
			yield return null;
		}
		//		yield return new WaitForSeconds(duraion);
		aoeFreezed = false;
	}
	
	private bool aoeFreezed =false;
	public void OnAoeProp()
	{
		if (aoeFreezed)
			return;
		
		if (SettingManager.Instance.TotalAoe <= 0)
			return;
		
		SettingManager.Instance.UseDaojuTime += 1;
		SettingManager.Instance.DaojuCost_Shuaxin += 1;
		GameData.Instance.AddAoe(-1);
		MTAManager.DoEvent(MTAPoint.MINI_USE_AOE);
		float duration = IOHelper.GetItemInfoById(1).cd_time;
		StartCoroutine(aoeCD(duration));
		
		StartCoroutine(PlayAOEEffects());
		
		
		aoeLabel.text = SettingManager.Instance.TotalAoe.ToString();
	}
	
	public void PlayAoeEffect()
	{
		StartCoroutine(PlayAOEEffects());
	}
	
	IEnumerator PlayAOEEffects()
	{
		GameData.Instance.Wudi = true;
		player.GetComponent<HeroController>().ChangeState(AnimState.daojuAttack);
		PlayerCamera.gameObject.SetActive(true);
		GameObject effect1 = (GameObject)Instantiate(Resources.Load("prefabs/Effects/daoju_qingping01"));
		effect1.transform.parent = player.transform;
		effect1.transform.position = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
		yield return new WaitForSeconds(2f);
		GameData.Instance.Wudi = false;
		PlayerCamera.gameObject.SetActive(false);
		GameObject effect2 = (GameObject)Instantiate(Resources.Load("prefabs/Effects/daoju_qingping02"));
		effect2.transform.parent = player.transform;
		effect2.transform.position = new Vector3(player.transform.position.x, 2f, player.transform.position.z);
		
		for (int i = EnemyManager.Instance.transform.childCount - 1; i >= 0; i--)
		{
			if (EnemyManager.Instance.transform.GetChild(i) != null && EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>() != null)
			{
				if ((int)EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().currentType > 100)
					EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().onDamage(Mathf.Min(500f, EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().HP), Vector3.zero);//.HP -= Mathf.Min(50f, enemy.HP);
				else
					EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().onDamage(EnemyManager.Instance.transform.GetChild(i).GetComponent<EnemyController>().HP, Vector3.zero);
			}
		}
	}
}
