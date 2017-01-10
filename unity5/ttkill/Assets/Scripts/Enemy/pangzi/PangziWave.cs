using UnityEngine;
using System.Collections;

public class PangziWave : MonoBehaviour {
	private float damage = 10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
			col.transform.GetComponent<HeroController>().GetHit(damage);
		}
	}
}
