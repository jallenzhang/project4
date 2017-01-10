using UnityEngine;
using System.Collections;
using System;
using Mono.Data.Sqlite;

public class AvatarDB {
//	public static string appDBPath = Application.dataPath + "/avatar.db";
//	DbAccess db = new DbAccess("Data Source=" + appDBPath);
	#if UNITY_EDITOR || UNITY_STANDALONE
	public static string appDBPath = Application.dataPath + "/avatar.db";
	DbAccess db = new DbAccess("Data Source=" + appDBPath);
	#elif UNITY_IOS
	public static string appDBPath = Application.persistentDataPath + "/avatar.db";
	DbAccess db = new DbAccess("Data Source=" + appDBPath);
	#elif UNITY_ANDROID
	public static string appDBPath = Application.persistentDataPath + "/avatar.db";
	DbAccess db = new DbAccess("URI=file:" + appDBPath);
	#endif

	private static AvatarDB instance = null;

	public static AvatarDB Instance
	{
		get
		{
			if (instance == null)
				instance = new AvatarDB();

			return instance;
		}
	}

	public AvatarDB()
	{
		Init();
	}
	
	public void Init()
	{
		try
		{
			db.CreateTable("avatar", new string[]{"id", "currentLv"}, new string[]{"text", "text"});
			
		}
		catch(Exception ex)
		{
			
		}
	}
	
	public void UpdateAvatar(int id, int currentLv)
	{
		db.UpdateInto("avatar", new string[]{"currentLv"}, new string[]{currentLv.ToString()}, "id", id.ToString());
	}
	
	public int GetAvatarLvById(int id)
	{
		int ret = -1;
		SqliteDataReader sqReader = db.SelectWhere("avatar",new string[]{"currentLv"},new string[]{"id"},new string[]{"="},new string[]{id.ToString()});
		
		while (sqReader.Read())
		{
			string strCurrent = sqReader.GetString(sqReader.GetOrdinal("currentLv"));
			ret = int.Parse(strCurrent);
		}
		
		if (ret == -1)
		{
			db.InsertInto("avatar", new string[]{id.ToString(), "1"});
			ret = 1;
		}
		
		return ret;
		
	}
}
