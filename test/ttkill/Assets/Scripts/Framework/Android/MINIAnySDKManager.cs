using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using anysdk;
using System;

public class MINIAnySDKManager : MonoBehaviour {
	static string appKey = "6CE5F76D-9E20-E331-93F4-2F2298A2E244";
	static string appSecret = "a444e34c5f6ae6f24610e92d1068d159";
	static string privateKey = "FA50CA6056DEED6316E9161A15EAFD10";
	static string oauthLoginServer = "http://oauth.anysdk.com/api/OauthLoginDemo/Login.php";
	// Use this for initialization

	public static string B20 = "B20";
	public static string B45 = "B45";
	public static string B96 = "B96";
	public static string B155 = "B155";
	public static string B400 = "B400";
	public static string B1000 = "B1000";

	private static Dictionary<string, string> priceNameDic = new Dictionary<string, string>(){
		{B20, "花费2元购买20钻石"},
		{B45, "花费4元购买45钻石"},
		{B96, "花费8元购买96钻石"},
		{B155, "花费12元购买155钻石"},
		{B400, "花费30元购买400钻石"},
		{B1000, "花费60元购买1000钻石"}};

	private static Dictionary<string, string> priceDic = new Dictionary<string, string>(){
		{B20, "2"},
		{B45, "4"},
		{B96, "8"},
		{B155, "12"},
		{B400, "30"},
		{B1000, "60"}};

	private static bool IsInitialized = false;

	private static MINIAnySDKManager manager = null;

	public static void Init()
	{
		if(!IsInitialized) {
			GameObject obj = new GameObject();
			manager = obj.AddComponent<MINIAnySDKManager>();
//			manager = new MINIAnySDKManager();
			AnySDK.getInstance ().init (appKey, appSecret, privateKey, oauthLoginServer);
			AnySDKUser.getInstance () .setListener (manager,"UserExternalCall");
			AnySDKIAP.getInstance ().setListener (manager,"IAPExternalCall");
			IsInitialized = true;
		}
	}

	void Start()
	{
		GameObject.DontDestroyOnLoad(gameObject);
	}

	void UserExternalCall(string msg)
	{
		Debug.Log("UserExternalCall("+ msg+")");
		Dictionary<string,string> dic = AnySDKUtil.stringToDictionary (msg);
		int code = Convert.ToInt32(dic["code"]);
		string result = dic["msg"];
		switch(code)
		{
		case (int)UserActionResultCode.kInitSuccess://初始化SDK成功回调
			AnySDKUser.getInstance().login();
			break;
		case (int)UserActionResultCode.kInitFail://初始化SDK失败回调
			break;
		case (int)UserActionResultCode.kLoginSuccess://登陆成功回调
			AnySDKParam param = new AnySDKParam((int)ToolBarPlace.kToolBarBottomLeft);
			if( AnySDKUser.getInstance ().isFunctionSupported( "showToolBar" ) ) {
				AnySDKUser.getInstance ().callFuncWithParam( "showToolBar", param );
			}

			if( AnySDKUser.getInstance ().isFunctionSupported( "enterPlatform" ) ) {
				AnySDKUser.getInstance ().callFuncWithParam( "enterPlatform" );
			}
			break;
		case (int)UserActionResultCode.kLoginNetworkError://登陆失败回调
		case (int)UserActionResultCode.kLoginCancel://登陆取消回调
		case (int)UserActionResultCode.kLoginFail://登陆失败回调
			break;
		case (int)UserActionResultCode.kLogoutSuccess://登出成功回调
			break;
		case (int)UserActionResultCode.kLogoutFail://登出失败回调
			break;
		case (int)UserActionResultCode.kPlatformEnter://平台中心进入回调

			break;
		case (int)UserActionResultCode.kPlatformBack://平台中心退出回调
			break;
		case (int)UserActionResultCode.kPausePage://暂停界面回调
			break;
		case (int)UserActionResultCode.kExitPage://退出游戏回调
			break;
		case (int)UserActionResultCode.kAntiAddictionQuery://防沉迷查询回调
			break;
		case (int)UserActionResultCode.kRealNameRegister://实名注册回调
			break;
		case (int)UserActionResultCode.kAccountSwitchSuccess://切换账号成功回调
			break;
		case (int)UserActionResultCode.kAccountSwitchFail://切换账号成功回调
			break;
		case (int)UserActionResultCode.kOpenShop://应用汇  悬浮窗点击粮饷按钮回调
			break;
		default:
			break;
		}
	}

	void IAPExternalCall(string msg)
	{
		Debug.Log("IAPExternalCall("+ msg+")");
		Dictionary<string,string> dic = AnySDKUtil.stringToDictionary (msg);
		int code = Convert.ToInt32(dic["code"]);
		string result = dic["msg"];
		
		switch(code)
		{
		case (int)PayResultCode.kPaySuccess://支付成功回调
			LoadingDialog.CloseDialog();
			break;
		case (int)PayResultCode.kPayFail://支付失败回调
			LoadingDialog.CloseDialog();
			break;
		case (int)PayResultCode.kPayCancel://支付取消回调
			LoadingDialog.CloseDialog();
			break;
		case (int)PayResultCode.kPayNetworkError://支付超时回调
			LoadingDialog.CloseDialog();
			break;
		case (int)PayResultCode.kPayProductionInforIncomplete://支付信息不完整
			LoadingDialog.CloseDialog();
			break;
			/**
        * 新增加:正在进行中回调
        * 支付过程中若SDK没有回调结果，就认为支付正在进行中
        * 游戏开发商可让玩家去判断是否需要等待，若不等待则进行下一次的支付
        */
		case (int)PayResultCode.kPayNowPaying:
			break;
		default:
			break;
		}
	}

	public static void BuyItem(string productId)
	{
		Dictionary<string,string> mProductionInfo = new Dictionary<string, string>();
		mProductionInfo.Add("Product_Id",productId);
		mProductionInfo.Add("Product_Name",priceNameDic[productId]);
		mProductionInfo.Add("Product_Price",priceDic[productId]);
		mProductionInfo.Add("Product_Count","1");
		mProductionInfo.Add("Role_Id",SettingManager.Instance.CurrentAvatar.ToString());
		mProductionInfo.Add("Role_Name","MiniGun");
		mProductionInfo.Add("Role_Grade",SettingManager.Instance.MaxAvatarLevel.ToString());
		mProductionInfo.Add("Role_Balance",SettingManager.Instance.TotalDiamond.ToString());
		mProductionInfo.Add("Server_Id","1");
		List<string> idArrayList =  AnySDKIAP.getInstance().getPluginId();
		if (idArrayList.Count == 1) {
			AnySDKIAP.getInstance().payForProduct(mProductionInfo,idArrayList[0]);
		}
		else //多种支付方式
		{
			//开发者需要自己设计多支付方式的逻辑及UI,Sample中有示例
		}
	}
}
