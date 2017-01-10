using UnityEngine;
using System.Collections;

public class BulletRaycast : MonoBehaviour {
	RaycastHit hintInfo;
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public RaycastHit GetHitInfo(Quaternion q)
	{
		Physics.Raycast (transform.position, q * transform.forward, out hintInfo);
		return hintInfo;
	}
}
