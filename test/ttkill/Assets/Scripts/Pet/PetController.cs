using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof(Rigidbody))]
public class PetController : StateMachineBehaviour {
	public float speed = 9;
	public float forceFollowDistance = 2;
	public float allowDistance = 2;
	public float velocitySnapness = 50.0f;
	public float turningSmoothing = 0.3f;
	public float minDistance = 0.05f;
	public PetType type = PetType.songshu;

	private Transform heroTransform;
	protected Vector3 randomPetPos = Vector3.zero;
	// Use this for initialization
	void Awake () {
		if (GameObject.FindGameObjectWithTag("Player") != null)
			heroTransform = GameObject.FindGameObjectWithTag("Player").transform;
		randomPetPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		if (Time.timeScale == 0)
			return;

		if (heroTransform == null)
			return;

		float distance = Vector3.Distance(transform.position, heroTransform.position);

		if (distance > forceFollowDistance)
		{
			randomPetPos = GenerateRandomPetMovePos();
//			rigidbody.velocity = Vector3.zero;
			ChangeState(AnimState.walk);
		}

		if (GetState().Equals(AnimState.idle))
			return;

//		else
		{
			if (Vector3.Distance(randomPetPos, transform.position) < minDistance)
			{
				ChangeState(AnimState.idle);
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;

				return;
			}
		}

		Vector3 dir = randomPetPos - transform.position;
		dir = dir.normalized;
		Vector3 targetVelocity = dir * speed;
		Vector3 deltavelocity = targetVelocity - rigidbody.velocity;
		rigidbody.AddForce(deltavelocity * velocitySnapness, ForceMode.Force);

		Vector3 faceDir = dir;
		if (faceDir == Vector3.zero) {			
			faceDir = transform.forward;
		}

		if (rigidbody.velocity == Vector3.zero)
			faceDir = transform.forward;

		float rotationAngle =  Ultilities.AngleAroundAxis (transform.forward, faceDir, Vector3.up);
		rigidbody.angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
	}

	protected Vector3 GenerateRandomPetMovePos()
	{
		if (heroTransform == null)
			return Vector3.zero;

		float x = Random.Range(-2f, 2f);
		float z = Random.Range(-2f, 2f);

		Vector3 pos_new = heroTransform.position + new Vector3(x, transform.position.y, z);
		if (Vector3.Distance(pos_new, transform.position) < minDistance)
		{
			pos_new = GenerateRandomPetMovePos();
		}

		return pos_new;
	}
}
