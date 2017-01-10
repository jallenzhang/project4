using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {
	public int index = 1;
	public bool needIncreaseSeq = true;
	public bool needAuto = false;
	public TutorialDir direction = TutorialDir.None;
	public bool m_needDescription = false;
	public string m_description = "";
	public bool needPause = false;

	private bool isOpened = false;
	// Use this for initialization
	void Start () {

		EventService.Instance.GetEvent<TutorialEvent>().Subscribe(OpenTutorialDialog);
	}

	void OnEnable()
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OpenTutorialDialog(int idx)
	{
		if (index == idx && !isOpened)
		{
			if (needPause)
				Time.timeScale = 0;

			TutorialDialog.PopUp(new TutorialParam(){target = gameObject, needIncrease = needIncreaseSeq, auto = needAuto, dir = direction, needDescription = m_needDescription, description = m_description});
			isOpened = true;
		}
	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<TutorialEvent>().Unsubscribe(OpenTutorialDialog);
	}
}
