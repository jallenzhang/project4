using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TencentMTA : MTAPlatform {

	public override void Init ()
	{
		MtaService.SetInstallChannel("appstore");
		string mta_appkey = null;
		#if UNITY_IPHONE
		mta_appkey = "I364BILZG9YK";
		#elif UNITY_ANDROID
		mta_appkey = "ALBV8K8KK38E";
		#endif

		MtaService.StartStatServiceWithAppKey(mta_appkey);	
	}

	public override void DoEvent (string key)
	{
		Dictionary<string, string> dict = new Dictionary<string, string>();
		MtaService.TrackCustomKVEvent(key, dict);		
	}
}
