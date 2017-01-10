using UnityEngine;
using System.Collections;

public class SimpleBullet : MonoBehaviour {
	public float speed = 10f;
	public float lifeTime = 1f;
	public float distance = 10;
	public float damage = 1f;
	public float m_force = 1;
	private GameObject m_spawnPoint;
	private bool m_hit = false;
	Quaternion m_qua;
	private GameObject player;
	// Use this for initialization
	void Start () {
//		player = GameObject.FindGameObjectWithTag("Player");
//		if (GameObject.FindGameObjectWithTag("Fake") != null)
//			player = GameObject.FindGameObjectWithTag("Fake");
	}

	public void InitWithSpawnPoint(GameObject spawnPoint, Quaternion q)
	{
		m_spawnPoint = spawnPoint;
		m_qua = q;
		player  = m_spawnPoint.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null)
		{
			Destroy(gameObject);
			return;
		}

		lifeTime -= Time.deltaTime;
		distance -= speed * Time.deltaTime;
		transform.position += transform.forward * speed * Time.deltaTime;

		RaycastHit hitinfo = transform.GetComponent<BulletRaycast>().GetHitInfo(m_qua);
		
		if (hitinfo.transform != null 
		    && hitinfo.transform.tag != "Player" 
		    && hitinfo.transform.tag != "Fake"
		    && hitinfo.transform != null
		    && hitinfo.distance <= distance
		    && hitinfo.transform.tag != "bullet")
		{

			if (hitinfo.transform.tag == "Enemy")
			{
				//hit the enemy
				Weapon weapon = WeaponController.Instance.GetWeaponByID(player.GetComponent<HeroController>().currentWeaponId);
				Vector3 force = hitinfo.transform.forward * -1 * weapon.atk / 10f;
				if (hitinfo.rigidbody != null && !m_hit)
				{
//					hitinfo.rigidbody.AddForce (force,ForceMode.Impulse);
//					hitinfo.transform.Translate(Vector3.back * (player.GetComponent<HeroController>().hitDistance + WeaponController.Instance.GetWeaponByID(player.GetComponent<HeroController>().currentWeaponId).hitDistance));
					m_hit = true;
					StartCoroutine(hitBack(hitinfo.transform, hitinfo.transform.position, hitinfo.transform.position + Vector3.back * (player.GetComponent<HeroController>().hitDistance + WeaponController.Instance.GetWeaponByID(player.GetComponent<HeroController>().currentWeaponId).hitDistance), 0.2f));
					hitinfo.transform.GetComponent<EnemyController>().onDamage(damage, hitinfo.point);
				}
			}
			else
			{
				//							Debug.Log("hitinfo's name is " + hitinfo.transform.gameObject.name);
				if (!m_hit)
				{
					m_hit = true;
					GameObject effect = (GameObject)Instantiate(Resources.Load("prefabs/Effects/zidanbeiji"));
					Ultilities.gm.audioScript.bulletHitWallFX.play();
					effect.transform.position = hitinfo.point;
				}
			}
			
			distance = hitinfo.distance;


		}

		if (distance <= 0 || lifeTime <= 0) {
			
			Destroy(gameObject);		
		}
	}

	IEnumerator hitBack(Transform target, Vector3 from, Vector3 to, float duration)
	{
		float rate = 1f / duration;
		float tmp = 0f;
		if (target == null || target.GetComponent<EnemyController>() == null || target.GetComponent<EnemyController>().HP <= 0 || (int)target.GetComponent<EnemyController>().currentType > 100)
			yield break;
		while(tmp < 1.0f)
		{
			tmp += rate * Time.deltaTime;
			target.position = Vector3.Lerp(from ,to, tmp);
			yield return null;
		}
	}
}
