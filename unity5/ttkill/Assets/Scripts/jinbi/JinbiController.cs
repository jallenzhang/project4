using UnityEngine;
using System.Collections;
using PathologicalGames;


public class JinbiController : MonoBehaviour {
	public float speed = 1f;
	public float moveSpeed = 50f;
	public int goldValue = 0;
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
//		StartCoroutine(ChangeToTrigger());

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 10, 0) * speed);

		if (GetComponent<BoxCollider>().isTrigger)
		{
			if (player != null && Vector3.Distance(player.transform.position, transform.position) < 5f)
				transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime * moveSpeed, Space.World);
//				rigidbody.AddForce((player.transform.position - transform.position).normalized * moveSpeed, ForceMode.Acceleration);
//			else
//				rigidbody.velocity = Vector3.zero;
		}

	}

	IEnumerator ChangeToTrigger()
	{
		yield return new WaitForSeconds(0.1f);
//		rigidbody.useGravity = false;
		GetComponent<BoxCollider>().isTrigger = true;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
			//gold added
			Ultilities.gm.audioScript.coinGotFX.play();
			UIBattleSceneLogic.Instance.AddGold(goldValue);

//			PoolManager.Pools["Items"].Despawn(transform);
			Destroy(gameObject);
		}
	}
}
