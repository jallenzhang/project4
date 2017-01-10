using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BulletRaycast))]
public class AutoFire : MonoBehaviour {
	public GameObject bulletPrefab;
	public float bulletDistance;
	public bool fire = false;
	public Transform spawnPoint;
	public float frequency = 10f;

	private AttackButton btnFire;
	private float lastFireTime = 0;
	private GameObject player;
	// Use this for initialization
	void Start () {
		if (spawnPoint == null)
			spawnPoint = transform;

		btnFire = GameObject.Find ("joystick_right").GetComponent<AttackButton>();
		player = (GameObject)GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetKeyDown (KeyCode.F)) {
			fire = true;
			EventService.Instance.GetEvent<FireChangeEvent>().Publish(true);
		}
		else if (Input.GetKeyUp (KeyCode.O))
		{
			fire = false;
			EventService.Instance.GetEvent<FireChangeEvent>().Publish(false);
		}
#endif

#if UNITY_IPHONE || UNITY_ANDROID
		if (btnFire.IsPressed()) {
			if (Time.timeScale == 0)
				return;

			fire = true;
			EventService.Instance.GetEvent<FireChangeEvent>().Publish(true);
		}
		else
		{
			fire = false;		
			EventService.Instance.GetEvent<FireChangeEvent>().Publish(false);
		}
#endif

		if (fire)
		{
			if (Time.time > lastFireTime + 1f / frequency)
			{
				lastFireTime = Time.time;
				GameObject bulletObj = (GameObject)Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
				SimpleBullet bullet = bulletObj.GetComponent<SimpleBullet>();

				Quaternion q = Quaternion.Euler(Vector3.zero);
				RaycastHit hitinfo = transform.GetComponent<BulletRaycast>().GetHitInfo(q);

				if (hitinfo.transform != null && hitinfo.distance <= bullet.distance)
				{
					if (hitinfo.transform.tag == "Enemy")
					{
						//hit the enemy
						Vector3 force = transform.forward * 10/frequency;
						hitinfo.rigidbody.AddForce (force, ForceMode.Impulse);
//						bullet.distance = hitinfo.distance;
						
						
						EventService.Instance.GetEvent<EnemyStateChangeEvent>().Publish(AnimState.take1, hitinfo.transform.GetComponent<EnemyController>().EnemyId);
						hitinfo.transform.GetComponent<EnemyController>().onDamage(bullet.damage, hitinfo.point);
					}
					else
					{
						Debug.Log("hitinfo's name is " + hitinfo.transform.gameObject.name);
					}
						
					bullet.distance = hitinfo.distance;
				}
			}
		}
	}
}
