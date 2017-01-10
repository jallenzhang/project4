using UnityEngine;
using System.Collections;

public class BattleSceneSingleton : MonoBehaviour {
	private static BattleSceneSingleton instance = null;
	public static BattleSceneSingleton Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (BattleSceneSingleton)GameObject.FindObjectOfType(typeof(BattleSceneSingleton));
				if (instance == null)
					Debug.LogError("can't find battleSceneSingleton component in this scene!");
			}

			return instance;
		}
	}

//	private GameObject player;
	// Use this for initialization
	void Start () {
//		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SendGameMessage<T>(GameMessageType type, T t)
	{
		switch(type)
		{
		case GameMessageType.MESSAGE_HERO_ATTACK:

			break;
		case GameMessageType.MESSAGE_HERO_DIE:
			break;
		case GameMessageType.MESSAGE_HERO_HURT:
			break;
		case GameMessageType.MESSAGE_HERO_IDLE:
			break;
		case GameMessageType.MESSAGE_HERO_WALK:
			break;
		case GameMessageType.MESSAGE_MONSTER_ATTACK:

			break;
		case GameMessageType.MESSAGE_MONSTER_DIE:
			break;
		case GameMessageType.MESSAGE_MONSTER_HURT:
			break;
		case GameMessageType.MESSAGE_MONSTER_WALK:
			break;
		}
	}
}
