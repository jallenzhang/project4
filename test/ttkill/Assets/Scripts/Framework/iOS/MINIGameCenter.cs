//#define US_VERSION
using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

public class MINIGameCenter : MonoBehaviour {
//	private string leaderBoardId =  "HS_LB";
	private static bool IsInitialized = false;

	void Awake()
	{
#if US_VERSION && UNITY_IPHONE
		if(!IsInitialized) {
			
			GameCenterManager.Dispatcher.addEventListener (GameCenterManager.GAME_CENTER_LEADERBOARD_SCORE_LOADED, OnLeaderboardScoreLoaded);
			
			
			
			//actions use example
			GameCenterManager.OnPlayerScoreLoaded += OnPlayerScoreLoaded;
			GameCenterManager.OnAuthFinished += OnAuthFinished;
			
			
			//Initializing Game Center class. This action will trigger authentication flow
			GameCenterManager.init();
			IsInitialized = true;
		}
#endif
	}

	public void ShowLeaderBoard()
	{
		GameCenterManager.ShowLeaderboards ();
	}

	public void ReportHighScore()
	{
		GameCenterManager.ReportScore(SettingManager.Instance.HighestScore, ConstData.LeaderBoardId);
	}

	private void OnLeaderboardScoreLoaded(CEvent e) {
		GK_PlayerScoreLoadedResult result = e.data as GK_PlayerScoreLoadedResult;
		
		if(result.IsSucceeded) {
			GK_Score score = result.loadedScore;
//			IOSNativePopUpManager.showMessage("Leaderboard " + score.leaderboardId, "Score: " + score.score + "\n" + "Rank:" + score.rank);
		}
		
	}

	private void OnPlayerScoreLoaded (GK_PlayerScoreLoadedResult result) {
		if(result.IsSucceeded) {
			GK_Score score = result.loadedScore;
//			IOSNativePopUpManager.showMessage("Leaderboard " + score.leaderboardId, "Score: " + score.score + "\n" + "Rank:" + score.rank);
			
//			Debug.Log("double score representation: " + score.GetDoubleScore());
//			Debug.Log("long score representation: " + score.GetLongScore());

			SettingManager.Instance.HighestScore = (int)Mathf.Max(score.GetLongScore(), SettingManager.Instance.HighestScore);
		}
	}

	void OnAuthFinished (ISN_Result res) {
		if (res.IsSucceeded) {
//			IOSNativePopUpManager.showMessage("Player Authed ", "ID: " + GameCenterManager.Player.Id + "\n" + "Alias: " + GameCenterManager.Player.Alias);
			GameCenterManager.LoadCurrentPlayerScore(ConstData.LeaderBoardId);
		} else {
//			IOSNativePopUpManager.showMessage("Game Center ", "Player authentication failed");
		}
	}
}
