using UnityEngine;
using System.Collections;

public class XueDisapper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(Disappear());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Disappear()
	{
		yield return new WaitForSeconds(2f);
		StartCoroutine(ChangeCutoff(0.5f, 1f, 2f));
	}

	IEnumerator ChangeCutoff(float from, float to, float duration)
	{
		float rate = 1f / duration;
		float tmp = 0;
		
		while(tmp < 1.0f)
		{
			tmp += rate * Time.deltaTime;
			renderer.material.SetFloat("_Cutoff", Mathf.Lerp(from, to, tmp));
			yield return null;
		}

		StartCoroutine(ChangeAlpha(1, 0, 3f));
	}

	IEnumerator ChangeAlpha(float from, float to, float duration)
	{
		float rate = 1f / duration;
		float tmp = 0;

		while(tmp < 1.0f)
		{
			tmp += rate * Time.deltaTime;
			renderer.material.SetColor("_Color", Color.Lerp(new Color(1, 1, 1, from), new Color(1, 1, 1, to), tmp));
			yield return null;
		}

		Destroy(gameObject);
	}
}
