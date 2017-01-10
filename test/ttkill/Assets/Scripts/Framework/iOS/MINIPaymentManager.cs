using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

public class MINIPaymentManager {

	public const string ONE_TIER 	=  "D60";
	public const string FIVE_TIER 	=  "D320";
	public const string TEN_TIER = "D750";
	public const string TWENTU_TIER = "D1700";
	public const string THIRTY_TIER = "D3000";
	public const string FIFTY_TIER = "D5400";

	private static bool IsInitialized = false;
	public static void init() {
		
		
		if(!IsInitialized) {
			
			//You do not have to add products by code if you already did it in seetings guid
			//Windows -> IOS Native -> Edit Settings
			//Billing tab.
			IOSInAppPurchaseManager.Instance.addProductId(ONE_TIER);
			IOSInAppPurchaseManager.Instance.addProductId(FIVE_TIER);
			IOSInAppPurchaseManager.Instance.addProductId(TEN_TIER);
			IOSInAppPurchaseManager.Instance.addProductId(TWENTU_TIER);
			IOSInAppPurchaseManager.Instance.addProductId(THIRTY_TIER);
			IOSInAppPurchaseManager.Instance.addProductId(FIFTY_TIER);
			
			
			
			//Event Use Examples
			IOSInAppPurchaseManager.Instance.addEventListener(IOSInAppPurchaseManager.VERIFICATION_RESPONSE, onVerificationResponse);
			
			IOSInAppPurchaseManager.Instance.OnStoreKitInitComplete += OnStoreKitInitComplete;
			
			
			//Action Use Examples
			IOSInAppPurchaseManager.Instance.OnTransactionComplete += OnTransactionComplete;
			IOSInAppPurchaseManager.Instance.OnRestoreComplete += OnRestoreComplete;
			
			
			IsInitialized = true;
			
		} 
		
		IOSInAppPurchaseManager.Instance.loadStore();
		
		
	}
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public static void buyItem(string productId) {
		IOSInAppPurchaseManager.Instance.buyProduct(productId);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	
	private static void UnlockProducts(string productIdentifier) {
		switch(productIdentifier) {
		case ONE_TIER:
			GameData.Instance.AddDiamond(60);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DIAMOND_TYPE_1);
			break;
		case FIVE_TIER:
			GameData.Instance.AddDiamond(320);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DIAMOND_TYPE_2);
			break;
		case TEN_TIER:
			GameData.Instance.AddDiamond(750);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DIAMOND_TYPE_3);
			break;
		case TWENTU_TIER:
			GameData.Instance.AddDiamond(1700);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DIAMOND_TYPE_4);
			break;
		case THIRTY_TIER:
			GameData.Instance.AddDiamond(3000);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DIAMOND_TYPE_5);
			break;
		case FIFTY_TIER:
			GameData.Instance.AddDiamond(5400);
			MTAManager.DoEvent(MTAPoint.MINI_BUY_DIAMOND_TYPE_6);
			break;
			
		}
	}
	
	private static void OnTransactionComplete (IOSStoreKitResponse response) {
		
		Debug.Log("OnTransactionComplete: " + response.productIdentifier);
		Debug.Log("OnTransactionComplete: state: " + response.state);

		LoadingDialog.CloseDialog();

		switch(response.state) {
		case InAppPurchaseState.Purchased:
		case InAppPurchaseState.Restored:
			//Our product been succsesly purchased or restored
			//So we need to provide content to our user depends on productIdentifier
			UnlockProducts(response.productIdentifier);
			break;
		case InAppPurchaseState.Deferred:
			//iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
			//You should update your UI to reflect this deferred state, and expect another Transaction Complete  to be called again with a new transaction state 
			//reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
			break;
		case InAppPurchaseState.Failed:
			//Our purchase flow is failed.
			//We can unlock intrefase and repor user that the purchase is failed. 
			Debug.Log("Transaction failed with error, code: " + response.error.code);
			Debug.Log("Transaction failed with error, description: " + response.error.description);
			
			
			break;
		}
		
		if(response.state == InAppPurchaseState.Failed) {
			IOSNativePopUpManager.showMessage("Transaction Failed", "Error code: " + response.error.code + "\n" + "Error description:" + response.error.description);
		} else {
//			IOSNativePopUpManager.showMessage("Store Kit Response", "product " + response.productIdentifier + " state: " + response.state.ToString());
		}
		
	}
	
	
	private static void OnRestoreComplete (IOSStoreKitRestoreResponce res) {
		if(res.IsSucceeded) {
			IOSNativePopUpManager.showMessage("Success", "Restore Compleated");
		} else {
			IOSNativePopUpManager.showMessage("Error: " + res.error.code, res.error.description);
		}
	}	
	
	
	private static void onVerificationResponse(CEvent e) {
		IOSStoreKitVerificationResponse response =  e.data as IOSStoreKitVerificationResponse;
		
		IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + response.status.ToString());
		
		Debug.Log("ORIGINAL JSON: " + response.originalJSON);
	}
	
	private static void OnStoreKitInitComplete(ISN_Result result) {
		if(result.IsSucceeded) {
			Debug.Log("StoreKit Init Succeeded, Available products count: " + IOSInAppPurchaseManager.instance.products.Count.ToString());
//			IOSNativePopUpManager.showMessage("StoreKit Init Succeeded", "Available products count: " + IOSInAppPurchaseManager.instance.products.Count.ToString());
		} else {
			Debug.Log("StoreKit Init Failed");
//			IOSNativePopUpManager.showMessage("StoreKit Init Failed",  "Error code: " + result.error.code + "\n" + "Error description:" + result.error.description);
		}
	}
}
