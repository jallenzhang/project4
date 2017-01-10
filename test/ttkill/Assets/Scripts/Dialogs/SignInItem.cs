using UnityEngine;
using System.Collections;

public class SignInItem : MonoBehaviour {
	public DAYOFSIGN day;
	public System.DayOfWeek dw;
//	public GameObject reward;
	public GameObject alreaySign;

	TweenScale ts = null;
	private bool canReward = false;
//	private 
	// Use this for initialization
	void Start () {
		if (day == DAYOFSIGN.All)
			EventService.Instance.GetEvent<SignInEvent>().Subscribe(UpdateUI);

		UpdateUI();
	}

	void UpdateUI()
	{
		if (day == DAYOFSIGN.All)
		{
			if (SettingManager.Instance.DaysSignInWeek == ((int)DAYOFSIGN.All - 1))
			{
				alreaySign.SetActive(false);
				canReward = true;
				if (System.DateTime.Now.DayOfWeek == dw)
				{
					ts = TweenScale.Begin(gameObject, 1f, Vector3.one * 1.1f);
					ts.method = UITweener.Method.Linear;
					ts.style = UITweener.Style.PingPong;
				}
			}
			else if (SettingManager.Instance.DaysSignInWeek == (int)DAYOFSIGN.All * 2 - 1)
			{
				alreaySign.SetActive(true);
			}


		}
		else if (day == Convert())
		{
			if ((SettingManager.Instance.DaysSignInWeek & (int)day) != 0)
			{
				alreaySign.SetActive(true);
			}
			else
			{
				alreaySign.SetActive(false);
				
				if (System.DateTime.Now.DayOfWeek == dw)
				{
					canReward = true;
					ts = TweenScale.Begin(gameObject, 1f, Vector3.one * 1.1f);
					ts.method = UITweener.Method.Linear;
					ts.style = UITweener.Style.PingPong;
				}
			}
		}
	}

	void OnDestroy()
	{
		if (day == DAYOFSIGN.All)
			EventService.Instance.GetEvent<SignInEvent>().Unsubscribe(UpdateUI);
	}

	DAYOFSIGN Convert()
	{
		DAYOFSIGN ret = DAYOFSIGN.Mondy;
		switch(dw)
		{
		case System.DayOfWeek.Monday:
			ret = DAYOFSIGN.Mondy;
			break;
		case System.DayOfWeek.Tuesday:
			ret = DAYOFSIGN.Tuesday;
			break;
		case System.DayOfWeek.Wednesday:
			ret = DAYOFSIGN.Wednesday;
			break;
		case System.DayOfWeek.Thursday:
			ret = DAYOFSIGN.Thursday;
			break;
		case System.DayOfWeek.Friday:
			ret = DAYOFSIGN.Friday;
			break;
		case System.DayOfWeek.Saturday:
			ret = DAYOFSIGN.Saturday;
			break;
		case System.DayOfWeek.Sunday:
			ret = DAYOFSIGN.Sunday;
			break;
		}

		return ret;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onSign()
	{
		if (canReward)
		{
			canReward = false;
			if (ts != null)
			{
//				ts.enabled = false;
				Destroy(ts);
			}
			transform.localScale = Vector3.one;
			SettingManager.Instance.DaysSignInWeek |= (int)day;
			Reward();
			alreaySign.SetActive(true);
			EventService.Instance.GetEvent<SignInEvent>().Publish();
		}

	}

	void Reward()
	{
		switch(day)
		{
		case DAYOFSIGN.Mondy:
			GameData.Instance.AddGold(5000);
			break;
		case DAYOFSIGN.Tuesday:
			GameData.Instance.AddJiaxue(1);
			break;
		case DAYOFSIGN.Wednesday:
			GameData.Instance.AddBindong(1);
			break;
		case DAYOFSIGN.Thursday:
			GameData.Instance.AddDiamond(30);
			break;
		case DAYOFSIGN.Friday:
			GameData.Instance.AddAoe(1);
			break;
		case DAYOFSIGN.Saturday:
			GameData.Instance.AddJiatelin(1);
			break;
		case DAYOFSIGN.Sunday:
			GameData.Instance.AddGold(5000);
			break;
		case DAYOFSIGN.All:
			GameData.Instance.AddDiamond(50);
			break;
		}
	}
}
