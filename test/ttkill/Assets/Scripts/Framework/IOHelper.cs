//#define US_VERSION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IOHelper {
//	private static string pre_path = Application.dataPath +"/Raw/";//+"/avatar_upgrade.txt";
#if UNITY_EDITOR || UNITY_STANDALONE
	#if US_VERSION
	private static string pre_path = Application.dataPath +"/StreamingAssets/US/";
	#else
	private static string pre_path = Application.dataPath +"/StreamingAssets/";
	#endif

#elif UNITY_IOS
	#if US_VERSION
	static string pre_path = Application.dataPath +"/Raw/US/";//+"/avatar_upgrade.txt";
	#else
	static string pre_path = Application.dataPath +"/Raw/";//+"/avatar_upgrade.txt";
	#endif
#elif UNITY_ANDROID
	static string pre_path = "jar:file://" + Application.dataPath + "!/assets/";//+"avatar_upgrade.txt;
#endif

	private static string avatar_upgrade_name = "avatar_upgrade.txt";
	private static string avatar_name = "avatar.txt";
	private static string gun_upgrade_name = "gun_upgrade.txt";
	private static string gun_name = "gun.txt";
	private static string monster_name = "monster.txt";
	private static string task_name = "task.txt";
	private static string fu_name = "magic.txt";
	private static string stage_name = "stage.txt";
	private static string stage_infinite = "stage_infinite.txt";
	private static string pet_name = "pet.txt";
	private static string item_name = "item.txt";

	private static List<AvatarUpgradeInfo> avaterUpgradeInfos = new List<AvatarUpgradeInfo>();
	private static List<AvatarInfo> avatarInfos = new List<AvatarInfo>();
	private static List<GunUpgradeInfo> gunUpgradeInfos = new List<GunUpgradeInfo>();
	private static List<GunInfo> gunInfos = new List<GunInfo>();
	private static List<MonsterInfo> monsterInfos = new List<MonsterInfo>();
	private static List<TaskInfo> taskInfos;
	private static List<FuInfo> fuInfos;
	private static List<StageInfo> stageInfos = new List<StageInfo>();
	private static List<StageInfiniteInfo> stageInfiniteInfos = new List<StageInfiniteInfo>();
	private static List<PetInfo> petInfos = new List<PetInfo>();
	private static List<ItemInfo> itemInfos = new List<ItemInfo>();

	#region itemInfo
	public static List<ItemInfo> GetItemInfos()
	{
		if (itemInfos != null && itemInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path, item_name);
			itemInfos = JsonHelper.DeSerialize<List<ItemInfo>>(content);
		}

		return itemInfos;
	}

	public static ItemInfo GetItemInfoById(int id)
	{
		if (id < 0)
			return null;

		GetItemInfos();

		foreach(ItemInfo info in itemInfos)
		{
			if (info.id == id)
				return info;
		}

		return null;
	}
	#endregion

	#region petinfo
	public static List<PetInfo> GetPetInfos()
	{
		if (petInfos != null && petInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path, pet_name);
			petInfos = JsonHelper.DeSerialize<List<PetInfo>>(content);
		}

		return petInfos;
	}

	public static PetInfo GetPetInfoByIdAndLevel(int id, int level)
	{
		if (id < 0)
			return null;
		
		GetPetInfos();
		
		foreach(PetInfo info in petInfos)
		{
			if (info.id == id && info.lv == level)
				return info;
		}
		
		return null;
	}
	#endregion

	#region stageinfo
	public static List<StageInfiniteInfo> GetStageInfiniteInfos()
	{
		if (stageInfiniteInfos != null && stageInfiniteInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path, stage_infinite);
			stageInfiniteInfos = JsonHelper.DeSerialize<List<StageInfiniteInfo>>(content);
		}

		return stageInfiniteInfos;
	}

	public static StageInfiniteInfo GetStageInfiniteInfoByStageId(int stageId)
	{
		if (stageId < 0)
			return null;

		GetStageInfiniteInfos();

		foreach(StageInfiniteInfo info in stageInfiniteInfos)
		{
			if (info.stageid == stageId)
			{
				return info;
			}
		}

		return null;
	}

	public static List<StageInfo> GetStageInfos()
	{
		if (stageInfos != null && stageInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path, stage_name);
			stageInfos = JsonHelper.DeSerialize<List<StageInfo>>(content);
		}

		return stageInfos;
	}

	public static StageInfo GetStageInfoByStageId(int stageId)
	{
		if (stageId < 0)
			return null;

		GetStageInfos();

		foreach(StageInfo info in stageInfos)
		{
			if (info.stage_id == stageId)
			{
				return info;
			}
		}

		return null;
	}
	#endregion

	#region AvatarUpgradeInfo
	public static List<AvatarUpgradeInfo> GetAvaterUpgradeInfos()
	{
		if (avaterUpgradeInfos !=null && avaterUpgradeInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path, avatar_upgrade_name);
			avaterUpgradeInfos = JsonHelper.DeSerialize<List<AvatarUpgradeInfo>>(content);
		}

		return avaterUpgradeInfos;
	}

	public static AvatarUpgradeInfo GetAvaterUpgradeInfoByIdAndLevel(int id, int level)
	{
		if (id < 0)
			return null;

		GetAvaterUpgradeInfos();

		foreach(AvatarUpgradeInfo info in avaterUpgradeInfos)
		{
			if (info.id == id && info.lv == level)
				return info;
		}

		return null;
	}
	#endregion

	#region AvatarInfo
	public static List<AvatarInfo> GetAvatarInfos()
	{
		if (avatarInfos != null && avatarInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path,avatar_name);
			avatarInfos = JsonHelper.DeSerialize<List<AvatarInfo>>(content);
		}

		return avatarInfos;
	}

	public static AvatarInfo GetAvatarInfoById(int id)
	{
		if (id < 0 )
			return null;

		GetAvatarInfos();

		foreach(AvatarInfo info in avatarInfos)
		{
			if (info.id == id)
				return info;
		}

		return null;
	}

	#endregion

	#region gun upgrade infos
	public static List<GunUpgradeInfo> GetGunUpgradeInfos()
	{
		if (gunUpgradeInfos != null && gunUpgradeInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path, gun_upgrade_name);
			gunUpgradeInfos = JsonHelper.DeSerialize<List<GunUpgradeInfo>>(content);
		}

		return gunUpgradeInfos;
	}

	public static GunUpgradeInfo GetGunUpgradeInfoByIdAndLevel(int id, int level)
	{
		if (id < 0 || level < 0)
			return null;

		GetGunUpgradeInfos();

		foreach(GunUpgradeInfo info in gunUpgradeInfos)
		{
			if (info.id == id && info.lv == level)
				return info;
		}

		return null;
	}
	#endregion

	#region gun info
	public static List<GunInfo> GetGunInfos()
	{
		if (gunInfos != null && gunInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path, gun_name);
			gunInfos = JsonHelper.DeSerialize<List<GunInfo>>(content);
		}

		return gunInfos;
	}

	public static GunInfo GetGunInfoById(int id)
	{
		if (id < 0)
			return null;
		GetGunInfos();
		foreach (GunInfo info in gunInfos)
		{
			if (info.id == id)
				return info;
		}

		return null;
	}
	#endregion

	#region monster info
	public static List<MonsterInfo> GetMonsterInfos()
	{
		if (monsterInfos != null && monsterInfos.Count == 0)
		{
			string content = FileHelper.LoadFile(pre_path, monster_name);
			monsterInfos = JsonHelper.DeSerialize<List<MonsterInfo>>(content);
		}

		return monsterInfos;
	}

	public static MonsterInfo GetMonsterInfo(int id)
	{
		if (id < 0)
			return null;

		GetMonsterInfos();

		foreach (MonsterInfo info in monsterInfos)
		{
			if (info.id == id)
				return info;
		}

		return null;
	}
	#endregion

	#region task info
	public static List<TaskInfo> GetTaskInfos()
	{
		if (taskInfos == null)
		{
			string content = FileHelper.LoadFile(pre_path, task_name);
			if (!string.IsNullOrEmpty(content))
			{
				taskInfos = JsonHelper.DeSerialize<List<TaskInfo>>(content);
			}
		}

		if (taskInfos == null) taskInfos = new List<TaskInfo>();

		return taskInfos;
	}

	public static TaskInfo GetTaskInfo(int id)
	{
		return GetTaskInfos().Find((e) => e.id == id);
	}
	#endregion

	#region fu info
	static int GetFuId(FuItem.Type type)
	{
		int magic_id = 0;
		if (type == FuItem.Type.Speed) magic_id = 1;
		else if (type == FuItem.Type.Restore) magic_id = 2;
		else if (type == FuItem.Type.Call) magic_id = 4;
		else if (type == FuItem.Type.Fury) magic_id = 3;
		return magic_id;
	}

	public static List<FuInfo> GetFuInfos()
	{
		if (fuInfos == null)
		{
			string content = FileHelper.LoadFile(pre_path, fu_name);
			if (!string.IsNullOrEmpty(content))
			{
				fuInfos = JsonHelper.DeSerialize<List<FuInfo>>(content);
			}
		}

		if (fuInfos == null) fuInfos = new List<FuInfo>();

		return fuInfos;
	}

	public static FuInfo GetFuInfo(FuItem.Type type, int lv)
	{
		int magic_id = GetFuId(type);
		foreach (var e in GetFuInfos())
		{
			if (e.magic_id == magic_id && e.lv == lv) return e;
		}
		return null;
	}

	public static int GetFuMaxLv(FuItem.Type type)
	{
		int magic_id = GetFuId(type);
		int lv = 0;
		foreach (var e in GetFuInfos())
		{
			if (e.magic_id == magic_id && e.lv > lv) lv = e.lv;
		}
		return lv;
	}

	public static int GetCurrentFuLv(FuItem.Type type)
	{
		return PlayerPrefs.GetInt("key_" + type.ToString(), 1);
	}

	public static void SetCurrentFuLv(FuItem.Type type, int lv)
	{
		PlayerPrefs.SetInt("key_" + type.ToString(), lv);
		PlayerPrefs.Save();
	}
	#endregion
}
