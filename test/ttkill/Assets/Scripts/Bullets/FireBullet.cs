using UnityEngine;
using System.Collections;

public class FireBullet : MonoBehaviour {
	public float speed = 10f;
	public float lifeTime = 0.1f;
	public float distance = 10;
	public float damage = 1f;

	private GameObject m_spawnPoint;
	// Use this for initialization
	void Start () {
		EventService.Instance.GetEvent<FireChangeEvent> ().Subscribe (onFireChanged);
		EventService.Instance.GetEvent<WeaponChangeEvent> ().Subscribe (OnWeaponChange);
		Ultilities.gm.audioScript.fireFX.play();
	}

	void onFireChanged(bool fire)
	{
		if (!fire)
		{
			lifeTime = 0;
		}
	}

	public void InitWithSpawnPoint(GameObject spawnPoint)
	{
		m_spawnPoint = spawnPoint;
		shoot();
	}

	void shoot()
	{
//		lifeTime -= Time.deltaTime;
//		distance -= speed * Time.deltaTime;
//		transform.position += transform.forward * speed * Time.deltaTime;
		
		Vector3 targetPos = m_spawnPoint.transform.position;
		//		transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
		
		transform.position = m_spawnPoint.transform.position;
		transform.rotation = m_spawnPoint.transform.rotation; 
		Quaternion q = Quaternion.Euler(Vector3.zero);
		RaycastHit hitinfo = m_spawnPoint.GetComponent<BulletRaycast>().GetHitInfo(q);
		
		if (hitinfo.transform != null && hitinfo.distance <= distance)
		{
			if (hitinfo.transform.tag == "Enemy")
			{
				//hit the enemy
				//				Vector3 force = transform.forward * 10/currentWeapon.Frequency;
				//				hitinfo.rigidbody.AddForceAtPosition (force, hitinfo.point, ForceMode.Impulse);
				
				
				hitinfo.transform.GetComponent<EnemyController>().onDamage(damage, hitinfo.point);
			}
			else
			{
				//							Debug.Log("hitinfo's name is " + hitinfo.transform.gameObject.name);
			}
			
			//			bullet.distance = hitinfo.distance;
		}
		
//		if (lifeTime <= 0) {
//			
//			Destroy(gameObject);		
//		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnWeaponChange(WeaponType weaponId, int capacity)
	{
		lifeTime = 0;
	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<FireChangeEvent> ().Unsubscribe (onFireChanged);
		EventService.Instance.GetEvent<WeaponChangeEvent> ().Unsubscribe (OnWeaponChange);
	}
}
