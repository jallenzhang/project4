using UnityEngine;
using System.Collections;

public class MainArea : MonoBehaviour {
	public GameObject btnWeapon;
	public GameObject btnPet;
	public GameObject btnFu;
	public GameObject btnUpgrade;
	public UILabel tiliRemained;

	private Vector3 weaponPos = new Vector3(360f, 181f, 0);
	private Vector3 petPos = new Vector3(360f, 28f, 0);
	private Vector3 fuPos = new Vector3(360f, -124f, 0);

	private bool destroyed = false;
	private int recoverDuration = 30 * 60;

	private static MainArea instance = null;
	public static MainArea Instance
	{
		get
		{
			return instance;
		}
	}

	void OnEnable()
	{
		Show();

		TweenScale.Begin(btnWeapon, 0.2f, Vector3.one);
		TweenAlpha.Begin(btnWeapon, 0.2f, 1);
		TweenScale ts1 = TweenScale.Begin(btnPet, 0.2f, Vector3.one);
		TweenPosition tp1 = TweenPosition.Begin(btnPet, 0.2f, petPos);
		TweenAlpha ta1 = TweenAlpha.Begin(btnPet, 0.2f, 1);
		ts1.delay = 0.2f;
		tp1.delay = 0.2f;
		ta1.delay = 0.2f;

		TweenScale ts2 = TweenScale.Begin(btnFu, 0.2f, Vector3.one);
		TweenPosition tp2 = TweenPosition.Begin(btnFu, 0.2f, fuPos);
		TweenAlpha ta2 = TweenAlpha.Begin(btnFu, 0.2f, 1);
		ts2.delay = 0.4f;
		tp2.delay = 0.4f;
		ta2.delay = 0.4f;

		if (SettingManager.Instance.TotalTili < 20)
		{
			destroyed = false;
			StartRecoverTili();
		}

		TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
	}

	void OnDisable()
	{
		destroyed = true;
	}

	void Start()
	{
		instance = this;
		TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
	}

	public void StartRecoverTili()
	{
		if (SettingManager.Instance.TotalTili < 20)
		{
			StartCoroutine(TiliRecoverCorou());
		}
	}
	
	IEnumerator TiliRecoverCorou()
	{
		while(!destroyed)
		{
			System.TimeSpan ts1 = new System.TimeSpan(System.DateTime.Now.Ticks);
			System.TimeSpan ts2 = new System.TimeSpan( System.Convert.ToDateTime(SettingManager.Instance.TiliRecoverTime).Ticks);
			
			System.TimeSpan ts = ts1.Subtract(ts2).Duration();
			int count = (int)(ts.TotalSeconds) / recoverDuration;
			int remained = (int)(ts.TotalSeconds) % recoverDuration;
			count = SettingManager.Instance.TotalTili + count;
			SettingManager.Instance.TotalTili = Mathf.Min(20, count);
			
			if (SettingManager.Instance.TotalTili >= 20)
			{
				tiliRemained.gameObject.SetActive(false);
				EventService.Instance.GetEvent<TiliChangeEvent>().Publish(SettingManager.Instance.TotalTili);
				break;
			}
			else
			{
				tiliRemained.gameObject.SetActive(true);
				tiliRemained.text = Ultilities.ConvertSecondToTime(recoverDuration - remained);
				if (remained <= 0)
				{
					SettingManager.Instance.TotalTili += 1;
					EventService.Instance.GetEvent<TiliChangeEvent>().Publish(SettingManager.Instance.TotalTili);
				}
			}
			
			yield return new WaitForSeconds(1);
		}
	}

	void OnDestroy()
	{
		destroyed = true;
	}

	public void ShowSALEDialog()
	{
		StartCoroutine(onShowSALEDialog());
	}

	IEnumerator onShowSALEDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.4f);
		SaleDialog.Popup();
	}

	public void ShowVIPDialog()
	{
		StartCoroutine(onShowVIPDialog());
	}

	IEnumerator onShowVIPDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.4f);
		VIPDialog.Popup();
	}

	public void Show()
	{
		gameObject.SetActive(true);
		btnUpgrade.SetActive(true);
		GameObject main = GameObject.FindGameObjectWithTag("3DMain");
		int childCount = main.transform.childCount;
		Debug.Log("childCount " + childCount);
		for(int i = 0; i < childCount; i++)
		{
//			main.transform.GetChild(i).gameObject.SetActive(true);
			if (main.transform.GetChild(i).gameObject.name.Equals("nan01"))
			{
				main.transform.GetChild(i).gameObject.SetActive(SettingManager.Instance.CurrentAvatarId == 1);
			}
			else if (main.transform.GetChild(i).gameObject.name.Equals("nv01"))
			{
				main.transform.GetChild(i).gameObject.SetActive(SettingManager.Instance.CurrentAvatarId == 2);
			}
			else
				main.transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	public void Hide()
	{
		StartCoroutine(onHide());
	}

	public void HideUpgradeBtn()
	{
		btnUpgrade.SetActive(false);
	}

	IEnumerator onHide()
	{
		TweenScale.Begin(btnWeapon, 0.2f, Vector3.zero);
		TweenAlpha.Begin(btnWeapon, 0.2f, 0);
		TweenScale ts1 = TweenScale.Begin(btnPet, 0.2f, Vector3.zero);
		TweenAlpha ta1 = TweenAlpha.Begin(btnPet, 0.2f, 0);
		TweenPosition tp1 = TweenPosition.Begin(btnPet, 0.2f, weaponPos);
		ts1.delay = 0.2f;
		tp1.delay = 0.2f;
		ta1.delay = 0.2f;
		
		TweenScale ts2 = TweenScale.Begin(btnFu, 0.2f, Vector3.zero);
		TweenPosition tp2 = TweenPosition.Begin(btnFu, 0.2f, weaponPos);
		TweenAlpha ta2 = TweenAlpha.Begin(btnFu, 0.2f, 0);
		ts2.delay = 0.4f;
		tp2.delay = 0.4f;
		ta2.delay = 0.4f;

		yield return new WaitForSeconds(0.6f);
		gameObject.SetActive(false);
		GameObject main = GameObject.FindGameObjectWithTag("3DMain");
		int childCount = main.transform.childCount;
		for(int i = 0; i < childCount; i++)
		{
			main.transform.GetChild(i).gameObject.SetActive(false);
		}
	}
}
