using UnityEngine;
using System.Collections;

public class miniMapCamera : MonoBehaviour {
	[HideInInspector]
	public GameObject Target;

//	private GameObject player;
	// Use this for initialization
	void Start () {
		Target = (GameObject) GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
	}


}
