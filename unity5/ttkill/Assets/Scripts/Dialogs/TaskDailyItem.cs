using UnityEngine;
using System.Collections;

public class TaskDailyItem : MonoBehaviour
{
	public UILabel description;
	public UILabel rewardValue;
	public UIProgressBar progress;
	public GameObject goldSprite;
	public GameObject diamondSprite;
	public GameObject btnReward;
	private TaskInfo m_info;
	private string RewardGot = "已领取";

	public void OnGetReward()
	{
		if (m_info.reward_type == 1)
			GameData.Instance.AddGold(int.Parse(m_info.reward));
		else if (m_info.reward_type == 2)
			GameData.Instance.AddDiamond(int.Parse(m_info.reward));

		SettingManager.Instance.AchievedTasks += ":"+m_info.id;
		btnReward.GetComponent<Collider>().enabled = false;
		StartCoroutine(ChangeDisableColor());
	}

	IEnumerator ChangeDisableColor()
	{
		yield return new WaitForSeconds (0.5f);
		btnReward.GetComponentInChildren<UILabel>().text = RewardGot;
		btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
	}

	public void Init(TaskInfo info)
	{
		m_info = info;
		rewardValue.text = info.reward;
		if (info.reward_type == 1)
		{
			goldSprite.SetActive(true);
			diamondSprite.SetActive(false);
		}
		else
		{
			goldSprite.SetActive(false);
			diamondSprite.SetActive(true);
		}

		string[] strs = SettingManager.Instance.AchievedTasks.Split(':');
		
		foreach(string str in strs)
		{
			int num = int.Parse(str);
			if (num != 0 && num == info.id)
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponentInChildren<UILabel>().text = RewardGot;

				return;
			}
		}

		CheckTaskIsFinished(info);
	}

	IEnumerator FireTrigger()
	{
		yield return new WaitForSeconds(0.3f);
		TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
	}

	void CheckTaskIsFinished(TaskInfo info)
	{
		switch(info.id)
		{
		case 7:
			if (SettingManager.Instance.WeaponUpgrade >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
				TutorialTrigger tt = btnReward.AddComponent<TutorialTrigger>();
				tt.index = 10;
				tt.needIncreaseSeq = true;
				StartCoroutine(FireTrigger());
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				progress.value = (float)SettingManager.Instance.WeaponUpgrade / float.Parse(info.value);
			}
			break;
		case 8:
			if (SettingManager.Instance.AdvantageModeTime >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				progress.value = (float)SettingManager.Instance.AdvantageModeTime / float.Parse(info.value);
			}
			break;
		case 9:
			if (SettingManager.Instance.ChallegeModeTime >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				progress.value = (float)SettingManager.Instance.ChallegeModeTime / float.Parse(info.value);
			}
			break;
		case 10:
			if (SettingManager.Instance.UseDaojuTime >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				progress.value = (float)SettingManager.Instance.UseDaojuTime / float.Parse(info.value);
			}
			break;
		case 11:
			if (SettingManager.Instance.KillBossTime >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				progress.value = (float)SettingManager.Instance.KillBossTime / float.Parse(info.value);
			}
			break;
		case 12:
			if (SettingManager.Instance.UseTiliNum >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				progress.value = (float)SettingManager.Instance.UseTiliNum / float.Parse(info.value);
			}
			break;
		}
	}
}
