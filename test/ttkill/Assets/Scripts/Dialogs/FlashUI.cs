using UnityEngine;
using System.Collections;

public class FlashUI : DialogBase {
	public GameObject pic1;
	public GameObject pic2;
	public GameObject pic3;
	public GameObject pic4;
	public GameObject pic5;
	public GameObject pic6;

	public static void PopUp()
	{
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/FlashDialog");
	}

	private int currentStep = 1;
	// Use this for initialization
	void Start () {
		currentStep = 2;
		TweenPosition tp =TweenPosition.Begin(pic1, 1f, new Vector3(-322f, 0, 0));
		tp.style = UITweener.Style.Once;
		tp.eventReceiver = gameObject;
		tp.callWhenFinished = "NextStep";
//		tp.method = UITweener.Method.;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator NextStep()
	{
		yield return new WaitForSeconds(2f);
		OnDemo();
	}
	
	public void OnDemo()
	{
		if (currentStep == 2)
		{
			currentStep = 3;
			TweenPosition tp =TweenPosition.Begin(pic2, 1.5f, new Vector3(322f, 0, 0));
			tp.delay = 1f;
			tp.style = UITweener.Style.Once;
			tp.eventReceiver = gameObject;
			tp.callWhenFinished = "NextStep";
//			tp.method = UITweener.Method.BounceIn;
		}
		else if (currentStep == 3)
		{
			currentStep = 4;
			pic1.SetActive(false);
			pic2.SetActive(false);
			TweenPosition tp =TweenPosition.Begin(pic3, 1f, new Vector3(-322f, 180, 0));
			tp.delay = 1f;
			tp.style = UITweener.Style.Once;
			tp.eventReceiver = gameObject;
			tp.callWhenFinished = "NextStep";
//			tp.method = UITweener.Method.BounceIn;
		}
		else if (currentStep == 4)
		{
			currentStep = 5;
			TweenPosition tp =TweenPosition.Begin(pic4, 1f, new Vector3(322f, 180, 0));
			tp.delay = 1f;
			tp.style = UITweener.Style.Once;
			tp.eventReceiver = gameObject;
			tp.callWhenFinished = "NextStep";
//			tp.method = UITweener.Method.BounceIn;
		}
		else if (currentStep == 5)
		{
			currentStep = 6;
			TweenPosition tp =TweenPosition.Begin(pic5, 1f, new Vector3(-322f, -180, 0));
			tp.delay = 1f;
			tp.style = UITweener.Style.Once;
			tp.eventReceiver = gameObject;
			tp.callWhenFinished = "NextStep";
//			tp.method = UITweener.Method.BounceIn;
		}
		else if (currentStep == 6)
		{
			currentStep = 7;
			TweenPosition tp =TweenPosition.Begin(pic6, 1f, new Vector3(322f, -180, 0));
			tp.delay = 1f;
			tp.style = UITweener.Style.Once;
			tp.eventReceiver = gameObject;
			tp.callWhenFinished = "NextStep";
//			tp.method = UITweener.Method.BounceIn;
		}
		else if (currentStep >= 7)
		{
			SettingManager.Instance.FirstIn = 0;
			LoadingScene.Load("changjing01");
		}
//		Application.LoadLevel("changjing01");
	}
}
