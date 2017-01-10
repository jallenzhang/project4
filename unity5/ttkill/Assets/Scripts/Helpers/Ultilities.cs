using UnityEngine;
using System.Collections;

public class Ultilities {
	public static GameManager gm;

	public static float AngleAroundAxis (Vector3 dirA, Vector3 dirB, Vector3 axis) {
		// Project A and B onto the plane orthogonal target axis
		dirA = dirA - Vector3.Project (dirA, axis);
		dirB = dirB - Vector3.Project (dirB, axis);
		// Find (positive) angle between A and B
		float angle = Vector3.Angle (dirA, dirB);
		
		// Return angle multiplied with 1 or -1
		return angle * (Vector3.Dot (axis, Vector3.Cross (dirA, dirB)) < 0 ? -1 : 1);
	}

	public static int TotalEnemiesInLevel()
	{
		int ret = 0;
		for (int i = 1; i <= ConstData.MaxWaves; i++)
		{
			ret += Mathf.FloorToInt(15 + (GameData.Instance.CurrentWave - 1) * 1.5f + GameData.Instance.CurrentLevel / 12);
		}

		return ret;
	}

	/// <summary>
	/// 将时间“00:00:00”转化为单位为秒的数值
	/// </summary>
	/// <param name="time">形式为"00:00:00"的时间字符串</param>
	/// <returns>秒数</returns>
	public static int ConvertTimeToSecond(string time)
	{
		string[] strSplits = time.Split(':');
		if (strSplits.Length != 3)
			return -1;//非法时间格式
		
		try
		{
			System.TimeSpan span = new System.TimeSpan(int.Parse(strSplits[0]), int.Parse(strSplits[1]), int.Parse(strSplits[2]));
			return (int)(span.Ticks / (1000 * 10000));
		}
		catch
		{
			return -1;
		}
		
	}
	
	/// <summary>
	/// 将单位为秒的数值转化为“00:00:00”时间格式
	/// </summary>
	/// <param name="seconds">秒数</param>
	/// <returns>形式为"00:00:00"的时间字符串</returns>
	public static string ConvertSecondToTime(int seconds)
	{
		int h = seconds / 3600;
		int m = (seconds - 3600 * h) / 60;
		int s = (seconds - 3600 * h) % 60;
		
		string len = h.ToString().PadLeft(2, '0') + ":" + m.ToString().PadLeft(2, '0') + ":" + s.ToString().PadLeft(2, '0');
		
		return len;
	}

	public static void CleanMemory()
	{
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}
}
