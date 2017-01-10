using UnityEngine;
using System.Collections;

public class ChangeRolePosition : MonoBehaviour {
	public Transform pos1;
	public Transform pos2;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(0.1f);
		transform.position = new Vector3((pos1.position.x + pos2.position.x) / 2, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
