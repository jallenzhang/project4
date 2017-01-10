using UnityEngine;
using System.Collections;

public class TuorialTriggerManager {
	private static TuorialTriggerManager instance;

	public static TuorialTriggerManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new TuorialTriggerManager();
			}

			return instance;
		}
	}

	public void OpenTutorialDialogByIndex(int index = 0)
	{
		if (index == 0)
			EventService.Instance.GetEvent<TutorialEvent>().Publish(SettingManager.Instance.TutorialSeq);
		else
			EventService.Instance.GetEvent<TutorialEvent>().Publish(index);
	}


}
