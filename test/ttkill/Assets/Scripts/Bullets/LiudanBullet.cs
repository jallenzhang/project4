using UnityEngine;
using System.Collections;

public class LiudanBullet : MonoBehaviour {
	public float speed = 10f;
	public float lifeTime = 1f;
	public float distance = 10;
	public float damage = 1f;
	public float m_force = 1;
	private GameObject m_spawnPoint;
	private GameObject target;
	private Vector3 tpos;
	private bool move = true;
	private float distanceToTarget = 0;
	private GameObject player;
	// Use this for initialization
	void Start () {

	}
	
	public void InitWithSpawnPoint(GameObject spawnPoint)
	{
		player = GameObject.FindGameObjectWithTag("Player");
		m_spawnPoint = spawnPoint;

//		if (EnemyManager.Instance.GetNearestEnemy(player.transform.position) == null)
//			return;

		EnemyController aimTrans = EnemyManager.Instance.GetNearestEnemy(player.transform.position);

		if (aimTrans != null && aimTrans.transform.tag == "Enemy")
		{
			target = aimTrans.gameObject;
			tpos = aimTrans.transform.position;

		}
		else
		{

			tpos= transform.forward * distance + spawnPoint.transform.position;
			Debug.Log("tpos is " + tpos);
		}
		tpos -= new Vector3(0, tpos.y, 0);
		distanceToTarget = Vector3.Distance(transform.position, tpos);
		StartCoroutine(Shoot());

	}

	IEnumerator Shoot()
	{
		while(move)
		{
			Vector3 targetPos = Vector3.zero;

			if (target != null)
			{
				tpos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
			}
			targetPos = tpos;

			transform.LookAt(targetPos);

			float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPos) / distanceToTarget) * 45f;

			this.transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);

			float currentDist = 0f;
			currentDist = Vector3.Distance(this.transform.position, targetPos);
			if (currentDist < 0.5f)
				move = false;
			this.transform.Translate (Vector3.forward * speed * Time.deltaTime);
			yield return null;
		}

		Ultilities.gm.audioScript.liudanBoomFX.play();
		Collider[] colliders = Physics.OverlapSphere(transform.position, 4.2f); 

		foreach(Collider col in colliders)
		{
			if (col.transform.tag == "Enemy")
			{
				Vector3 dir = col.transform.position - transform.position;
				Weapon weapon = WeaponController.Instance.GetWeaponByID(player.GetComponent<HeroController>().currentWeaponId);
				Vector3 force = dir.normalized * weapon.Frequency;
				if (col.rigidbody != null && col.transform.GetComponent<EnemyController>() != null)// && (int)col.transform.GetComponent<EnemyController>().currentType < 100)
				{
					col.transform.GetComponent<EnemyController>().onDamage(damage, col.transform.position);
					col.rigidbody.AddForce (force, ForceMode.Impulse);
				}
			}
		}

		GameObject boomEffect = (GameObject)Instantiate(Resources.Load("prefabs/Bullets/zidan_liudan_baozha"));
		boomEffect.transform.position = tpos;

		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {



	}
}
