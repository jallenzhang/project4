using System.Collections;
using System.Collections.Generic;

public enum AnimState
{
	idle = 1,
	walk = 2,//近战走动
	attack = 4,//近战静止攻击
	skill = 8,
	take1,
	take2,
	dead,
	faint,
	run,
	attackgun,//持枪静止攻击
	attackgunwalk,//持枪走动攻击
	attackwalk,//近战走动攻击
	thinkAttack,
	staticAttack,//静止攻击
	daojuAttack,
}

public enum DAYOFSIGN
{
	Mondy = 1,
	Tuesday = 2,
	Wednesday = 4,
	Thursday = 8,
	Friday = 16,
	Saturday =32,
	Sunday = 64,
	All = 128,
}

public enum WeaponType
{
	bangqiugun = 1001,
	dao = 1002,
//	chuizi = 1003,
	dianju = 1004,

	gun_area = 2000,
	gun_shouqiang = 2001,
	gun_sandan = 2002,
	gun_liudan = 2003,
	gun_m4 = 2004,
	gun_fire = 2005,
	gun_jiatelin = 2006,

	gun_single = 3000,
	gundouble_shouqiang = 3001,
	gundouble_sandan = 3002,
	gundouble_liudan = 3003,
	gundouble_m4 = 3004,
	gundouble_fire = 3005,

	gun_double,

	other_area = 4000,

}

public enum PetType
{
	none = 0,
	songshu = 1,
	tuzi = 2,
	pet3,
	pet4,
	pet5,
	pet6,
	pet7,
}

public enum LevelType
{
	RushLevel,
	InfiniteLevel,
}

public class ConstData {
	//int: weaponType
	//string: bullet prefab path
	public static Dictionary<int, string> bulletDic = 
		new Dictionary<int, string>(){{(int)WeaponType.gun_fire, "prefabs/Bullets/Bullet1"},
									  {(int)WeaponType.gun_liudan, "prefabs/Bullets/Bullet1"},
									  {(int)WeaponType.gun_m4, "prefabs/Bullets/Bullet1"},
									  {(int)WeaponType.gun_sandan, "prefabs/Bullets/Bullet1"},
									  {(int)WeaponType.gun_shouqiang, "prefabs/Bullets/Bullet1"}};

	public static List<float> EnemyWaveTimes = new List<float>() {3f, 16f, 29.8f, 44.4f, 59.8f, 76f, 93f, 110.8f, 129.4f, 148.8f, 169f};
	public static int MaxWaves = 11;

	public static string LeaderBoardId =  "HS_LB";
}
