using UnityEngine;
using System.Collections;

public class UICloseTutorial : MonoBehaviour {
	bool m_needIncreaseSeq = true;
	bool m_auto = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(bool needIncreaseSeq, bool auto)
	{
		m_needIncreaseSeq = needIncreaseSeq;
		m_auto = auto;
	}

	void OnClick()
	{
		Time.timeScale = 1.2f;
		DialogManager.Instance.CloseDialog();
		if (m_needIncreaseSeq)
		{
			SettingManager.Instance.TutorialSeq += 1;
			if (m_auto)
				TuorialTriggerManager.Instance.OpenTutorialDialogByIndex();
		}
	}
}
