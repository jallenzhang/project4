//#define US_VERSION
using UnityEngine;
using System.Collections;

public class VIPDialog : DialogBase {
	public GameObject GoldAddtionalObj;
	public GameObject WeaponAddtionalObj;

	public GameObject goldDoneObj;
	public GameObject weaponDoneObj;

	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/VIPDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/VIPDialog");
#endif
	}

	void Start()
	{
		GoldAddtionalObj.collider.enabled = (SettingManager.Instance.GoldAddtional == 0);
		goldDoneObj.SetActive(SettingManager.Instance.GoldAddtional == 1);
		WeaponAddtionalObj.collider.enabled = (SettingManager.Instance.WeaponAddtional == 0);
		weaponDoneObj.SetActive(SettingManager.Instance.WeaponAddtional == 1);
	}

	public void OnBuyGoldAddtion()
	{
#if US_VERSION
		if (SettingManager.Instance.TotalDiamond < 100)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		SettingManager.Instance.GoldAddtional = 1;
		MTAManager.DoEvent(MTAPoint.MINI_BUY_VIP_TYPE_1);
		DialogManager.Instance.PopupFadeOutMessage("Buy this item success!", Vector3.zero, 3);
		GoldAddtionalObj.collider.enabled = false;
		goldDoneObj.SetActive(true);
#else
		SettingManager.Instance.GoldAddtional = 1;
		DialogManager.Instance.PopupFadeOutMessage("购买成功", Vector3.zero, 3);
		GoldAddtionalObj.collider.enabled = false;
		goldDoneObj.SetActive(true);
#endif

	}

	public void OnWeaponAddtional()
	{
#if US_VERSION
		if (SettingManager.Instance.TotalDiamond < 200)
		{
			NotEnoughDiamondDialog.Popup();
			return;
		}

		SettingManager.Instance.WeaponAddtional = 1;
		MTAManager.DoEvent(MTAPoint.MINI_BUY_VIP_TYPE_2);
		DialogManager.Instance.PopupFadeOutMessage("Buy this item success!", Vector3.zero, 3);
		WeaponAddtionalObj.collider.enabled = false;
		weaponDoneObj.SetActive(true);
#else
		SettingManager.Instance.WeaponAddtional = 1;
		DialogManager.Instance.PopupFadeOutMessage("购买成功", Vector3.zero, 3);
		WeaponAddtionalObj.collider.enabled = false;
		weaponDoneObj.SetActive(true);
#endif

	}

	public void CloseDialog()
	{
		if (UILayout.Instance != null)
			UILayout.Instance.BottomIn();
		
		if (MainArea.Instance != null)
			MainArea.Instance.Show();

		DialogManager.Instance.CloseDialog();
	}
	
}
