using UnityEngine;
using System.Collections;
using System;

public class TutorialStoryDialogParam : DialogParam
{
	public Action m_callback;
}

public class TutorialStoryDialog : BaseDialog<TutorialStoryDialogParam> {
	public GameObject step1;
	public GameObject step2;
	public GameObject step3;

	int currentStep = 1;

	public static void Popup(Action callback)
	{
		#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/TutorialStoryDialog", new TutorialStoryDialogParam(){m_callback = callback});
		#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/TutorialStoryDialog", new TutorialStoryDialogParam(){m_callback = callback});
		#endif
	}

	void Start()
	{
		Time.timeScale = 0;
	}

	public void GotoNext()
	{
		if (++currentStep < 4)
		{
			step1.SetActive(currentStep == 1);
			step2.SetActive(currentStep == 2);
			step3.SetActive(currentStep == 3);
		}
		else
		{
			Time.timeScale = 1.2f;
			if (this.Param.m_callback != null)
			{
				this.Param.m_callback();
			}
			SettingManager.Instance.TutorialSeq++;
			DialogManager.Instance.CloseDialog();
		}
	}
}
