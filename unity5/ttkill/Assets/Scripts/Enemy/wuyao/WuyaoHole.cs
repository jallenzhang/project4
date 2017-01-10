using UnityEngine;
using System.Collections;

public class WuyaoHole : MonoBehaviour {
	[HideInInspector]
	public float skill_damage;
	// Use this for initialization
	void Start () {
		StartCoroutine(ExploreHole());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator ExploreHole()
	{
		yield return new WaitForSeconds(3.0f);
		GameObject explore = (GameObject)Instantiate(Resources.Load("prefabs/Effects/boss_wuyao_skill02"));
		explore.GetComponent<WuyaoExplore>().skill_damage = skill_damage;
		explore.transform.parent = transform.parent;
		explore.transform.position = transform.position;// + new Vector3(0, 3, 0);
		Destroy(gameObject);
	}
}
