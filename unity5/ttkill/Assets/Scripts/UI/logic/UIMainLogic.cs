using UnityEngine;
using System.Collections;

public class UIMainLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		PlayerPrefs.DeleteAll();
//		PlayerPrefs.Save();

		if (SettingManager.Instance.LastestDayOfWeek == -1
		    || (System.DateTime.Now.DayOfWeek == System.DayOfWeek.Monday && SettingManager.Instance.LastestDayOfWeek == (int)System.DayOfWeek.Sunday)
		    || Mathf.Abs( SettingManager.Instance.LastDayPlay - System.DateTime.Now.Day) > 7)
		{
			SettingManager.Instance.DaysSignInWeek = 0;
		}
		
		SettingManager.Instance.LastestDayOfWeek = (int)System.DateTime.Now.DayOfWeek;
		SettingManager.Instance.LastDayPlay = System.DateTime.Now.Day;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenRankDialog()
	{
		GameCenterManager.ShowLeaderboards ();
	}

	public void OpenWeaponDialog()
	{
		StartCoroutine(onOpenWeaponDialog());
	}

	public void OpenSettingDialog()
	{
		SettingDialog.Popup();
	}

	IEnumerator onOpenWeaponDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.6f);
		WeaponDialog.Popup();
	}

	public void CloseWeaponDialog()
	{
		WeaponDialog.Close();
		MainArea.Instance.Show();
	}

	public void OpenPetDialog()
	{
		StartCoroutine(onOpenPetDialog());
	}

	IEnumerator onOpenPetDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.6f);
//		PetDialog.Popup();
		PetUpgradeDialog.Popup();
	}

	public void OpenFuDialog()
	{
		StartCoroutine(onOpenFuDialog());
	}

	IEnumerator onOpenFuDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.6f);
		FuDialog.Popup();
	}

	public void OpenPKDialog()
	{
		StartCoroutine(onOpenPKDialog());
	}

	IEnumerator onOpenPKDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.6f);
		PKDialog.Popup();
	}

	public void OpenFriendsDialog()
	{
		StartCoroutine(onOpenFriendsDialog());
	}

	IEnumerator onOpenFriendsDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.6f);
		FriendsDialog.Popup();
	}

	public void OpenSignInDialog()
	{
		StartCoroutine(onOpenSignInDialog());
	}

	IEnumerator onOpenSignInDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.6f);
		SignInDialog.Popup();
	}

	public void OpenTaskDialog()
	{
		StartCoroutine(onOpenTaskDialog());
	}

	IEnumerator onOpenTaskDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.6f);
		TaskDialog.Popup();
	}

	public void OnShopDialog()
	{
		StartCoroutine(onOnShopDialog());
	}

	IEnumerator onOnShopDialog()
	{
//		MainArea.Instance.Hide();
//		yield return new WaitForSeconds(0.6f);
		ShangChengDialog.Popup();
		yield return null;
	}

	public void OnOpenRankingListDialog()
	{
		StartCoroutine(onOnOpenRankingListDialog());
	}

	IEnumerator onOnOpenRankingListDialog()
	{
		MainArea.Instance.Hide();
		yield return new WaitForSeconds(0.6f);
		RankingListDialog.Popup();
	}

	public void OnOpenHeroUpgradeDialog()
	{
		StartCoroutine(onOnOpenHeroUpgradeDialog());
	}

	IEnumerator onOnOpenHeroUpgradeDialog()
	{
		UILayout.Instance.BottomOut();
		MainArea.Instance.Hide();
		MainArea.Instance.HideUpgradeBtn();
		yield return new WaitForSeconds(0.6f);
		HeroUpgradeDialog.Popup();
	}

	public void GotoBattle()
	{
		StartCoroutine(FireTrigger());
	}
	
	IEnumerator FireTrigger()
	{
		yield return new WaitForEndOfFrame();
		WeaponSelectionDialog.Popup("wujin");
	}

}
