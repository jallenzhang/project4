//#define US_VERSION
using UnityEngine;
using System.Collections;

public class WeaponSelectionDialog : DialogBase
{
	private static string sceneName = string.Empty;
	public static void Popup(string name)
	{
		sceneName = name;
		if (UILayout.Instance != null)
			UILayout.Instance.BottomOut();
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/wuqixuanze_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/wuqixuanze");
#endif
	}

	public UIGrid allEquipmentsGrid, selectedEquipmentsGrid;
	public GameObject CloseWeaponItem;

	public GameObject itemPrefab;
	private Camera main;

	IEnumerator Start()
	{
//		DontDestroyOnLoad(gameObject);
		if (Camera.main != null)
		{
			main = Camera.main;
			Camera.main.enabled = false;
		}
		foreach (var i in System.Enum.GetValues(typeof(WeaponType)))
		{
			if ((WeaponType)i == WeaponType.gun_area
				|| (WeaponType)i == WeaponType.gun_single
				|| (WeaponType)i == WeaponType.gun_double
			    || (WeaponType)i == WeaponType.gun_jiatelin
			    || (WeaponType)i == WeaponType.bangqiugun
				|| (WeaponType)i == WeaponType.other_area) continue;
			var go = NGUITools.AddChild(allEquipmentsGrid.gameObject, itemPrefab);
			var item = (go as GameObject).GetComponent<WeaponSelectionItem>();
			item.WeaponType = (WeaponType)i;
		}
		allEquipmentsGrid.Reposition();
		yield return new WaitForEndOfFrame();
		OnSelectedWeaponsChanged();
	}

	void OnDestroy()
	{
		if (main != null)
			main.enabled = true;
	}

	public int SelectedGunWeaponCount()
	{
		return selectedEquipmentsGrid.transform.childCount;
	}

	public bool HasCloseWeaponSelected()
	{
//		foreach (var i in selectedEquipmentsGrid.GetComponentsInChildren<WeaponSelectionItem>())
//		{
//			if (i.IsCloseWeapon) return true;
//		}
//		return false;
		return CloseWeaponItem.GetComponent<WeaponSelectionItem>().icon.enabled;
	}

	public void OnSelectedWeaponsChanged()
	{
		// destroy current
		while (selectedEquipmentsGrid.transform.childCount > 0)
		{
			var t = selectedEquipmentsGrid.transform.GetChild(0);
			t.parent = null;
			Destroy(t.gameObject);
		}

		CloseWeaponItem.GetComponent<WeaponSelectionItem>().icon.enabled = false;
		CloseWeaponItem.GetComponent<WeaponSelectionItem>().selected = false;
		CloseWeaponItem.GetComponent<UISprite>().enabled = false;
		// rebuild selected
		var allItems = allEquipmentsGrid.GetComponentsInChildren<WeaponSelectionItem>();
		foreach (var i in allItems)
		{
			if (i.selected)
			{
				i.icon.color = new Color(0.3f, 0.3f, 0.3f);

				if (i.WeaponType < WeaponType.gun_area)
				{
					CloseWeaponItem.GetComponent<UISprite>().enabled = true;
					CloseWeaponItem.GetComponent<WeaponSelectionItem>().icon.enabled = true;
					CloseWeaponItem.GetComponent<WeaponSelectionItem>().icon.spriteName = i.icon.spriteName;
					CloseWeaponItem.GetComponent<WeaponSelectionItem>().parent = i;
					CloseWeaponItem.GetComponent<WeaponSelectionItem>().WeaponType = i.WeaponType;
					CloseWeaponItem.GetComponent<WeaponSelectionItem>().icon.color = Color.white;
					CloseWeaponItem.GetComponent<WeaponSelectionItem>().selected = true;
				}
				else
				{
					var go = Instantiate(i.gameObject) as GameObject;
					go.transform.parent = selectedEquipmentsGrid.transform;
					go.transform.localPosition = Vector3.zero;
					go.transform.localScale = Vector3.one;
					go.GetComponent<WeaponSelectionItem>().parent = i;
	//				go.collider.enabled = false;
					go.GetComponent<WeaponSelectionItem>().WeaponType = i.WeaponType;
					go.GetComponent<WeaponSelectionItem>().icon.color = Color.white;
				}
			}
		}
		selectedEquipmentsGrid.Reposition();

		// update data
		GameData.Instance.selectedWeapons.Clear();
		foreach (var i in selectedEquipmentsGrid.GetComponentsInChildren<WeaponSelectionItem>())
		{
			GameData.Instance.selectedWeapons.Add(i.WeaponType);
		}

		if (CloseWeaponItem.GetComponent<WeaponSelectionItem>().icon.enabled)
		{
			GameData.Instance.selectedWeapons.Add(CloseWeaponItem.GetComponent<WeaponSelectionItem>().WeaponType);
		}

	}

	public void OnEnterFight()
	{
		foreach(WeaponType item in GameData.Instance.selectedWeapons)
		{
			if (item == WeaponType.bangqiugun)
			{
				GameData.Instance.selectedWeapons.Remove(item);
				break;
			}
		}
		
		if (GameData.Instance.selectedWeapons.Count < 5)
		{
#if US_VERSION
			GameObject dlg = (GameObject)Instantiate(Resources.Load("prefabs/WarningPanel_us"));
#else
			GameObject dlg = (GameObject)Instantiate(Resources.Load("prefabs/WarningPanel"));
#endif
			dlg.transform.parent = DialogManager.Instance.transform;
			dlg.transform.localPosition = new Vector3(0, 80, 0);
			dlg.transform.localScale = Vector3.one;
			return;
		}

		StartCoroutine(OpneOtherDialog());
	}

	IEnumerator OpneOtherDialog()
	{
		yield return new WaitForSeconds(0.3f);
		OtherDialog.Popup(sceneName);
	}

}
