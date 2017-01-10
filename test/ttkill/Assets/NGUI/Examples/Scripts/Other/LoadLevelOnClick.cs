using UnityEngine;

public class LoadLevelOnClick : MonoBehaviour
{
	public string levelName;
	public GameObject main;

	void OnClick ()
	{
        Debug.Log("SettingManager.Instance.FirstIn " + SettingManager.Instance.FirstIn);
		if (SettingManager.Instance.FirstIn == 1)
		{
			main.SetActive(false);
			FlashUI.PopUp();
			return;
		}

		if (!string.IsNullOrEmpty(levelName))
		{
			Application.LoadLevel(levelName);
		}
	}

	public void OnSetting()
	{
		SettingDialog.Popup();
	}

	void Start()
	{
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();

		GameData.Instance.Init();
		if (SettingManager.Instance.LastestDayOfWeek == -1
		    || (System.DateTime.Now.DayOfWeek == System.DayOfWeek.Monday && SettingManager.Instance.LastestDayOfWeek == (int)System.DayOfWeek.Sunday))
		{
			SettingManager.Instance.DaysSignInWeek = 0;
		}

		SettingManager.Instance.LastestDayOfWeek = (int)System.DateTime.Now.DayOfWeek;

		if (SettingManager.Instance.LastDayPlay != System.DateTime.Now.Day)
		{
			SettingManager.Instance.WeaponUpgrade = 0;
			SettingManager.Instance.AdvantageModeTime = 0;
			SettingManager.Instance.ChallegeModeTime = 0;
			SettingManager.Instance.UseDaojuTime = 0;
			SettingManager.Instance.KillBossTime = 0;
			SettingManager.Instance.UseTiliNum = 0;
			SettingManager.Instance.AchievedTasks = "0";
		}
		SettingManager.Instance.LastDayPlay = System.DateTime.Now.Day;
	}
}