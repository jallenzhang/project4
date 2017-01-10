//#define US_VERSION
using UnityEngine;
using System.Collections;

public class RankingListDialog : MonoBehaviour
{
	public static void Popup()
	{
		MainArea.Instance.gameObject.SetActive(false);
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/paiming_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/paiming");
#endif
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
