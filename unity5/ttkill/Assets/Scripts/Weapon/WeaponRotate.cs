using UnityEngine;
using System.Collections;

public class WeaponRotate : MonoBehaviour {
	public float speed = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 0, 30) * speed);
	}
}
