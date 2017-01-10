//#define US_VERSION
using UnityEngine;
using System.Collections;

public class SettingDialog : DialogBase {
	public Transform MusicSwitch;
	public Transform SoundSwitch;
	public GameObject lowGraphic;
	public GameObject middleGraphic;
	public GameObject highGraphic;
	private string radioChoose = "xuanzhong_dian";
	private string riadioNoChoose = "huazhixuanze_di";

	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/SettingDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/SettingDialog");
#endif
	}

	public void Confirm()
	{
		Cancel();
	}

	void Cancel()
	{
		DialogManager.Instance.CloseDialog();
	}

	void Start()
	{
		Init ();
	}

	void Init()
	{
		StartCoroutine(UpdateUI());
	}

	IEnumerator UpdateUI()
	{
//		lowGraphic.spriteName = SettingManager.Instance.Graphic == 1 ? radioChoose : riadioNoChoose;
		lowGraphic.SetActive(SettingManager.Instance.Graphic == 1);
//		middleGraphic.spriteName = SettingManager.Instance.Graphic == 2 ? radioChoose : riadioNoChoose;
		middleGraphic.SetActive(SettingManager.Instance.Graphic == 2);
//		highGraphic.spriteName = SettingManager.Instance.Graphic == 3 ? radioChoose : riadioNoChoose;
		highGraphic.SetActive(SettingManager.Instance.Graphic == 3);
		MusicSwitch.localPosition = SettingManager.Instance.Music == 1 ? new Vector3(35, 0, 0) : new Vector3(-35, 0, 0);
		SoundSwitch.localPosition = SettingManager.Instance.Sound == 1 ? new Vector3(35, 0, 0) : new Vector3(-35, 0, 0);
		yield return new WaitForEndOfFrame();
	}

	public void onMusic()
	{
		SettingManager.Instance.Music = SettingManager.Instance.Music == 0 ? 1 : 0;
		StartCoroutine(UpdateUI());
		Ultilities.gm.audioScript.toggleBGM();//.UpdateStatus();
	}

	public void onSound()
	{
		SettingManager.Instance.Sound = SettingManager.Instance.Sound == 0 ? 1 : 0;
		StartCoroutine(UpdateUI());
		Ultilities.gm.audioScript.toggleFX();
	}

	public void onLowGraphic()
	{
		SettingManager.Instance.Graphic = 1;
		StartCoroutine(UpdateUI());
	}

	public void onMiddleGraphic()
	{
		SettingManager.Instance.Graphic = 2;
		StartCoroutine(UpdateUI());
	}

	public void onHighGraphic()
	{
		SettingManager.Instance.Graphic = 3;
		StartCoroutine(UpdateUI());
	}
}
