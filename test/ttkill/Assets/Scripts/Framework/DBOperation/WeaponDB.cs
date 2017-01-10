using UnityEngine;
using System.Collections;
using System;
using Mono.Data.Sqlite;

public class WeaponDB {
//	public static string appDBPath = Application.dataPath + "/weapon.db";
//	DbAccess db = new DbAccess("Data Source=" + appDBPath);
#if UNITY_EDITOR || UNITY_STANDALONE
	public static string appDBPath = Application.dataPath + "/weapon.db";
	DbAccess db = new DbAccess("Data Source=" + appDBPath);
#elif UNITY_IOS
	public static string appDBPath = Application.persistentDataPath + "/weapon.db";
	DbAccess db = new DbAccess("Data Source=" + appDBPath);
#elif UNITY_ANDROID
	public static string appDBPath = Application.persistentDataPath + "/weapon.db";
	DbAccess db = new DbAccess("URI=file:" + appDBPath);
#endif
	private static WeaponDB instance = null;
	public static WeaponDB Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new WeaponDB();
			}

			return instance;
		}
	}

	public WeaponDB()
	{
		Init();
	}

	public void Init()
	{
		try
		{
			db.CreateTable("weapon", new string[]{"id", "currentLv"}, new string[]{"text", "text"});
			
		}
		catch(Exception ex)
		{
			
		}
	}

	public void UpdateWeapon(int id, int currentLv)
	{
		db.UpdateInto("weapon", new string[]{"currentLv"}, new string[]{currentLv.ToString()}, "id", id.ToString());
	}

	public int GetWeaponLvById(int id)
	{
		int ret = -1;
		SqliteDataReader sqReader = db.SelectWhere("weapon",new string[]{"currentLv"},new string[]{"id"},new string[]{"="},new string[]{id.ToString()});

		while (sqReader.Read())
		{
			string strCurrent = sqReader.GetString(sqReader.GetOrdinal("currentLv"));
			ret = int.Parse(strCurrent);
		}


		if (ret == -1)
		{
			if (WeaponType.bangqiugun == (WeaponType)id 
			    || WeaponType.dao == (WeaponType)id 
			    || WeaponType.gun_shouqiang == (WeaponType)id 
			    || WeaponType.gun_liudan == (WeaponType)id 
			    || WeaponType.gun_m4 == (WeaponType)id 
			    || WeaponType.gun_sandan == (WeaponType)id)
			{
				db.InsertInto("weapon", new string[]{id.ToString(), "1"});
				ret = 1;
			}
			else
			{
				db.InsertInto("weapon", new string[]{id.ToString(), "0"});
				ret = 0;
			}
		}

		return ret;

	}


}
