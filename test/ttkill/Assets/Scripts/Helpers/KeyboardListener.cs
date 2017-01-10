//#define US_VERSION
using UnityEngine;
using System.Collections;

public class KeyboardListener : MonoBehaviour {
	
	public static bool EscPressed = false;
	
	private const string CHANGJING = "changjing";
	private const string WUJIN = "wujin";
	private const string UI = "ui";
	// Use this for initialization
	void Start () {
		EscPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!EscPressed && Input.GetKeyUp(KeyCode.Escape))
		{
			
			string currentLevelName = Application.loadedLevelName;
			
			if (currentLevelName.Contains(CHANGJING) || currentLevelName.Contains(WUJIN) )
			{
				Time.timeScale = 0;
				GameData.Instance.Pause = true;
//				DialogManager.Instance.PopupDialog(CommonAsset.Load("prefabs/Dialogs/PauseGameDialog"));
				PauseGameDialog.Popup();
			}
			else if(currentLevelName.Contains(UI))
			{
				Ultilities.CleanMemory();
#if US_VERSION
				Application.LoadLevel("login_us");
#else
				Application.LoadLevel("login");
#endif
			}
			else if (currentLevelName.Contains("chuanggan"))
			{
				Ultilities.CleanMemory();
#if US_VERSION
				Application.LoadLevel("ui_us");
#else
				Application.LoadLevel("ui");
#endif
			}
			else if (currentLevelName.Contains("login"))
			{
				ExitGameDialog.Popup();
			}

			
			EscPressed = true;
		}
	}

}
