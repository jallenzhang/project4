using UnityEngine;
using System.Collections;

public class TaskNormalItem : MonoBehaviour
{
	public UILabel description;
	public UILabel rewardValue;
	public UIProgressBar progress;
	public GameObject goldSprite;
	public GameObject diamondSprite;
	public GameObject btnReward;
	private TaskInfo m_info;

	public void OnRefresh()
	{
		if (SettingManager.Instance.TotalDiamond < 2)
		{
			NotEnoughGoldDialog.Popup();
			return;
		}

		GameData.Instance.AddDiamond(-2);

		var dlg = NGUITools.FindInParents<TaskDialog>(transform);
		if (dlg != null)
		{
			dlg.OnNormalRefreshClick(this);
		}
	}

	public void OnGetReward()
	{
		if (m_info.reward_type == 1)
			GameData.Instance.AddGold(int.Parse(m_info.reward));
		else if (m_info.reward_type == 2)
			GameData.Instance.AddDiamond(int.Parse(m_info.reward));

		btnReward.GetComponent<Collider>().enabled = false;
		btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
		var dlg = NGUITools.FindInParents<TaskDialog>(transform);
		if (dlg != null)
		{
			dlg.OnNormalRefreshClick(this);
		}
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

		CheckTaskIsFinished(info);
	}

	public void Reset(TaskInfo info)
	{
		switch(info.id)
		{
		case 1:
			SettingManager.Instance.DaojuCost_Shuaxin = 0;
			break;
		case 2:
			SettingManager.Instance.FuGot_Shuaxin = 0;
			break;
		case 3:
			SettingManager.Instance.KillBoss_Shuaxin = 0;
			break;
		case 4:
			SettingManager.Instance.GoldGot_Shuaxin = 0;
			break;
		case 5:
			SettingManager.Instance.JiqiangGot_Shuaxin = 0;
			break;
		case 6:
			SettingManager.Instance.CostGoldNum_Shuaxin = 0;
			break;
		}
	}

	void CheckTaskIsFinished(TaskInfo info)
	{
		switch(info.id)
		{
		case 1:
			if (SettingManager.Instance.DaojuCost_Shuaxin >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.DaojuCost_Shuaxin / float.Parse(info.value);
			}
			break;
		case 2:
			if (SettingManager.Instance.FuGot_Shuaxin >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.FuGot_Shuaxin / float.Parse(info.value);
			}
			break;
		case 3:
			if (SettingManager.Instance.KillBoss_Shuaxin >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.KillBoss_Shuaxin / float.Parse(info.value);
			}
			break;
		case 4:
			if (SettingManager.Instance.GoldGot_Shuaxin >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.GoldGot_Shuaxin / float.Parse(info.value);
			}
			break;
		case 5:
			if (SettingManager.Instance.JiqiangGot_Shuaxin >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.JiqiangGot_Shuaxin / float.Parse(info.value);
			}
			break;
		case 6:
			if (SettingManager.Instance.CostGoldNum_Shuaxin >= int.Parse(info.value))
			{
				btnReward.GetComponent<Collider>().enabled = true;
				progress.value = 1;
			}
			else
			{
				btnReward.GetComponent<Collider>().enabled = false;
				btnReward.GetComponent<UISprite>().spriteName = "lingqujiangli_hui";
				progress.value = (float)SettingManager.Instance.CostGoldNum_Shuaxin / float.Parse(info.value);
			}
			break;
		}
	}

}
