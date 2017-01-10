using UnityEngine;
using System.Collections;

public enum GameMessageType
{
	MESSAGE_HERO_IDLE,
	MESSAGE_HERO_WALK,
	MESSAGE_HERO_ATTACK,
	MESSAGE_HERO_HURT,
	MESSAGE_HERO_DIE,


	MESSAGE_MONSTER_WALK,
	MESSAGE_MONSTER_ATTACK,
	MESSAGE_MONSTER_HURT,
	MESSAGE_MONSTER_DIE,
}

//如果进入一个新的状态，需要一些触发，比如NPC看到了Player，由巡逻状态进入跟踪状态
public enum Translate
{                        
	NullTrans,
	Translate_Hero_Attack,
	Translate_Hero_Idle,
	Translate_Hero_Walk,
	Translate_Hero_Die,

	Translate_Monster_Attack,
	Translate_Monster_Idle,
	Translate_Monster_Walk,
	Translate_Monster_Die,

	SeePlayer,
	LosePlayer
}

//每个状态都应该有一个ID,作为识别改状态的标志
public enum StateID
{ 
	NullState,
	Hero_Attack,
	Hero_Idle,
	Hero_Walk,
	Hero_Die,

	Monster_Attack,
	Monster_Idle,
	Monster_Walk,
	Monster_Die,

	Chaseingplayer,
	FollowPath
}