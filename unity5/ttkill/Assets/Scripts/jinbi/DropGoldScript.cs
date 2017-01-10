using UnityEngine;
using System.Collections;
using PathologicalGames;

public class DropGoldScript : MonoBehaviour {
	public float duration = 4;
	private int enemy_type;
	SpawnPool spawnPool;

	// Use this for initialization
	void Start () {
		spawnPool =  PoolManager.Pools["Items"];
		StartCoroutine(PlayEffect());
	}

	public void Init(int type)
	{
		enemy_type = type;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator PlayEffect()
	{
		yield return new WaitForSeconds(duration - 1f);
		int r1 = Random.Range(0, 100);
		if (enemy_type < 100)
		{
			//xiao guai
			int r2 = Random.Range(0, 100);
			GameObject gold = (GameObject)Instantiate(Resources.Load( r2 < 70 ? "prefabs/gold/jinbi_little" : "prefabs/gold/jinbi_middle"));
//			Transform gold;
//			if (r2 < 70)
//				gold = PoolManager.Pools["Items"].Spawn("jinbi_little");
//			else
//				gold = PoolManager.Pools["Items"].Spawn("jinbi_middle");
			gold.transform.position = transform.position + new Vector3(0, 0.1f, 0);
			gold.transform.localScale = Vector3.one * 0.3f;
		}
		else if (enemy_type > 100)
		{
//			MonsterInfo monster = IOHelper.GetMonsterInfo(enemy_type);
//			int count = 100 / 10; //(int)monster.money
//
//			for (int i = 0; i < count; i++)
//			{
//				GameObject gold = (GameObject)Instantiate(Resources.Load("prefabs/gold/jinbi_large"));
//				gold.transform.position = transform.position + Quaternion.Euler(new Vector3(0,0,360f/count)) * Quaternion.Euler(Vector3.zero) * new Vector3(5, 0, 0);
//				gold.transform.localScale = Vector3.one * 0.3f;
//			}
		}

		yield return new WaitForSeconds(1);
//		spawnPool.Despawn(transform);
		Destroy(gameObject);
	}
}
