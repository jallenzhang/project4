using UnityEngine;
using System.Collections;

public class UIBloodArea : MonoBehaviour {
	public GameObject nomoreBloodArea;
	public float noMoreBloodSpeed = 1f;

//	public GameObject 

	private bool showNoMoreBlood =false;
//	private float mFactor = 0;

	private static UIBloodArea instance = null;

	public static UIBloodArea Instance
	{
		get
		{
			return instance;
		}
	}

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator NoMoreBloodEffect(Material mat)
	{
		float mFactor = 0;
		while(showNoMoreBlood)
		{
			mFactor += noMoreBloodSpeed * Time.deltaTime;
			float tmp = 1f - (mFactor - Mathf.Floor(mFactor));
//			mat.SetFloat("_Cutoff", Mathf.Lerp(0.5f, 1f, tmp));
//			Debug.Log("!!!!!!!!!!!!!!!!!!!! " + tmp);
//			mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(0, 1f, tmp));

			yield return null;
		}
	}

	public void ShowNoMoreBlood()
	{
//		showNoMoreBlood = true;
		nomoreBloodArea.SetActive(true);
//		UISprite[] sprites = nomoreBloodArea.GetComponentsInChildren<UISprite>();
//		foreach(UISprite sprite in sprites)
//		{
//			StartCoroutine(NoMoreBloodEffect(sprite.material));
//		}

	}

	public void HideNoMoreBlood()
	{
		nomoreBloodArea.SetActive(false);
	}
}
