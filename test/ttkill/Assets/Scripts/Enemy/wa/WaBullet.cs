using UnityEngine;
using System.Collections;

public class WaBullet : MonoBehaviour {
	private Transform m_parent = null;
	private Vector3 m_dir = Vector3.zero;
	private float bulletSpeed = 7;
	private float bulletDamage = 10;
	private float liftTime = 5f;
	// Use this for initialization
	void Start () {
	}

	public void Init(Transform parent, float damage)
	{
//		m_parent = parent;
		bulletDamage = damage;
//		m_dir = (transform.position - m_parent.position).normalized;
	}
	
	// Update is called once per frame
	void Update () {
//		if (m_parent != null)
		{
			transform.position += transform.forward * bulletSpeed * Time.deltaTime;
//			RaycastHit hintInfo;
//			if (Physics.Raycast (transform.position, m_dir, out hintInfo))
//			{
//				if (hintInfo.distance <= 0.1f)
//				{
//					if (hintInfo.transform.tag == "Player")
//					{
//						hintInfo.transform.GetComponent<HeroController>().GetHit(bulletDamage);
//					}
//
//					Destroy(gameObject);
//					m_parent = null;
//				}
//			}
		}
		liftTime -= Time.deltaTime;
		if (liftTime <= 0 && gameObject != null)
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
			col.transform.GetComponent<HeroController>().GetHit(bulletDamage);
		}

		if (gameObject != null)
			Destroy(gameObject);
	}
}
