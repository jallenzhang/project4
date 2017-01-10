using UnityEngine;
using System.Collections;
using System;
using Mono.Data.Sqlite;

public class PetDB {
	#if UNITY_EDITOR || UNITY_STANDALONE
	public static string appDBPath = Application.dataPath + "/pet.db";
	DbAccess db = new DbAccess("Data Source=" + appDBPath);
	#elif UNITY_IOS
	public static string appDBPath = Application.persistentDataPath + "/pet.db";
	DbAccess db = new DbAccess("Data Source=" + appDBPath);
	#elif UNITY_ANDROID
	public static string appDBPath = Application.persistentDataPath + "/pet.db";
	DbAccess db = new DbAccess("URI=file:" + appDBPath);
	#endif

	private static PetDB instance = null;
	public static PetDB Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new PetDB();
			}
			
			return instance;
		}
	}
	
	public PetDB()
	{
		Init();
	}
	
	public void Init()
	{
		try
		{
			db.CreateTable("pet", new string[]{"id", "currentLv","onBattle"}, new string[]{"text", "text","text"});
			
		}
		catch(Exception ex)
		{
			
		}
	}

	public void UpdatePet(int id, int currentLv, int onBattle)
	{
		SettingManager.Instance.MaxPetLevel = Mathf.Max(currentLv, SettingManager.Instance.MaxPetLevel);
		db.UpdateInto("pet", new string[]{"currentLv","onBattle"}, new string[]{currentLv.ToString(), onBattle.ToString()}, "id", id.ToString());
	}

	public int GetPetOnBattleById(int id)
	{
		int ret = -1;
		SqliteDataReader sqReader = db.SelectWhere("pet",new string[]{"currentLv","onBattle"},new string[]{"id"},new string[]{"="},new string[]{id.ToString()});
		while (sqReader.Read())
		{
			string strCurrent = sqReader.GetString(sqReader.GetOrdinal("onBattle"));
			ret = int.Parse(strCurrent);
		}
		if (ret == -1)
		{
			db.InsertInto("pet", new string[]{id.ToString(), "0", "0"});
			ret = 0;
		}
		return ret;
	}
	
	public int GetPetLvById(int id)
	{
		int ret = -1;
		SqliteDataReader sqReader = db.SelectWhere("pet",new string[]{"currentLv","onBattle"},new string[]{"id"},new string[]{"="},new string[]{id.ToString()});
		while (sqReader.Read())
		{
			string strCurrent = sqReader.GetString(sqReader.GetOrdinal("currentLv"));
			ret = int.Parse(strCurrent);
		}
		if (ret == -1)
		{
			db.InsertInto("pet", new string[]{id.ToString(), "0", "0"});
			ret = 0;
		}
		return ret;
		
	}
}
