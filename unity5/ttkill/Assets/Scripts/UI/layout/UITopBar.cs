using UnityEngine;
using System.Collections;

public class UITopBar : MonoBehaviour {
	public UILabel labelGold;
	public UILabel labelDiamond;
	public UILabel labelTili;

//	private float originalWidth;
//	private float originalHeight;

	void Awake()
	{
		OnGoldChange(SettingManager.Instance.TotalGold);
		OnDiamondChange(SettingManager.Instance.TotalDiamond);
		OnTiliChange(SettingManager.Instance.TotalTili);
//		originalWidth = GetComponent<UISprite>().width;
//		originalHeight = GetComponent<UISprite>().height;
	}

	// Use this for initialization
	void Start () {

		GetComponent<UISprite>().width = (int)(((float)ScreenHelper.Instance.Width) / transform.localScale.x);
		SubscribeEvents();
	}

	void SubscribeEvents()
	{
		EventService.Instance.GetEvent<GoldChangeEvent> ().Subscribe (OnGoldChange);
		EventService.Instance.GetEvent<DiamondChangeEvent> ().Subscribe (OnDiamondChange);
		EventService.Instance.GetEvent<TiliChangeEvent>().Subscribe(OnTiliChange);
	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<GoldChangeEvent> ().Unsubscribe (OnGoldChange);
		EventService.Instance.GetEvent<DiamondChangeEvent> ().Unsubscribe (OnDiamondChange);
		EventService.Instance.GetEvent<TiliChangeEvent>().Unsubscribe(OnTiliChange);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGoldChange(int gold)
	{
		labelGold.text = gold.ToString();
	}

	void OnDiamondChange(int diamond)
	{
		labelDiamond.text = diamond.ToString();
	}

	void OnTiliChange(int tili)
	{
		labelTili.text = tili.ToString();
	}

	public void AddGold()
	{
//		GameData.Instance.AddGold(100000);
		ShangChengDialog.Popup();
	}

	public void AddDiamond()
	{
		ShangChengDialog.Popup();
//		GameData.Instance.AddDiamond(1000);
	}

	public void AddTili()
	{
		ShangChengDialog.Popup();
	}
}
