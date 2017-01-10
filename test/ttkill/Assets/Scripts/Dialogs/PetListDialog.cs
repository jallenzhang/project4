//#define US_VERSION
using UnityEngine;
using System.Collections;

public class PetListDialog : DialogBase {
	float originalCameraDepth = 0;
	Camera camera;
	public static bool isLeftPet = true;
	public static PetType oldType;
	public static void Popup(bool left, PetType type)
	{
		isLeftPet = left;
		oldType = type;
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/PetListDialog_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/PetListDialog");
#endif
	}

	void Start()
	{
		camera = NGUITools.FindCameraForLayer(gameObject.layer);
		originalCameraDepth = camera.depth;
		camera.depth = 88;
	}

	public void CloseDialog()
	{
		DialogManager.Instance.CloseDialog();
	}

	void OnDestroy()
	{
		camera.depth = originalCameraDepth;
	}

	public void onBattleChange()
	{
		if (isLeftPet)
		{
			EventService.Instance.GetEvent<PetOnLeftBattleEvent>().Publish(oldType);//
		}
		else
		{
			EventService.Instance.GetEvent<PetOnRightBattleEvent>().Publish(oldType);//
		}
	}
}
