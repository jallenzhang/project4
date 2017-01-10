using UnityEngine;
using System.Collections;

public class BeeBullet : MonoBehaviour {
	private float speed = 15;
	private float distance = 0.1f;
	private float m_attackValue = 1;
	// Use this for initialization
	void Start () {
	
	}

	public void Init(float attackRange, float attackValue)
	{
		distance = attackRange;
		m_attackValue = attackValue;
	}
	
	// Update is called once per frame
	void Update () {
		distance -= speed * Time.deltaTime;
		transform.position += transform.forward * speed * Time.deltaTime;

		RaycastHit hitinfo = transform.GetComponent<BulletRaycast>().GetHitInfo(Quaternion.Euler(Vector3.zero));
		
		if (hitinfo.transform != null && hitinfo.transform.tag != "Enemy" && hitinfo.transform != null && hitinfo.distance <= distance)
		{
			if (hitinfo.transform.tag == "Player")
			{
				hitinfo.transform.GetComponent<HeroController>().GetHit(m_attackValue);
			}
			distance = hitinfo.distance;
		}

		if (distance <= 0)
			Destroy(gameObject);
	}
}
