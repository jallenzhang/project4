using UnityEngine;
using System.Collections;

public class FuManager : MonoBehaviour {

	public float spawnDuration = 30;

	public Transform[] spawnPositons;

	public GameObject[] fuItemPrefabs;

	GameObject lastItem;

	IEnumerator Start()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnDuration);
			SpawnNewItem();
		}
	}

	void SpawnNewItem()
	{
		if (lastItem) return;

		int index = Random.Range(0, fuItemPrefabs.Length);
//		index = 1;
		lastItem = Instantiate(fuItemPrefabs[index],
			spawnPositons[index].position + new Vector3(0, 1.5f),
			Quaternion.identity) as GameObject;
	}

}
