using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MtaU3DTest : MonoBehaviour {

	// 本样例只提供常用的接口，更详细的API见MtaService.cs或原生的开发者文档
	void Start () {
		// set配置接口
		// 开启debug，发布时请设置为false
		MtaService.SetDebugEnable(true);

		// 设置发布渠道，如果在androidManifest.xml配置，可不需要调用此接口
		MtaService.SetInstallChannel("play");
		// 设置上报策略，默认为APP_LAUNCH
		// MtaService.SetStatSendStrategy(MtaService.MTAStatReportStrategy.INSTANT);

		// 初始化，andriod可跳过此步骤
		// !!!!! 重要 !!!!!!!
		// MTA的appkey在android和ios系统上不同，请为根据不同平台设置不同appkey，否则统计结果可能会有问题。
		string mta_appkey = null;
#if UNITY_IPHONE
		mta_appkey = "Aqc123456";
#elif UNITY_ANDROID
		mta_appkey = "Iqc222222";
#endif


		MtaService.StartStatServiceWithAppKey(mta_appkey);	
		// 上报QQ号码
		// MtaService.reportQQ("123456");

		// 上报游戏用户，游戏高级模型需要用到
		MtaGameUser gameUser = new MtaGameUser("account1", "worldname1", "level1");
		MtaService.ReportGameUser(gameUser);

		// 根据业务实际情况，填充monitor对象的值
		MtaAppMonitor monitor = new MtaAppMonitor("download");
		monitor.RequestSize = 1000;
		monitor.ResponseSize = 304;
		monitor.MillisecondsConsume = 1000;
		monitor.ResultType = MtaAppMonitor.SUCCESS_RESULT_TYPE;
		monitor.ReturnCode = 0;
		monitor.Sampling = 1;
		// 上报接口监控数据
		MtaService.ReportAppMonitorStat(monitor);

		// 上报错误信息
		MtaService.ReportError("some error.");


		// 进入场景
		MtaService.TrackBeginPage("page1");
		// 退出场景
		MtaService.TrackEndPage("page1");

		// 构建自定义事件的key-value
		Dictionary<string, string> dict = new Dictionary<string, string>();
		dict.Add("account", "12345");
		dict.Add("amount", "100");
		dict.Add("item", "firearm");
		// 上报buy类型的自定义事件
		MtaService.TrackCustomKVEvent("buy", dict);		
		
		// 构建自定义事件的key-value
		Dictionary<string, string> beDict = new Dictionary<string, string>();
		beDict.Add("account", "12345");
		beDict.Add("level", "8");
		beDict.Add("name", "model");
		// 通关前
		MtaService.TrackCustomBeginKVEvent("mission", beDict);
		// 通关ing...
		// 通关后
		MtaService.TrackCustomEndKVEvent("mission", beDict);

		// 获取在线配置，key为前台配置的在线配置信息
		Debug.Log("getCustomProperty=" + MtaService.GetCustomProperty("key"));

#if UNITY_ANDROID
		// 监控www.qq.com域名
		Dictionary<string, int> speedMap = new Dictionary<string, int>();
		speedMap.Add("www.qq.com", 80);
		MtaService.TestSpeed(speedMap);

		// 获取MID
		string mid = MtaService.GetMid();
		Debug.Log("mid is " + mid);
#endif



	}

	void testMtaAPI(){

		// 设置appkey，如果在androidManifest.xml配置，可不需要调用此接口
		// MtaService.SetAppKey("Aqc1222");
		//MtaService.CommitEvents(-1);
		//MtaService.StartNewSession();
		//MtaService.StopSession();

		MtaService.SetAutoExceptionCaught(true);
		MtaService.SetCustomUserId("user1");
		MtaService.SetEnableSmartReporting(false);
		MtaService.SetMaxBatchReportCount(10);

		MtaService.SetMaxParallelTimmingEvents(1000);
		MtaService.SetMaxSessionStatReportCount(10);
		MtaService.SetMaxSendRetryCount(10);
		MtaService.SetMaxStoreEventCount(10000);
		MtaService.SetSendPeriodMinutes(100);
		MtaService.SetSessionTimoutMillis(60*1000);

#if UNITY_ANDROID
		MtaService.SetMaxDaySessionNumbers(10);

		Debug.Log("getAppKey=" + MtaService.GetAppKey());
		Debug.Log("getInstallChannel=" + MtaService.GetInstallChannel());
		Debug.Log("getCustomUserId=" + MtaService.GetCustomUserId());
#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
