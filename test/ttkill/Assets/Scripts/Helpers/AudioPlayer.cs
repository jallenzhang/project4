using UnityEngine;
using System.Collections;

/// <summary> ##################################
/// 
/// NOTICE :
/// This script is for the audio fx and bgm~!
/// You can add more Audioclips as well as modify the enums as you see fit.
/// 
/// Note that this is just where all the clips are stored. There are various places in other scripts
/// that will reference this script to play the appropriate audio using the defined enum.
/// 
/// </summary> ##################################

public class AudioPlayer : MonoBehaviour {
	
	// the audio source is a gameObject so that you can move it around for various effect.
	// the default listener is attached to the "AudioSource" GameObject.
	public AudioSource player;
	public AudioSource bgmPlayer;
	
	public bool enableMusic = true;
	public AudioClip[] bgm;
	
	public bool enableSoundFX = true;
	public customAudio gameOverSoundFX;
	public customAudio shouqiangSoundFX;
	public customAudio jiqiangSoundFX;
	public customAudio liudanBoomFX;
	public customAudio liudanFX;
	public customAudio bulletHitWallFX;
	public customAudio coinGotFX;
	public customAudio fireFX;
	public customAudio sandanFX;
	public customAudio weaponChangeFX;
	public customAudio bangqiugunFX;
	public customAudio bangqiugunHitFX;
	public customAudio daoFX;
	public customAudio daoHitFX;
	public customAudio chuiziFX;
	public customAudio chuiziHitFx;
	public customAudio dianjuFX;
	public customAudio dianjuHitFX;
	public customAudio fuFX;
	public customAudio recoverFX;

	// created a custom class to store a bool as a reference,
	// and to simulate a cooldown function with "x" seconds.
	[System.Serializable]
	public class customAudio{
		public AudioClip audioClip;
		bool canPlay = true;
		
		public void play(){
			if(audioClip != null && canPlay && Ultilities.gm.audioScript.enableSoundFX){
				Ultilities.gm.audioScript.player.PlayOneShot(audioClip);
				Ultilities.gm.audioScript.StartCoroutine(coolDown(0.1f)); // built-in spam filter
			}
		}
		
		// causes a delayed state transition
		IEnumerator coolDown(float timer){
			canPlay = !canPlay; // reverse the state
			yield return new WaitForSeconds(timer);
			canPlay = !canPlay; // back to original
		}
	}


	void Start()
	{

	}
	// function to play the bgm if enabled
	void loadBGM(){
		if(bgmPlayer == null || bgm.Length == 0){
			return; // null player!
		}
		bgmPlayer.audio.clip = bgm[Random.Range(0,bgm.Length)]; // set the clip
		if(enableMusic && bgmPlayer.audio.clip != null){ // if music is enabled
			bgmPlayer.audio.Play(); // play
		}
	}
	
	IEnumerator playNextBGM(){
		while(true){
			if(enableMusic && !bgmPlayer.audio.isPlaying){
				loadBGM();
			}
			yield return new WaitForSeconds(2f);
		}
	}
	
	// function to toggle the bgm on/off
	public void toggleBGM(){
		if(bgmPlayer.audio.clip != null && bgmPlayer.audio.isPlaying){ // if music is playing
			bgmPlayer.audio.Pause(); // pause the music
			enableMusic = false;
		} else {
			bgmPlayer.audio.Play(); // play
			enableMusic = true;
		}
	}
	
	// function to toggle the FX on/off
	public void toggleFX(){
		enableSoundFX = !enableSoundFX;
	}
	
	void Awake(){
		if(player == null){ // try and get it manually if player forgot to assign an AudioSource
			player = GameObject.Find("AudioSource").GetComponent<AudioSource>();
		}
		if(bgmPlayer == null && player != null){
			bgmPlayer = player;
		}
		
		if(player == null){
			Debug.LogError("audio source reference is null. Fix the AudioPlayer script reference !");
		}
		if(bgmPlayer == null){
			Debug.LogError("bgm audio source reference is null. Fix the AudioPlayer script reference !");
		}

		enableMusic = SettingManager.Instance.Music != 0;
		enableSoundFX = SettingManager.Instance.Sound != 0;

		StartCoroutine(playNextBGM() );
	}
}
