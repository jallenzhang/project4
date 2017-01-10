using UnityEngine;
using System.Collections;

public class UILayout : MonoBehaviour {
	public Transform top;
	public Transform bottom;
	public GameObject NotEnoughWeaponDlg;

	private Vector3 originalPos = Vector3.zero;
	private static UILayout instance = null;
	public static UILayout Instance
	{
		get
		{
			return instance;
		}
	}
	// Use this for initialization
	void Start () {
		instance = this;
		originalPos = bottom.localPosition;
//		TopIn();
//		BottomIn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TopIn()
	{
//		TweenPosition tpTop = TweenPosition.Begin(top.gameObject, 2, top.localPosition + new Vector3(0, -80, 0));
//		tpTop.from = top.localPosition;
//		tpTop.method = UITweener.Method.BounceIn;
	}

	public void TopOut()
	{
//		TweenPosition tpTop = TweenPosition.Begin(top.gameObject, 2, top.localPosition + new Vector3(0, 80, 0));
//		tpTop.from = top.localPosition;
//		tpTop.method = UITweener.Method.BounceOut;
	}

	public void BottomIn()
	{
		TweenPosition tpBottom = TweenPosition.Begin(bottom.gameObject, 0.5f, originalPos);//bottom.localPosition + new Vector3(0, 260, 0));
		tpBottom.from = bottom.localPosition;
		tpBottom.method = UITweener.Method.BounceIn;
	}

	public void BottomOut()
	{
		TweenPosition tpBottom = TweenPosition.Begin(bottom.gameObject, 0.5f, originalPos + new Vector3(0, -260, 0));//bottom.localPosition + new Vector3(0, -260, 0));
		tpBottom.from = bottom.localPosition;
		tpBottom.method = UITweener.Method.EaseOut;
	}

//	public void PopUpNotEnoughWeapon()
//	{
//		GameObject dlg = (GameObject)Instantiate(NotEnoughWeaponDlg);
//		dlg.transform.parent = transform;
//		dlg.transform.localPosition = new Vector3(0, 80, 0);
//		dlg.transform.localScale = Vector3.one;
//	}
}
