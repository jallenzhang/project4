using UnityEngine;
using System.Collections;

public class FuTrigger : MonoBehaviour {
	public FuType type;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
			//hit the new weapon and change weapon	
			EventService.Instance.GetEvent<FuEvent>().Publish(type);
			gameObject.collider.enabled = false;
			Ultilities.gm.audioScript.fuFX.play();
			SettingManager.Instance.FuGot_Shuaxin += 1;
			Destroy(gameObject);//.transform.parent.gameObject);
		}
	}
}
