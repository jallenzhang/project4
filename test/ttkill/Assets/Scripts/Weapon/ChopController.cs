using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HeroController))]
public class ChopController : MonoBehaviour {
	public float chopAngles = 60f;
	public float distance = 3;
	public float frequence = 3;
	public float damage = 1;

	public bool chopOnHand = true;

	private bool fire = false;
	private AttackButton btnFire;
	private float lastFireTime = 0;
	private GameObject player;
	// Use this for initialization
	void Start () {
		btnFire = GameObject.Find ("joystick_right").GetComponent<AttackButton>();
	}
	
	// Update is called once per frame
	void Update () {
		if (chopOnHand)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetKeyDown(KeyCode.F))
			{
				fire = true;
			}
			else if (Input.GetKeyUp(KeyCode.F))
			{
				fire = false;
			}
#elif UNITY_IPHONE || UNITY_ANDROID

			if (btnFire.IsPressed() && !fire)
			{
				EventService.Instance.GetEvent<HeroStateChangeEvent>().Publish(AnimState.attack, transform.GetComponent<HeroController>().heroId);
			}

			if (!btnFire.IsPressed() && fire)
			{
				EventService.Instance.GetEvent<HeroStateChangeEvent>().Publish(AnimState.idle, transform.GetComponent<HeroController>().heroId);
			}

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
				if (Time.time > lastFireTime + 1f / frequence)
				{
					lastFireTime = Time.time;

					Debug.Log("current rotation is: " + transform.rotation.eulerAngles);

					Quaternion r = Quaternion.AngleAxis (chopAngles/2f, Vector3.up) * transform.rotation;
					Vector3 rPoint = r * Vector3.forward * distance + transform.position;

					Debug.DrawLine(transform.position, rPoint, Color.red, 10f);

					Quaternion l = Quaternion.AngleAxis(chopAngles / 2f, Vector3.down) * transform.rotation;
					Vector3 lPoint = l * Vector3.forward * distance + transform.position;
					Debug.DrawLine(transform.position, lPoint, Color.green, 10f);

					for (int i = EnemyManager.Instance.Enemies.Count - 1; i >=0; i--)
					{
						if (EnemyManager.Instance.Enemies[i] != null && Helper.isINTriangle(EnemyManager.Instance.Enemies[i].transform.position, transform.position, lPoint, rPoint))
						{
							Debug.Log("hit enemy!!!!!!!!!!");
							Vector3 force = transform.forward * 3;
							EnemyManager.Instance.Enemies[i].rigidbody.AddForce(force, ForceMode.Impulse);
							
							//							EventService.Instance.GetEvent<EnemyStateChangeEvent>().Publish(AnimState.take, enemy.EnemyId);
							EnemyManager.Instance.Enemies[i].onDamage(damage, EnemyManager.Instance.Enemies[i].transform.position);
						}
					}

//					foreach(EnemyController enemy in EnemyManager.Instance.Enemies)
//					{
//						if (enemy != null && Helper.isINTriangle(enemy.transform.position, transform.position, lPoint, rPoint))
//						{
//							Debug.Log("hit enemy!!!!!!!!!!");
//							Vector3 force = transform.forward * 3;
//							enemy.rigidbody.AddForce(force, ForceMode.Impulse);
//							
////							EventService.Instance.GetEvent<EnemyStateChangeEvent>().Publish(AnimState.take, enemy.EnemyId);
//							enemy.onDamage(damage, enemy.transform.position);
//						}
//					}
				}
			}
		}

	}
}
