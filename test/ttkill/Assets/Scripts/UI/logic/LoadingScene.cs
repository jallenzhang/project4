using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
	static string levelName;

	public static void Load(string name)
	{
		Debug.Log("load " + name);
		levelName = name;
		Application.LoadLevel("loading");
	}

	// Use this for initialization
	IEnumerator Start () {
		Debug.Log("level name " + levelName);
		yield return new WaitForSeconds(1);
		Ultilities.CleanMemory();
		yield return Application.LoadLevelAsync(levelName);
		yield return new WaitForSeconds(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
