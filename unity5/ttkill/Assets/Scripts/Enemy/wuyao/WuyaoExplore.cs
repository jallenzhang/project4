using UnityEngine;
using System.Collections;

public class WuyaoExplore : MonoBehaviour {
	private GameObject player;
	[HideInInspector]
	public float skill_damage;
	// Use this for initialization
	void Start () {
		player = (GameObject)GameObject.FindGameObjectWithTag("Player");
		StartCoroutine(ExploreHit());
	}

	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator ExploreHit()
	{
		yield return new WaitForSeconds(1f);
		float distance = Vector3.Distance(transform.position, player.transform.position);
		if (distance < 5f)
		{
			player.GetComponent<HeroController>().GetHit(skill_damage);
		}
	}
}
