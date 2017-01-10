using UnityEngine;
using System.Collections;

public class bindongItem : MonoBehaviour {
	private string binBoomPrefab = "prefabs/Effects/daoju_bingdong03";
	private string binBallPrefab = "prefabs/Effects/daoju_bingdong01";
	// Use this for initialization
	IEnumerator Start () {
		TweenPosition tp = gameObject.AddComponent<TweenPosition>();
		tp.from = Vector3.zero + new Vector3(transform.position.x, 0, transform.position.z);
		tp.to = new Vector3(10, -30f, -10f) + new Vector3(transform.position.x, 0, transform.position.z);
		tp.duration = 0.6f;

		GameObject ball = (GameObject)Instantiate(Resources.Load(binBallPrefab));
		ball.transform.parent = transform;
		ball.transform.localPosition = Vector3.zero;
		ball.transform.localScale = Vector3.one;
		Destroy(ball, 1f);
		yield return new WaitForSeconds(1f);
		GameObject obj = (GameObject)Instantiate(Resources.Load(binBoomPrefab));
		obj.transform.parent = transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		Destroy(gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
