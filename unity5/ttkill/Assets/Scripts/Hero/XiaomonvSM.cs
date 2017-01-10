using UnityEngine;
using System.Collections;

public class XiaomonvSM : HeroController
{
	private float animChangeDuration = 0.05f;
	void Start ()
	{
//		IOHelper.GetAvaterUpgradeInfos();
		int level = AvatarDB.Instance.GetAvatarLvById(1);
		level = Mathf.Max(1, level);
		AvatarUpgradeInfo info = IOHelper.GetAvaterUpgradeInfoByIdAndLevel(1, level);
		hp = maxHp = info.hp * (1 + hpAddtional);
		Probability = info.Probability * 100f;
		crit = info.crit;
		hitDistance = info.distance;
		Init ();
	}

	void Init ()
	{
		Initialize<AnimState> ();
		ChangeState (AnimState.idle);
	}

	protected void idle_Enter ()
	{
		//jinzhan
		if (followSmoke != null)
			followSmoke.SetActive(false);
		if (currentWeaponId < WeaponType.gun_area)
		{
			if (currentWeaponId == WeaponType.dianju)
			{
				GetComponent<Animation>().CrossFade ("gunidle");
			}
			else
			{
				GetComponent<Animation>().CrossFade("meleeidle");
			}
		}
		//持枪
		else if (currentWeaponId < WeaponType.other_area)
		{
			if (currentWeaponId < WeaponType.gun_single)
				GetComponent<Animation>().CrossFade ("gunidle");
			else if (currentWeaponId < WeaponType.gun_double)
				GetComponent<Animation>().CrossFade("doublegunidle");
		}
//		else if (currentWeaponId == WeaponType.dianju)
//		{
//			animation.CrossFade ("gunidle");
//		}
	}
	
	protected void idle_Exit ()
	{
	}

	private Vector3 oldDir = Vector3.zero;
	IEnumerator skill_Enter()
	{
		if (followSmoke != null)
			followSmoke.SetActive(false);
		speed = 0;
		oldDir = dir;
		dir = Vector3.zero;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezeRotationY;

//		if (currentWeaponId == WeaponType.chuizi)
//		{
//			animation.CrossFade("hammerattack02");
//			yield return new WaitForSeconds(1.7f);
//			ChangeState(AnimState.attack);
//		}
//		else
		{
			GetComponent<Animation>().CrossFade("meleeattack02");
			yield return new WaitForSeconds(1.36f);
			ChangeState(AnimState.attack);
		}

	}

	protected void skill_Exit()
	{
		speed = 8;
		GetComponent<Rigidbody>().constraints ^= RigidbodyConstraints.FreezeRotationY;
		dir = oldDir;
	}
	
	protected void walk_Enter ()
	{
		if (followSmoke != null)
			followSmoke.SetActive(true);
		if (!isFired)
		{
			if (currentWeaponId < WeaponType.gun_area)
			{
				if (currentWeaponId == WeaponType.dianju)
				{
					GetComponent<Animation>().CrossFade ("gunwalk");
				}
				else
				{
					GetComponent<Animation>().CrossFade ("meleewalk");
				}
			}
			else if (currentWeaponId < WeaponType.other_area)
			{
				if (currentWeaponId < WeaponType.gun_single)
					GetComponent<Animation>().CrossFade ("gunwalk");
				else if (currentWeaponId < WeaponType.gun_double)
					GetComponent<Animation>().CrossFade("doublegunidlewalk");

			}
//			else if (currentWeaponId == WeaponType.dianju)
//			{
//				animation.CrossFade ("gunwalk");
//			}
		}
		else
		{
			ChangeState(AnimState.attack);
		}

	}

	protected IEnumerator staticAttack_Enter()
	{
//		Debug.Log("############  staticAttack_Enter()");
//		animation["attack"].weight = 1;
//		animation["attack"].enabled = true;
//		yield return null;
		if (followSmoke != null)
			followSmoke.SetActive(false);
		if (currentWeaponId < WeaponType.gun_area) {
//			if (currentWeaponId == WeaponType.chuizi)
//			{
//				animation.CrossFade("hammerattack01");
//			}
//			else 
			if (currentWeaponId == WeaponType.dianju) {
				GetComponent<Animation>().CrossFade("gunattack", animChangeDuration, PlayMode.StopAll);
			}
			else
			{
				GetComponent<Animation>().CrossFade("meleeattack01", animChangeDuration, PlayMode.StopAll);
			}
		} else if (currentWeaponId < WeaponType.other_area) {
			if (currentWeaponId < WeaponType.gun_single)
			{
				if (currentWeaponId == WeaponType.gun_shouqiang)
					GetComponent<Animation>().CrossFade("gunattack", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_m4)
					GetComponent<Animation>().CrossFade("gunattack_m4", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_sandan)
					GetComponent<Animation>().CrossFade("gunattack_sandan", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_fire)
					GetComponent<Animation>().CrossFade("gunattack_fire", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_jiatelin)
					GetComponent<Animation>().CrossFade("gunattack_jiatelin", animChangeDuration, PlayMode.StopAll);
				else
					GetComponent<Animation>().CrossFade("gunattack_liudan", animChangeDuration, PlayMode.StopAll);
			}
			else
			{
				if (currentWeaponId == WeaponType.gundouble_shouqiang)
					GetComponent<Animation>().CrossFade("doublegunattack", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_m4)
					GetComponent<Animation>().CrossFade("doublegunattack_m4", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_sandan)
					GetComponent<Animation>().CrossFade("doublegunattack_sandan", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_fire)
					GetComponent<Animation>().CrossFade("doublegunattack_fire", animChangeDuration, PlayMode.StopAll);
				else
					GetComponent<Animation>().CrossFade("doublegunattack_liudan", animChangeDuration, PlayMode.StopAll);
			}

		} 
		yield return null;
	}

	protected void staticAttack_Exit ()
	{
//		animation["attack"].enabled = false;
//		animation["attackgun"].enabled = false;
	}

	IEnumerator daojuAttack_Enter()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Animation>().CrossFade("skill", animChangeDuration, PlayMode.StopAll);
		yield return new WaitForSeconds(2f);
		GetComponent<Collider>().enabled = true;
		GetComponent<Rigidbody>().useGravity = true;
		ChangeState(AnimState.idle);
	}

	void daojuAttack_Exit()
	{
		GetComponent<Collider>().enabled = true;
		GetComponent<Rigidbody>().useGravity = true;
	}
	
	protected IEnumerator attack_Enter ()
	{
//		Debug.Log("############  attack_Enter()");
//		animation["attack"].weight = 1;
//		animation["attack"].enabled = true;
//		yield return null;
		if (followSmoke != null)
			followSmoke.SetActive(true);
		if (currentWeaponId < WeaponType.gun_area) {
//			if (currentWeaponId == WeaponType.chuizi)
//				animation.CrossFade("hammerattackwalk");
//			else 
			if (currentWeaponId == WeaponType.dianju) {
				
				GetComponent<Animation>().CrossFade("gunattackwalk", animChangeDuration, PlayMode.StopAll);
			}
			else
				GetComponent<Animation>().CrossFade("meleeattackwalk", animChangeDuration, PlayMode.StopAll);
		} else if (currentWeaponId < WeaponType.other_area) {
			if (currentWeaponId < WeaponType.gun_single)
			{
				if (currentWeaponId == WeaponType.gun_shouqiang)
					GetComponent<Animation>().CrossFade("gunattackwalk", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_m4)
					GetComponent<Animation>().CrossFade("gunattackwalk_m4", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_sandan)
					GetComponent<Animation>().CrossFade("gunattackwalk_sandan", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_fire)
					GetComponent<Animation>().CrossFade("gunattackwalk_fire", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_jiatelin)
					GetComponent<Animation>().CrossFade("gunattackwalk_jiatelin", animChangeDuration, PlayMode.StopAll);
				else
					GetComponent<Animation>().CrossFade("gunattackwalk_liudan", animChangeDuration, PlayMode.StopAll);
			}
			else if (currentWeaponId < WeaponType.gun_double)
			{
				if (currentWeaponId == WeaponType.gundouble_m4)
					GetComponent<Animation>().CrossFade("doublegunattackwalk_m4", animChangeDuration, PlayMode.StopAll);	
				else if (currentWeaponId == WeaponType.gundouble_liudan)
					GetComponent<Animation>().CrossFade("doublegunattackwalk_liudan", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_fire)
					GetComponent<Animation>().CrossFade("doublegunattackwalk_fire", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_sandan)
					GetComponent<Animation>().CrossFade("doublegunattackwalk_sandan", animChangeDuration, PlayMode.StopAll);
				else
					GetComponent<Animation>().CrossFade("doublegunattackwalk", animChangeDuration, PlayMode.StopAll);
			}
		} 
//		else if (currentWeaponId == WeaponType.dianju) {
//
//			animation.CrossFade("gunattackwalk");
//		}
		yield return null;
	}
	
	protected void attack_Exit ()
	{
//		animation["attack"].enabled = false;
//		animation["attackgun"].enabled = false;
	}
	
	protected IEnumerator take_Enter ()
	{
		if (followSmoke != null)
			followSmoke.SetActive(false);
		if (currentWeaponId < WeaponType.gun_area) {
			if (currentWeaponId == WeaponType.dianju) {
				GetComponent<Animation>().CrossFade("guntake", animChangeDuration, PlayMode.StopAll);
			}
			else
				GetComponent<Animation>().CrossFade("meleetake", animChangeDuration, PlayMode.StopAll);
		} else if (currentWeaponId < WeaponType.other_area) {
			if (currentWeaponId < WeaponType.gun_single)
				GetComponent<Animation>().CrossFade("guntake", animChangeDuration, PlayMode.StopAll);
			else
				GetComponent<Animation>().CrossFade("doubleguntake", animChangeDuration, PlayMode.StopAll);
		} 
		else if (currentWeaponId == WeaponType.dianju) {
			GetComponent<Animation>().CrossFade("guntake", animChangeDuration, PlayMode.StopAll);
		}
		yield return null;
	}
	
	protected void take_Exit ()
	{
	}
	
	protected IEnumerator dead_Enter ()
	{
		GetComponent<Animation>().CrossFade ("dead", animChangeDuration, PlayMode.StopAll);
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = false;
		yield return new WaitForSeconds (2f);
//		Destroy(gameObject);
		if (!automatic)
		{

			if (GameData.Instance.DeadTime < 3)
			{
				GameData.Instance.DeadTime += 1;
				FuhuoDialog.PopUp(GameData.Instance.DeadTime);
			}
			else
			{
				Camera.main.GetComponent<GrayscaleEffect>().enabled = true;
				ResultDialog.Popup(false);
			}
		}

//		Application.LoadLevel("ui");
	}
	
	protected void dead_Exit ()
	{
		
	}
}
