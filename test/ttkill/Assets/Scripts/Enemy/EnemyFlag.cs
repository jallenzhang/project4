using UnityEngine;
using System.Collections;

public class EnemyFlag : MonoBehaviour {
	public GUITexture enemy;
	// Use this for initialization
	void Start () {
		enemy.transform.localScale = new Vector3( enemy.pixelInset.size.x / Screen.width, enemy.pixelInset.size.y / Screen.height,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
