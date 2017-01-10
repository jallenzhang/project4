using UnityEngine;
using System.Collections;

public class TaskGrowItem : MonoBehaviour
{
	public UILabel description;
	public UILabel rewardValue;
	public UIProgressBar progress;
	public GameObject goldSprite;
	public GameObject diamondSprite;
	public GameObject btnReward;
	private TaskInfo m_info;

//	private string strValue;
//	private string strReward;

	public void OnGetReward()
	{
		if (m_info.reward_type == 1)
			GameData.Instance.AddGold(int.Parse(rewardValue.text));
		else if (m_info.reward_type == 2)
			GameData.Instance.AddDiamond(int.Parse(rewardValue.text));

		btnReward.GetComponent<Collider>().enabled = false;

		switch(m_info.id)
		{
		case 13:
			SettingManager.Instance.TaskIndex13 += 1;
			break;
		case 14:
			SettingManager.Instance.TaskIndex14 += 1;
			break;
		case 15:
			SettingManager.Instance.TaskIndex15 += 1;
			break;
		case 16:
			SettingManager.Instance.TaskIndex16 += 1;
			break;
		case 17:
			SettingManager.Instance.TaskIndex17 += 1;
			break;
		case 18:
			SettingManager.Instance.TaskIndex18 += 1;
			break;
		case 19:
			SettingManager.Instance.TaskIndex19 += 1;
			break;
		case 20:
			SettingManager.Instance.TaskIndex20 += 1;
			break;
		}

		description.text = m_info.Description;
		Init(m_info);
	}

	public void Init(TaskInfo info)
	{
		m_info = info;
		string[] rewards = info.reward.Split(';');

		int index1 = 0;
		switch(info.id)
		{
		case 13:
			index1 = SettingManager.Instance.TaskIndex13;
			break;
		case 14:
			index1 = SettingManager.Instance.TaskIndex14;
			break;
		case 15:
			index1 = SettingManager.Instance.TaskIndex15;
			break;
		case 16:
			index1 = SettingManager.Instance.TaskIndex16;
			break;
		case 17:
			index1 = SettingManager.Instance.TaskIndex17;
			break;
		case 18:
			index1 = SettingManager.Instance.TaskIndex18;
			break;
		case 19:
			index1 = SettingManager.Instance.TaskIndex19;
			break;
		case 20:
			index1 = SettingManager.Instance.TaskIndex20;
			break;
		}

		int index2 = Mathf.Min(rewards.Length -1, index1);

		rewardValue.text = rewards[index2];
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

		if (index1 >= rewards.Length)
		{
			btnReward.GetComponent<Collider>().enabled = false;
			btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
		}
		else
		{
			CheckTaskIsFinished(info);
		}
	}

	void CheckTaskIsFinished(TaskInfo info)
	{
		string value = "100";
		switch(info.id)
		{
		case 13:
			value = info.value.Split(';')[SettingManager.Instance.TaskIndex13];
			if (SettingManager.Instance.GunNum >= int.Parse(value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.GunNum / float.Parse(value);
			}
			break;
		case 14:
			value = info.value.Split(';')[SettingManager.Instance.TaskIndex14];
			if (SettingManager.Instance.MaxWeaponLevel >= int.Parse(value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.MaxWeaponLevel / float.Parse(value);
			}
			break;
		case 15:
			value = info.value.Split(';')[SettingManager.Instance.TaskIndex15];
			if (SettingManager.Instance.MaxAvatarLevel >= int.Parse(value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.MaxAvatarLevel / float.Parse(value);
			}
			break;
		case 16:
			value = info.value.Split(';')[SettingManager.Instance.TaskIndex16];
			if (SettingManager.Instance.PetNum >= int.Parse(value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.PetNum / float.Parse(value);
			}
			break;
		case 17:
			value = info.value.Split(';')[SettingManager.Instance.TaskIndex17];
			if (SettingManager.Instance.MaxPetLevel >= int.Parse(value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.MaxPetLevel / float.Parse(value);
			}
			break;
		case 18:
			value = info.value.Split(';')[SettingManager.Instance.TaskIndex18];
			if (SettingManager.Instance.TotalGoldGot >= int.Parse(value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.TotalGoldGot / float.Parse(value);
			}
			break;
		case 19:
			value = info.value.Split(';')[SettingManager.Instance.TaskIndex19];
			if (SettingManager.Instance.TotalBossKill >= int.Parse(value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.TotalBossKill / float.Parse(value);
			}
			break;
		case 20:
			value = info.value.Split(';')[SettingManager.Instance.TaskIndex20];
			if (SettingManager.Instance.NextLevel > int.Parse(value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)(SettingManager.Instance.NextLevel - 1f)/ float.Parse(value);
			}
			break;
		}
	}
}
