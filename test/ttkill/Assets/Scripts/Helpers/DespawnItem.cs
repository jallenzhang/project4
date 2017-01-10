using UnityEngine;
using System.Collections;
using PathologicalGames;

public class DespawnItem : MonoBehaviour {
	public float duration = 1f;
	// Use this for initialization
	SpawnPool spawnPool;
	void Start () {
		spawnPool =  PoolManager.Pools["Items"];

	}

	void OnEnable()
	{
		StartCoroutine(Despawn());
	}

	IEnumerator Despawn()
	{
		yield return new WaitForSeconds(duration);
		spawnPool.Despawn(transform);
	}
}
