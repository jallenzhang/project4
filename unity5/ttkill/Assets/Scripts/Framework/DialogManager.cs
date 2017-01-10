using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DialogManager : MonoBehaviour {

	private const int BASE_DEPTH = 20;

	private int currentMaxDepth = BASE_DEPTH;

	private Stack<GameObject> dialogQueue = new Stack<GameObject>();

	private string errorMessageDialogPath = "prefabs/Dialog/ErrorDialog/ErrorMessageDialog";
	private string errorMessageDialogWithOneButtonPath = "prefabs/Dialog/ErrorDialog/ErrorMessageDialogWithOneButton";

	public GameObject mainNode;
	public GameObject topNode;
	public GameObject middleNode;
	public GameObject bottomNode;

	private GameObject maskPanel;

	public GameObject MaskPanel {
		get {
			return maskPanel;
		}
	}

	private GameObject messagePrefab;

	private static DialogManager _instance;
	public static DialogManager Instance {
		get {
			if (_instance == null) {
				GameObject go = GameObject.FindGameObjectWithTag("DialogManager");
				if (go != null) _instance = go.GetComponent<DialogManager>();
			}
			return _instance;
		}
	}

	// Use this for initialization
	void Start () {
		_instance = this;

		messagePrefab = CommonAsset.Load("UI/debugMsg") as GameObject;

		maskPanel = NGUITools.AddChild(gameObject);
		maskPanel.AddComponent<UIPanel>();
		maskPanel.AddComponent<UIStretch>().style = UIStretch.Style.Both;
		maskPanel.AddComponent<BoxCollider>();
		var tex = maskPanel.AddComponent<UIWidget>();
		tex.autoResizeBoxCollider = true;
		tex.color = Color.black;
		maskPanel.SetActive(false);
	}

	public static DialogManager GetInstance() {
		return Instance;
	}

	public GameObject PopupDialog<T>(string prefab, T param, bool instant = true) where T : DialogParam {
		return PopupDialog(CommonAsset.Load(prefab) as GameObject, param, instant);
	}

	public GameObject PopupDialog<T>(GameObject prefab, T param, bool instant = true) where T : DialogParam {
		GameObject go = NGUITools.AddChild(gameObject, prefab);
		if (go.GetComponent<DialogBase>() != null) go.GetComponent<DialogBase>().style = DialogStyle.NormalDialog;
		PushDialog(go, param, instant);

		return go;
	}

	public GameObject PopupDialog(string prefab, bool instant = true, GameObject sender = null) {
		return PopupDialog(CommonAsset.Load(prefab), instant, sender);
	}
	
	public GameObject PopupDialog(UnityEngine.Object prefab, bool instant = true, GameObject sender = null) {
		GameObject go = NGUITools.AddChild(gameObject, prefab as GameObject);
		if (go.GetComponent<DialogBase>() != null) go.GetComponent<DialogBase>().style = DialogStyle.NormalDialog;
		PushDialog(go, sender, instant);
		
		return go;
	}


	public GameObject PopupNotificationDialog<T>(string prefab, T param, bool instant = true) where T : DialogParam {
		GameObject go = NGUITools.AddChild(gameObject, CommonAsset.Load(prefab) as GameObject);
		if (go.GetComponent<DialogBase>() != null) go.GetComponent<DialogBase>().style = DialogStyle.NotificationDialog;
		PushDialog(go, param, instant);
		
		return go;
	}

	public GameObject PopupNotificationDialog(string prefab, GameObject sender = null, bool instant = true) {
		GameObject go = NGUITools.AddChild(gameObject, CommonAsset.Load(prefab) as GameObject);
		if (go.GetComponent<DialogBase>() != null) go.GetComponent<DialogBase>().style = DialogStyle.NotificationDialog;
		PushDialog(go, sender, instant);
		
		return go;
	}


//	public GameObject PopupErrorMessageBox(string code, Action OKCallback = null) {
//		string content = ErrorCodeParser.ErrorContentByCode(code);
////		string title = ErrorCodeParser.ErrorTitleByCode(code);
//
//		return PopupMessage(content);
//	}

//	public GameObject PopupErrorMessageBox(string title, string content, Action OKCallback = null, Action CancelCallback = null, string cancelContent = null, string btn2Content = null, ErrorDialogStyle style = ErrorDialogStyle.DialogWithTwoButton)
//	{
//		GameObject errorDialog = null;
//		if (style == ErrorDialogStyle.DialogWithTwoButton)
//			errorDialog = (GameObject)Instantiate(CommonAsset.Load(errorMessageDialogPath));
//		else if (style == ErrorDialogStyle.DialogWithOneButton)
//			errorDialog = (GameObject)Instantiate(CommonAsset.Load(errorMessageDialogWithOneButtonPath));
//		else
//			return errorDialog;
//		
//		errorDialog.transform.parent = transform;
//		errorDialog.transform.localScale = Vector3.one;
//		
//		errorDialog.GetComponent<CommonMessageDialog>().InitFullUI(title, content, OKCallback, CancelCallback,cancelContent, btn2Content);
//
//		errorDialog.GetComponent<DialogBase>().style = DialogStyle.ErrorDialog;
//
//		PushDialog(errorDialog);
//		
//		return errorDialog;
//	}

//	public GameObject PopupAwardBox(string title,string content,string typeName,string Name,int ranknum,GameObject icon)
//	{
//		GameObject AwardDialog = null;
//		AwardDialog = (GameObject)Instantiate(CommonAsset.Load("prefabs/Dialog/package/Goods/OPenBoxAward"));
//
//		AwardDialog.transform.parent = transform;
//		AwardDialog.transform.localScale = Vector3.one;
//		
//		AwardDialog.GetComponent<OPenBoxAward> ().FullContent (title, content, typeName,Name, ranknum, icon);
//		
//		AwardDialog.GetComponent<DialogBase>().style = DialogStyle.ErrorDialog;
//		
//		PushDialog(AwardDialog);
//		
//		return AwardDialog;
//	}

	public void PopupServerDownDialog()
	{

	}

//	public GameObject PopupErrorMessageBox(string title, string content, Action OKCallback = null, Action CancelCallback = null)
//	{
//		GameObject errorDialog = (GameObject)Instantiate(Resources.Load(errorMessageDialogPath));
//		
//		errorDialog.transform.parent = transform;
//		errorDialog.transform.localScale = Vector3.one;
//		
//		errorDialog.GetComponent<CommonMessageDialog>().InitUI(title, content, OKCallback, CancelCallback);
//		
//		PushDialog(errorDialog);
//
//		return errorDialog;
//	}

//	public GameObject PopupErrorMessageBoxOneButton(string code, Action OKCallback = null, Action CancelCallback = null)
//	{
//		string content = ErrorCodeParser.ErrorContentByCode(code);
//		string title = ErrorCodeParser.ErrorTitleByCode(code);
//
//		GameObject errorDialog = (GameObject)Instantiate(CommonAsset.Load(errorMessageDialogWithOneButtonPath));
//
//		errorDialog.transform.parent = transform;
//		errorDialog.transform.localScale = Vector3.one;
//		errorDialog.transform.localPosition = Vector3.zero;
//
//		errorDialog.GetComponent<CommonMessageDialog>().InitUI(title, content, OKCallback, CancelCallback);
//
//		errorDialog.GetComponent<DialogBase>().style = DialogStyle.ErrorDialog;
//		PushDialog(errorDialog);
//
//		return errorDialog;
//	}

//	private float lastPopupMessageTime = 0;
//	public GameObject PopupMessage(string msg, float duration = 1.2f) {
//		if (string.IsNullOrEmpty(msg)) return null;
//
//		string strippenMsg = NGUIText.StripSymbols(msg);
//		if (strippenMsg.Length > 16) { // insert \n
//			int insertIndex = 16;
//			for (int i = insertIndex - 1; i >= insertIndex - 7 && i >= 0; i--) {
//				if (msg[i] == '[') {
//					int sub = 0;
//					bool bold = false;
//					bool italic = false;
//					bool underline = false;
//					bool strikethrough = false;
//					int retVal = i;
//
//					if (NGUIText.ParseSymbol(msg, ref retVal, null, false, ref sub, ref bold, ref italic, ref underline, ref strikethrough)) {
//						insertIndex = i;
//					}
//
//					break;
//				}
//			}
//			msg = msg.Insert(insertIndex, "\n");
//		}
//
//		float timeSinceLast = Time.time - lastPopupMessageTime;
//		float delay = timeSinceLast > 0.5f ? 0.0f : 0.5f - timeSinceLast;
//
//		GameObject o = Instantiate(messagePrefab) as GameObject;
//		o.GetComponentInChildren<UILabel>().text = msg;
//		o.GetComponent<UIPanel>().depth = 999;
//
//		o.transform.parent = transform;
//		o.transform.localScale = Vector3.one;
//		o.transform.localPosition = Vector3.zero;
//
//		var tp = TweenPosition.Begin(o, duration, new Vector3(0, 500));
//		tp.method = UITweener.Method.EaseOut;
//		tp.onFinished.Add(new EventDelegate(()=>Destroy(o, 0.1f)));
//		tp.delay = delay;
//
//		var ta = TweenAlpha.Begin(o, duration, 0);
//		ta.steeperCurves = true;
//		ta.method = UITweener.Method.EaseIn;
//		ta.delay = delay;
//
//		lastPopupMessageTime = Time.time + delay;
//
//		return o;
//	}

	public GameObject PopupFadeOutMessage(string msg, Vector3 pos, float duration = 1.2f) {
		GameObject o = Instantiate(messagePrefab) as GameObject;
//		o.transform.parent = transform;
		o.GetComponentInChildren<UILabel>().text = msg;
		o.GetComponent<UIPanel>().depth = 999;
		
		o.transform.parent = transform;
		o.transform.localScale = new Vector3(0.2f, 0.2f);
		o.transform.localPosition = pos;

		var ta = TweenAlpha.Begin(o, duration, 0);
		ta.steeperCurves = true;
		ta.method = UITweener.Method.EaseIn;
		ta.onFinished.Add(new EventDelegate(()=>Destroy(o, 0.1f)));

		var ts = TweenScale.Begin(o, duration/4, Vector3.one);
		ts.method = UITweener.Method.EaseOut;

		return o;
	}

	public GameObject PopupFadeOutMessage(string msg, float duration = 1.2f) {
		return PopupFadeOutMessage(msg, new Vector3(0, 200), duration);
	}

	private void AdjustPanelDepth(GameObject go, bool increase = true)
	{
		UIPanel[] panels = go.GetComponentsInChildren<UIPanel>(true);
		if (panels == null || panels.Length == 0) return;

		int minDepth = int.MaxValue;
		int maxDepth = int.MinValue;
		foreach (UIPanel p in panels)
		{
			if (p.depth < minDepth) minDepth = p.depth;
			if (p.depth > maxDepth) maxDepth = p.depth;
		}

		if (increase)
		{
			foreach (UIPanel p in panels)
			{
				p.depth = p.depth - minDepth + currentMaxDepth;
			}

			currentMaxDepth = currentMaxDepth + maxDepth - minDepth + 10;
		}
		else
		{
			currentMaxDepth = minDepth;
		}
	}

	private void PushDialog<T>(GameObject go, T param, bool instant) where T : DialogParam
	{
		try {
			AdjustPanelDepth(go);
			
			if (dialogQueue.Count > 0)
			{
				var topDialog = dialogQueue.Peek().GetComponent<DialogBase>();
				if (topDialog != null && go.GetComponent<DialogBase>().style == DialogStyle.NormalDialog)
				{
					topDialog.OnPause();
				}
			}
			
			var newDialog = go.GetComponent<BaseDialog<T>>();
			if (newDialog != null)
			{
				newDialog.Init(param);
//				newDialog.OnResume();
				if (newDialog.IsFullScreen() && mainNode != null) {
//					mainNode.SetActive(false);
				}
			}
		} catch (Exception e) {
			Debug.LogError(e);
		}
		
		dialogQueue.Push (go);

//		PlayOpenDialogAnimation(go, instant);

//		EventService.Instance.GetEvent<TutorialEvent>().Publish(SettingManager.Instance.TutorialSeq);
	}

	public GameObject GetTopDialog(){
		return dialogQueue.Peek ();
	}

	public int GetDialogCountInScreen()
	{
		return dialogQueue.Count;
	}


	//TODO: should delete it when refactor done!
	private void PushDialog(GameObject go, GameObject sender = null, bool instant = true)
	{
		try {
			AdjustPanelDepth(go);

			if (dialogQueue.Count > 0)
			{
				DialogBase curDb = dialogQueue.Peek().GetComponent<DialogBase>();
				if (curDb != null && go.GetComponent<DialogBase>().style == DialogStyle.NormalDialog)
				{
					curDb.OnPause();
				}
			}

			DialogBase newDb = go.GetComponent<DialogBase>();
			if (newDb != null)
			{
				if (sender != null)
				{
					newDb.Init(sender, null);
				}
//				newDb.OnResume();
				if (newDb.IsFullScreen() && mainNode != null) {
//					mainNode.SetActive(false);
				}
			}
		} catch (Exception e) {
			Debug.LogError(e);
		}

		dialogQueue.Push (go);

//		PlayOpenDialogAnimation(go, instant);

//		EventService.Instance.GetEvent<TutorialEvent>().Publish(SettingManager.Instance.TutorialSeq);
	}

//	void PlayOpenDialogAnimation(GameObject go, bool instant)
//	{
//		if (gameObject.activeInHierarchy) StartCoroutine(CoPlayOpenDialogAnimation(go, instant));
//	}
//
//	IEnumerator CoPlayOpenDialogAnimation(GameObject go, bool instant)
//	{
//		if (go != null)
//		{
//			UIPanel[] panels = go.GetComponentsInChildren<UIPanel>();
//			float[] alphas = new float[panels.Length];
//			int minPanelDepth = int.MaxValue;
//			for (int i = 0; i < panels.Length; i++)
//			{
//				alphas[i] = panels[i].alpha;
//				panels[i].alpha = 0;
//				if (panels[i].depth < minPanelDepth) minPanelDepth = panels[i].depth;
//			}
//
////			if (maskPanel != null) maskPanel.SetActive(true);
//			if (maskPanel != null) maskPanel.GetComponent<UIPanel>().depth = minPanelDepth - 1;
//			
//			yield return new WaitForEndOfFrame();
//			yield return new WaitForEndOfFrame();
//
//			if (go != null)
//			{
//				for (int i = 0; i < panels.Length; i++)
//				{
//					panels[i].alpha = alphas[i];
//				}
//
//				UIAnchor[] anchors = go.GetComponentsInChildren<UIAnchor>();
//				foreach (var a in anchors) a.enabled = false;
//
//				ResetDraggablePanel[] rdps = go.GetComponentsInChildren<ResetDraggablePanel>();
//				foreach (var p in rdps) p.enabled = false;
//
//				UIStretch[] stretchs = go.GetComponentsInChildren<UIStretch>();
//				foreach (var s in stretchs) s.enabled = false;
//
//				for (int i = 0; i < panels.Length; i++)
//				{
//					TweenAlpha ta = TweenAlpha.Begin(panels[i].gameObject, instant ? 0.0f : 1.0f, alphas[i]);
//					ta.from = 0.0f;
//					ta.steeperCurves = true;
//					ta.method = UITweener.Method.EaseOut;
//					if (i == 0)
//					{
//						ta.onFinished.Add(new EventDelegate(() => {
//							foreach (var a in anchors) a.enabled = true;
//							foreach (var p in rdps) p.enabled = true;
//							foreach (var s in stretchs) if(s != null) s.enabled = true;
//						}));
//					}
//					if (instant)
//					{
//						foreach (var a in anchors) a.enabled = true;
//						foreach (var p in rdps) p.enabled = true;
//						foreach (var s in stretchs) if(s != null) s.enabled = true;
//					}
//				}
//			}
//		}
//	}
//
	void PlayCloseDialogAnimation(GameObject go)
	{
		if (go != null)
		{
			maskPanel.SetActive(false);
			Destroy(go);
			return;
		}
	}

	public void CloseDialog()
	{
		if (dialogQueue.Count <= 0) {
			return;
		}

		GameObject go = dialogQueue.Pop ();
		var ds = go.GetComponent<DialogBase>();
		DialogStyle goDialogStyle = ds == null ? DialogStyle.NormalDialog : ds.style;
//		AdjustPanelDepth(go, false);
		Debug.Log("close dialog " + go.name);
		PlayCloseDialogAnimation(go);

		if (dialogQueue.Count > 0)
		{
			DialogBase topDb = dialogQueue.Peek().GetComponent<DialogBase>();
			if (topDb != null && goDialogStyle == DialogStyle.NormalDialog)
			{
				topDb.OnResume();
			}
		}

		// check full screen dialog exist
		foreach (var item in dialogQueue) {
			if (item != null) {
				DialogBase dlg = item.GetComponent<DialogBase>();
				if (dlg != null && dlg.IsFullScreen()) {
					return;
				}
			}
		}
		if (mainNode != null && dialogQueue.Count == 0) { // no fullscreen dialog exist
			mainNode.SetActive(true);
			if (middleNode != null)
				middleNode.SetActive(true);
		}

		if (dialogQueue.Count == 0) currentMaxDepth = BASE_DEPTH;

//		EventService.Instance.GetEvent<TutorialEvent>().Publish(SettingManager.Instance.TutorialSeq);
	}

	public void ClearAll()
	{
		while (dialogQueue.Count > 0) 
		{
			CloseDialog();
		}
		currentMaxDepth = BASE_DEPTH;

//		EventService.Instance.GetEvent<TutorialEvent>().Publish(SettingManager.Instance.TutorialSeq);
	}
}
