using UnityEngine;
using System.Collections;

public class WeaponTrigger : MonoBehaviour {
	public WeaponType weaponId;
	public int currentCapacity;
	public float speed = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 30, 0) * speed);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
			//hit the new weapon and change weapon	
			EventService.Instance.GetEvent<WeaponChangeEvent>().Publish(weaponId, currentCapacity);
			Destroy(gameObject);//.transform.parent.gameObject);
		}
	}
}
