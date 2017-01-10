using UnityEngine;
using System.Collections;
public enum TutorialDir
{
	None,
	Up,
	Right,
	Down,
	Left,
}

public class TutorialParam : DialogParam
{
	public GameObject target;//the object should to be clone;
	public bool needIncrease; //check add tutorialSeq or not
	public bool auto;  // check should auto fire next tutoiralSeq event
	public TutorialDir dir;
}

public class TutorialDialog : BaseDialog<TutorialParam> {

	public static void PopUp(TutorialParam param)
	{
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/TutorialDialog", param);
	}

	void Start()
	{
		GameObject btn = Instantiate(this.Param.target, this.Param.target.transform.position, this.Param.target.transform.localRotation) as GameObject;
		btn.transform.parent = transform;
		if (this.Param.target.GetComponent<TutorialTrigger>().index == 18) //for fu
		{
			StartCoroutine(adjustPos(btn));
//			btn.transform.localPosition = Vector3.zero;// new Vector3(btn.transform.localPosition.x - 640f, btn.transform.localPosition.y, btn.transform.localPosition.z);
		}
		btn.transform.localScale = Vector3.one;

		if (this.Param.dir != TutorialDir.None)
		{
			GameObject arrow = null;
			Vector3 pos = Vector3.zero;
			switch(this.Param.dir)
			{
			case TutorialDir.Down:
				arrow = (GameObject)Instantiate(Resources.Load("UI/arrowBottomPos"));
				pos = new Vector3(btn.transform.localPosition.x, btn.transform.position.y - btn.GetComponent<UISprite>().localSize.y / 2 - 55, 0);
				break;
			case TutorialDir.Up:
				arrow = (GameObject)Instantiate(Resources.Load("UI/arrowUpPos"));
				pos = new Vector3(btn.transform.localPosition.x, btn.transform.localPosition.y + btn.GetComponent<UISprite>().localSize.y / 2 + 55, 0);
				break;
			case TutorialDir.Left:
				arrow = (GameObject)Instantiate(Resources.Load("UI/arrowLeftPos"));
				pos = new Vector3(btn.transform.localPosition.x - btn.GetComponent<UISprite>().localSize.x / 2 - 55, btn.transform.localPosition.y, 0);
				break;
			case TutorialDir.Right:
				arrow = (GameObject)Instantiate(Resources.Load("UI/arrowRightPos"));
				pos = new Vector3(btn.transform.localPosition.x + btn.GetComponent<UISprite>().localSize.x / 2 + 55, btn.transform.localPosition.y, 0);
				break;
			}

			arrow.transform.parent = transform;
			arrow.transform.localPosition = pos;
			arrow.transform.localScale = Vector3.one;
		}

//		btn.transform.GetComponent<TutorialTrigger>().enabled = false;
		TutorialTrigger[] triggers = btn.transform.GetComponents<TutorialTrigger>();
		foreach(TutorialTrigger trig in triggers)
		{
			trig.enabled = false;
		}

		btn.AddComponent<UICloseTutorial>().Init(this.Param.needIncrease, this.Param.auto);
		TweenScale ts = TweenScale.Begin(btn, 1f, Vector3.one * 1.2f);
		ts.style = UITweener.Style.PingPong;
	}

	IEnumerator adjustPos(GameObject  btn)
	{
		yield return new WaitForSeconds(1);
		btn.transform.localPosition = new Vector3(btn.transform.localPosition.x - 640f, btn.transform.localPosition.y, btn.transform.localPosition.z);
	}
}
